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
        public IActionResult RegisterFilmStudio([FromBody] FilmStudio filmStudio)
        {
            if (filmStudio == null)
            {
                return BadRequest("Ogiltig input");
            }

            var newStudio = _filmStudioService.RegisterFilmStudio(filmStudio);
            if (newStudio == null)
            {
                return Conflict ("Filmstudion finns redan");
            }

            return Ok(newStudio);
        }

        //POST login filmstudio
        //api/filmstudio/login
        // DONE - works
        [HttpPost("login")]
        public IActionResult LoginFilmStudio([FromBody] FilmStudioLogin loginModel)
        {
            if (loginModel == null)
            {
                return BadRequest("Ogiltig input");
            }

            var studio = _filmStudioService.FilmStudioLogin(loginModel.Username, loginModel.Password);
            if (studio == null)
            {
                return Unauthorized("Felaktigt användarnamn eller lösenord");
            }

            var token = _authService.GenerateJwtToken(studio.Username, studio.Role, studio.FilmStudioId);
            
            return Ok(new { filmStudio = studio, token });
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
        [Authorize]
        public IActionResult GetAllFilmStudios()
        {
            //check if user is admin
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            bool includeFullDetails = userRole == "admin"; // if admin include full details

            var studios = _filmStudioService.GetAllFilmStudios(userRole, includeFullDetails);

            return Ok(studios);
        }

        // GET rented film copies for a specific film studio
        // api/filmstudio/{studioId}/rented-films
        // 200 ok but doesnt fetch list corretly??? 
        [HttpGet("{studioId}/rented-films")]
        [Authorize(Roles = "filmstudio")]
        public IActionResult GetRentedFilms(int studioId)
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            bool includeFullDetails = userRole == "admin"; 
            var studio = _filmStudioService.GetFilmStudioById(studioId, userRole, includeFullDetails);
            if (studio == null)
            {
                return NotFound($"Filmstudion med ID {studioId} hittades inte");
            }

            return Ok(studio.RentedFilms ?? new List<FilmCopy>());
        }

    }
}