using FilmStudioSFF.Models;
using FilmStudioSFF.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FilmStudioSFF.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class FilmStudioController : ControllerBase
    {
        private readonly FilmStudioService _filmStudioService;
        private readonly FilmStudioSFF.Services.AuthenticationService _authService;
        public FilmStudioController(FilmStudioService filmStudioService, AuthenticationService authService)
        {
            _authService = authService;
            _filmStudioService = filmStudioService;
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

            // Generera JWT-token för den inloggade användaren
            var token = _authService.GenerateJwtToken(studio.Username, studio.Role, studio.FilmStudioId);
            
            // Skicka tillbaka både filmstudion och JWT-token
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
        [Authorize(Roles = "admin")]
        public IActionResult GetAllFilmStudios()
        {
            var studios = _filmStudioService.GetAllFilmStudios();
            return Ok(studios);
        }

        //POST rent a filmcopy to a filmstudio
        [HttpPost("{studioId}/rent")]
        public IActionResult RentFilmToStudio(int studioId, [FromBody] FilmCopy filmCopy)
        {
            if (filmCopy == null)
            {
                return BadRequest("Ogiltig film.");
            }

            var success = _filmStudioService.RentFilmToStudio(studioId, filmCopy);
            if (!success)
            {
                return NotFound("Filmstudion hittades inte.");
            }

            return Ok("Filmen har hyrts ut.");
        }

        // GET rented film copies for a specific film studio
        [HttpGet("{studioId}/rented-films")]
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