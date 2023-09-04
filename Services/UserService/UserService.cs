using PersonalShopper.DAL.DTOs;
using PersonalShopper.DAL.Models.Constants;
using PersonalShopper.DAL.Models;
using Microsoft.AspNetCore.Identity;
using PersonalShopper.DAL.Repositories.AuthWrapperRepository;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PersonalShopper.Repositories.UnitOfWork;

namespace PersonalShopper.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthWrapperRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IAuthWrapperRepository repository, UserManager<User> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> RegisterUserAsync(UserRegisterDTO dto)
        {
            var registerUser = new User
            {
                Email = dto.Email,
                UserName = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Age = dto.Age
            };

            var result = await _userManager.CreateAsync(registerUser, dto.Password);

            if (result.Succeeded)
            {
                var cart = new Cart
                {
                    User = registerUser
                };

                await _unitOfWork.Carts.Create(cart);
                _unitOfWork.Save();
                registerUser.Cart = cart;
                await _userManager.UpdateAsync(registerUser);


                await _userManager.AddToRoleAsync(registerUser, UserRoleType.User);

                return true;
            }

            return false;
        }


        public async Task<string> LoginUser(UserLoginDTO dto)
        {
            User user = await _userManager.FindByEmailAsync(dto.Email);

            if (user != null)
            {
                user = await _repository.User.GetUserAndUserRoleById(user.Id);
                List<string> roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();

                var newJti = Guid.NewGuid().ToString();
                var tokenHandler = new JwtSecurityTokenHandler();
                var signinkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom secret key for auth"));

                var token = GenerateJwtToken(signinkey, user, roles, tokenHandler, newJti);

                _repository.SessionToken.Create(new SessionToken(newJti, user.Id, token.ValidTo));
                await _repository.SaveAsync();

                return tokenHandler.WriteToken(token);
            }


            return null;
        }

        private SecurityToken GenerateJwtToken(SymmetricSecurityKey signinkey, User user, List<string> roles, JwtSecurityTokenHandler tokenHandler, string newJti)
        {

            var subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName + " " + user.Age + " " + user.PhoneNumber),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, newJti)
            });

            foreach (var role in roles)
            {
                subject.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = subject,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(signinkey, SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return token;
        }
    }
}
