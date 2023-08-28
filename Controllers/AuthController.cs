using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalShopper.DAL.DTOs;
using PersonalShopper.DAL.Models;
using PersonalShopper.DAL;
using System.Security.Claims;
using System.Security.Cryptography;
using PersonalShopper.Services;

namespace PersonalShopper.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;
        //public static User userLoggedIn = null;
        public AuthController(
            UserManager<User> userManager,
            IUserService userService,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _userService = userService;
            _context = context;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user != null)
            {
                return BadRequest("The user already exists!");
            }

            var result = await _userService.RegisterUserAsync(dto);

            if (result)
            {
                return Ok(result);
            }

            return BadRequest();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO dto)
        {
            var authtoken = await _userService.LoginUser(dto);

            if (authtoken == null)
            {
                return Unauthorized();
            }

            var user = await _userManager.FindByEmailAsync(dto.Email);

            var refreshToken = await GenerateRefreshTokenAsync();
            await SetRefreshTokenAsync(refreshToken, user);

            //userLoggedIn = user;
            return Ok(new { authtoken });
        }


        private async Task<RefreshToken> GenerateRefreshTokenAsync()
        {
            var refreshToken = new RefreshToken()
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddMinutes(2),
                Created = DateTime.Now
            };

            return refreshToken;
        }

        private async Task SetRefreshTokenAsync(RefreshToken newRefreshToken, User user)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };

            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;

            var refreshTokenEntity = new RefreshToken
            {
                Token = newRefreshToken.Token,
                UserId = user.Id,
                Created = newRefreshToken.Created,
                Expires = newRefreshToken.Expires
            };

            _context.RefreshTokens.Add(refreshTokenEntity);

            await _context.SaveChangesAsync();
        }


        [HttpPost("refresh-token")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return BadRequest("The user doesn't exist!");
            }

            var refreshToken = Request.Cookies["refreshToken"];

            if (user.RefreshToken != refreshToken)
            {
                return Unauthorized("Invalid Refresh Token");
            }
            else if (user.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token Expired");
            }

            UserLoginDTO dto = new UserLoginDTO();
            dto.Email = user.Email;
            dto.Password = user.PasswordHash;
            var authtoken = await _userService.LoginUser(dto);

            var newRefreshToken = await GenerateRefreshTokenAsync();
            await SetRefreshTokenAsync(newRefreshToken, user);

            return Ok(authtoken);
        }



    }
}
