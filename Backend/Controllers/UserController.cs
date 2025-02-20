// using Microsoft.AspNetCore.Mvc;
// using FilmStudioSFF.Models;
// using FilmStudioSFF.Services;
// using FilmStudioSFF.Interfaces;
// using Microsoft.AspNetCore.Authorization;
// using System.IdentityModel.Tokens.Jwt;
// using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
// using System.Security.Claims;


// namespace FilmStudioSFF.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class UserController : ControllerBase
//     {
//         private readonly UserService _userService;
        
//         private readonly FilmStudioSFF.Services.AuthenticationService _authService;

//         public UserController(UserService userService, FilmStudioSFF.Services.AuthenticationService authService)
//         {
//             _userService = userService;
//             _authService = authService;
//         }

//         //POST api/user/register
//         // DONE - works
//         [HttpPost("register")]  
//         public ActionResult<UserRegister> RegisterUser([FromBody] UserRegister userRegister)
//         {
//             if (userRegister == null)
//             {
//                 return BadRequest("Invalid user data."); // 400 Bad Request
//             }

//             var user = _userService.RegisterUser(userRegister);
//             if (user == null)
//             {
//                 return Conflict ("User already exists."); // 409 Conflict
//             }

//             return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user); // 201 Created
//         }

//         //POST api/user/login
//         // DONE - works
//         [HttpPost("authenticate")]
//         public ActionResult Authenticate([FromBody] UserAuthenticate loginRequest)
//         {
//             var user = _userService.AuthenticateUser(loginRequest);
//             if (user is null)
//             {
//                 return Unauthorized(new { message = "Invalid username or password." });
//             }

//             var token = _authService.GenerateJwtToken(user.Username, user.Role, user.UserId);
//             return Ok(new { token, role = user.Role });
//         }

//         //GET: api/user/id (get one user, admin can get all)
//         // 401 unauth Invalid user ID, auth doesnt work?
//         [HttpGet("{id}")]
//         [Authorize(Roles = "admin")]
//         public ActionResult<UserRegister> GetUser(int id)
//         {
//             var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
//             if (userIdClaim == null || !int.TryParse(userIdClaim, out var requestingUserId))
//             {
//                 return Unauthorized("Invalid user ID.");
//             }

//             var requestingUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

//             if (requestingUserRole != "admin" && requestingUserId != id)
//             {
//                 return Forbid();
//             }

//             var user = _userService.GetUserById(id);
//             if (user == null)
//             {
//                 return NotFound();
//             }

//             return Ok(user);
//         }

//         //GET: api/user/all (only admin)
//         //DONE - works
//         [HttpGet("all")]
//         [Authorize (Roles = "admin")]
//         public ActionResult<IEnumerable<UserRegister>> GetAllUsers()
//         {
//             var users = _userService.GetAllUsers();
//             return Ok(users); // 200 OK
//         } 
//     }
// }

using Microsoft.AspNetCore.Mvc;
using FilmStudioSFF.Models;
using FilmStudioSFF.Services;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FilmStudioSFF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly AuthenticationService _authService;

        public UserController(UserService userService, AuthenticationService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        // POST: api/user/register
        [HttpPost("register")]
        public ActionResult<UserRegister> RegisterUser([FromBody] UserRegister userRegister)
        {
            if (userRegister == null)
            {
                return BadRequest("Invalid user data.");
            }

            var user = _userService.RegisterUser(userRegister);
            if (user == null)
            {
                return Conflict("User already exists.");
            }

            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
        }

        // POST: api/user/authenticate
        [HttpPost("authenticate")]
        public ActionResult Authenticate([FromBody] UserAuthenticate loginRequest)
        {
            var user = _userService.AuthenticateUser(loginRequest);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            var token = _authService.GenerateJwtToken(user.Username, user.Role, user.UserId);
            return Ok(new { token, role = user.Role });
        }

        // GET: api/user/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public ActionResult<UserRegister> GetUser(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out var requestingUserId))
            {
                return Unauthorized("Invalid user ID.");
            }

            var requestingUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (requestingUserRole != "admin" && requestingUserId != id)
            {
                return Forbid();
            }

            var user = _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // GET: api/user/all
        [HttpGet("all")]
        [Authorize(Roles = "admin")]
        public ActionResult<IEnumerable<UserRegister>> GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            return Ok(users);
        }
    }
}