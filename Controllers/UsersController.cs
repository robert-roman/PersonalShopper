using PersonalShopper.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [Authorize(Roles = "Admin,User")]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _unitOfWork.Users.GetAll();

            return Ok(new { users });
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
