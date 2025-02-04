using Microsoft.AspNetCore.Mvc;
using FilmStudioSFF.Models;
using FilmStudioSFF.Services;
using FilmStudioSFF.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;


namespace FilmStudioSFF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly FilmStudioSFF.Services.AuthenticationService _authService;

        public UserController(UserService userService, FilmStudioSFF.Services.AuthenticationService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        //POST api/user/register
        [HttpPost("register")]  
        public ActionResult<User> RegisterUser([FromBody] IUserRegister userRegister)
        {
            if (userRegister == null)
            {
                return BadRequest("Invalid user data."); // 400 Bad Request
            }

            var user = _userService.RegisterUser(userRegister);
            if (user == null)
            {
                return Conflict ("User already exists."); // 409 Conflict
            }

            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user); // 201 Created
        }

        //POST api/user/login
        [HttpPost("login")]
        public ActionResult<string> Login([FromBody] IUserAuthenticate loginRequest)
        {
            var user = _userService.AuthenticateUser(loginRequest);
            if (user == null)
            {
                return Unauthorized("Invalid username or password."); // 401 Unauthorized
            }

            var token = _authService.GenerateJwtToken(user);
            return Ok(new { Token = token }); // 200 OK
        }

        //GET: api/user/id (get one user, admin can get all)
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<User> GetUser(int id)
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (!int.TryParse(userIdClaim, out var requestingUserId))
            {
                return Unauthorized("Invalid user ID."); // 401 Unauthorized
            }
            var requestingUserRole = User.FindFirst("role")?.Value; 

            if (requestingUserRole != "admin" && requestingUserId != id)
            {
                return Forbid(); // 403 Forbidden
            }

            var user = _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound(); // 404 Not Found
            }        

            return Ok(user); // 200 OK  
        }

        //GET: api/user/all (only admin)
        [HttpGet("all")]
        [Authorize (Roles = "admin")]
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            return Ok(users); // 200 OK
        } 
    }
}