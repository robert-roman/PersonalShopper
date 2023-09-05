using PersonalShopper.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalShopper.DAL.DTOs;
using System.Security.Claims;
using System.Linq;
using PersonalShopper.DAL.Models;
using System.Text.Json.Serialization;
using System.Text.Json;
using PersonalShopper.Services.CartProductService;

namespace PersonalShopper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartService _cartService;


        public UsersController(IUnitOfWork unitOfWork, ICartService cartService)
        {
            _unitOfWork = unitOfWork;
            _cartService = cartService;
        }

        //GET: api/Users/{email}
        [HttpGet("{email}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserProfileDTO>> GetUserByEmail(string email)
        {
            var user = await _unitOfWork.Users.GetUserByEmail(email);

            if (user == null)
            {
                return NotFound("There is no user with this email");
            }

            return new UserProfileDTO(user);
        }

        //GET: api/Users/
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserProfileDTO>>> GetAllUsers()
        {
            if (_unitOfWork.Users == null)
            {
                return NotFound();
            }
            var results = (await _unitOfWork.Users.GetAll()).Select(u => new UserProfileDTO(u)).ToList();
            return results;
        }


        //GET: api/Users/userCart/
        [HttpGet("userCart")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<CartDTO>> RefreshCart()
        {
            var currentUserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserID == null)
            {
                return Forbid("No user currently logged in");
            }

            var userCart = await _unitOfWork.Carts.GetCartById(int.Parse(currentUserID));
            await _cartService.CalculateCartPrice(userCart);

            var userCartDTO = new CartDTO(userCart);
            return userCartDTO;
        }

        //GET: api/Users/userCart/{userId}
        [HttpGet("userCart/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CartDTO>> RefreshCartUser(int userId)
        {
            var userCart = await _unitOfWork.Carts.GetCartById(userId);
            await _cartService.CalculateCartPrice(userCart);

            var userCartDTO = new CartDTO(userCart);
            return userCartDTO;
        }




        //PUT: api/Users/email/{userEmail}
        [HttpPut("email/{userEmail}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<UserRegisterDTO>> ModifyUserProfile(string userEmail, UserRegisterDTO userModified)
        {
            var currentUserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserID == null)
            {
                return Forbid("No user currently logged in");
            }
            
            var loggedUser = await _unitOfWork.Users.GetUserById(int.Parse(currentUserID));
            if (!loggedUser.Email.Equals(userEmail) && !loggedUser.UserRoles.Any(ur => ur.Role.Name == "Admin"))
            {
                return Forbid("You are not authorized to edit other users' profile");
            }

            loggedUser.Email = userModified.Email;
            loggedUser.FirstName = userModified.FirstName;
            loggedUser.LastName = userModified.LastName;

            await _unitOfWork.Users.Update(loggedUser);
            _unitOfWork.Save();

            return Ok(loggedUser);

        }

        //DELETE: api/Users/DeleteProfile{email}
        [HttpDelete("DeleteProfile{email}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] string email)
        {

            var profileDeleted = await _unitOfWork.Users.GetUserByEmail(email);

            if (profileDeleted == null)
            {
                return BadRequest("There is no user with this username");
            }

            _unitOfWork.Users.Delete(profileDeleted);
            _unitOfWork.Save();
            return Ok();
        }

    }
}
