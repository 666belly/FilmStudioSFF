using FilmStudioSFF.Models;
using FilmStudioSFF.Services;
using Microsoft.AspNetCore.Mvc;

namespace FilmStudioSFF.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class FilmStudioController : ControllerBase
    {
        private readonly FilmStudioService _filmStudioService;

        public FilmStudioController(FilmStudioService filmStudioService)
        {
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

        public List<FilmCopy> GetRentedFilmCopies(int studioId)
        {
            var studio = _filmStudioService.GetFilmStudioById(studioId);
            return studio?.RentedFilms ?? new List<FilmCopy>();
        }

    }
}