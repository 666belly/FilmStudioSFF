using FilmStudioSFF.Models;
using FilmStudioSFF.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;

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
        [HttpGet("{id}")]   
        public IActionResult GetFilmStudioById(int id)
        {
            var studio = _filmStudioService.GetFilmStudioById(id);
            if (studio == null)
            {
                return NotFound($"Filmstudion med ID {id} hittades inte");
            }

            return Ok(studio); //200 OK
        }

        //GET all filmstudios
        [HttpGet] 
        public IActionResult GetAllFilmStudios()
        {
            Debug.WriteLine("GET request received for all film studios");
            var studios = _filmStudioService.GetAllFilmStudios();
            return Ok(studios);
        }


        // GET rented film copies for a specific film studio
        [HttpGet("{studioId}/rented-films")]
        // [Authorize(Roles = "filmstudio")]
        public IActionResult GetRentedFilmCopies(int studioId)
        {
            var studio = _filmStudioService.GetFilmStudioById(studioId);
            if (studio == null)
            {
                return NotFound($"Filmstudion med ID {studioId} hittades inte");
            }

            return Ok(studio.RentedFilms ?? new List<FilmCopy>());
        }

    }
}