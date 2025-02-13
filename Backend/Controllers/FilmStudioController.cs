using FilmStudioSFF.Models;
using FilmStudioSFF.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using System.Security.Claims;

namespace FilmStudioSFF.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class FilmStudioController : ControllerBase
    {
        private readonly FilmStudioService _filmStudioService;
        private readonly FilmStudioSFF.Services.AuthenticationService _authService;
        private readonly ILogger<FilmStudioController> _logger;

        public FilmStudioController(FilmStudioService filmStudioService, AuthenticationService authService, ILogger<FilmStudioController> logger)
        {
            _logger = logger;
            _filmStudioService = filmStudioService;
            _authService = authService;
        }
        

        //POST register new filmstudio
        //api/filmstudio/register
        // DONE - works

        [HttpPost("register")]
        public ActionResult<IRegisterFilmStudio> Register([FromBody] FilmStudio filmStudio)
        {
            if (filmStudio == null)
            {
                return BadRequest("Film studio data is required.");
            }

            var registeredFilmStudio = _filmStudioService.RegisterFilmStudio(filmStudio);

            return Ok(registeredFilmStudio);
        }


        [HttpPost("login")]
        public ActionResult<FilmStudioLoginResponse> LoginFilmStudio([FromBody] FilmStudioLogin loginModel)
        {
            if (loginModel == null)
            {
                return BadRequest("Login data is required.");
            }

            var studioLoginResponse = _filmStudioService.FilmStudioLogin(loginModel.Username, loginModel.Password);
            if (studioLoginResponse == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            return Ok(studioLoginResponse);
        }

        //GET specific filmstudio based on id
        //api/filmstudio/id
        // DONE - works
        [HttpGet("{id}")]   
        public IActionResult GetFilmStudioById(int id)
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            bool includeFullDetails = userRole == "admin"; // if admin include full details

            var studio = _filmStudioService.GetFilmStudioById(id, userRole, includeFullDetails);

            return Ok(studio);
        }

        //GET all filmstudios
        //api/filmstudio
        // DONE - works
        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult GetAllFilmStudios()
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            bool includeFullDetails = userRole == "admin"; 

            var studios = _filmStudioService.GetAllFilmStudios(userRole, includeFullDetails);

            return Ok(studios);
        }

        // GET rented film copies for a specific film studio
        // api/filmstudio/{studioId}/rented-films
        // 200 ok but doesnt fetch list corretly??? 
        [HttpGet("{studioId}/rented-films")]
        //[Authorize(Roles = "filmstudio")]
        public IActionResult GetRentedFilms(int studioId)
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            bool includeFullDetails = userRole == "admin";

            var rentedFilms = _filmStudioService.GetRentedFilms(studioId);

            if (rentedFilms == null || !rentedFilms.Any())
            {
                return NotFound($"No rented films found for studio ID {studioId}");
            }

            return Ok(rentedFilms);
        }


    }
}