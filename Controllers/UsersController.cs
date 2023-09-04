using PersonalShopper.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalShopper.DAL.DTOs;
using System.Security.Claims;
using System.Linq;
using PersonalShopper.DAL.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace PersonalShopper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //GET: api/Users/{email}
        [HttpGet("{email}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _unitOfWork.Users.GetUserByEmail(email);

            if (user == null)
            {
                return NotFound("There is no user with this email");
            }

            return Ok(user);
        }

        //GET: api/Users/
        [HttpGet]
        [AllowAnonymous]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _unitOfWork.Users.GetAll();

            return Ok(new { users });
        }

        //GET: api/Users/carts
        [HttpGet("cart")]
        [AllowAnonymous]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllCarts()
        {
            var carts = await _unitOfWork.Carts.GetAll();

            return Ok(new { carts });
        }

        //GET: api/Users/userCart/
        [HttpGet("userCart")]
        //[AllowAnonymous]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<Cart>> RefreshUserCart()
        {
            var currentUserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserID == null)
            {
                return Forbid("No user currently logged in");
            }

            //var loggedUser = await _unitOfWork.Users.GetUserAndUserRoleById(int.Parse(currentUserID));
            var userCart = await _unitOfWork.Carts.GetCartById(int.Parse(currentUserID));
            //loggedUser.Cart = userCart;

            /*await _unitOfWork.Users.Update(loggedUser);
            _unitOfWork.Save();*/

            return Ok();
        }

        /*// PUT: api/Users/updateUserCart
        [HttpPut("updateUserCart")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<Cart>> UpdateUserCart()
        {
            var currentUserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserID == null)
            {
                return Forbid("No user currently logged in");
            }

            var userCart = await _unitOfWork.Carts.GetCartById(int.Parse(currentUserID));

            if (userCart == null)
            {
                return NotFound("Cart not found for the current user");
            }

            var cartProductsList = await _unitOfWork.CartProducts.GetProductsByCartId(userCart.UserId);
            userCart.CartProducts = new HashSet<CartProduct>(cartProductsList);

            var loggedUser = await _unitOfWork.Users.GetUserAndUserRoleById(int.Parse(currentUserID));
            userCart.User = loggedUser;

            await _unitOfWork.Carts.Update(userCart);
            _unitOfWork.Save();

            // Configure JSON serialization options to ignore circular references
            var jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                MaxDepth = 32 // Adjust the max depth as needed
            };

            var json = JsonSerializer.Serialize(userCart, jsonOptions);

            return Content(json, "application/json");
        }*/


        [HttpPut("updateUserCart")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> UpdateUserCart()
        {
            var currentUserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserID == null)
            {
                return Forbid("No user currently logged in");
            }

            var userCart = await _unitOfWork.Carts.GetCartById(int.Parse(currentUserID));


            if (userCart == null)
            {
                return NotFound("Cart not found for the current user");
            }

            var cartProductsList = await _unitOfWork.CartProducts.GetProductsByCartId(userCart.CartId);
            userCart.CartProducts = cartProductsList;

            userCart.CartPrice = 1047;

            await _unitOfWork.Carts.Update(userCart);
            var result =  _unitOfWork.Save();
            return Ok();
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
            
            var loggedUser = await _unitOfWork.Users.GetUserAndUserRoleById(int.Parse(currentUserID));
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
