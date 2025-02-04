using Microsoft.AspNetCore.Mvc;
using FilmStudioSFF.Models;
using FilmStudioSFF.Services;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace FilmStudioSFF.Controllers
{
    [Route ("api/[controller]")]
    [ApiController] 
    public class FilmController : Controller
    {
        private readonly FilmService _filmService;

        public FilmController(FilmService filmService)
        {
            _filmService = filmService;
        }

        //GET: api/film
        [HttpGet]
        public ActionResult<IEnumerable<Film>> GetAllFilms()
        {
            var filmService = _filmService.GetAllFilms();
            if (filmService == null)
            {
                return NotFound();
            }

            return Ok(filmService); //returnstatus 200ok
        }

        //GET: api/film/id
        [HttpGet("{id}")]
        public ActionResult<Film> GetFilmById(int id)
        {
            var film = _filmService.GetFilmById(id);
            if (film == null)
            {
                return NotFound(); //returnstatus 404notfound
            }

            return Ok(film); //returnstatus 200ok
        }

        //POST: api/film
        [HttpPost]
        public ActionResult<Film> AddFilm([FromBody] Film newFilm)
        {
            if (newFilm == null)
            {
                return BadRequest(); //returnstatus 400badrequest
            }

            _filmService.AddFilm(newFilm);
            return CreatedAtAction(nameof(GetFilmById), new { id = newFilm.FilmId }, newFilm); //returnstatus 201created
        }
        
        //DELETE: api/film/id
        [HttpDelete("{id}")]
        public ActionResult DeleteFilm(int id)
        {
            var film = _filmService.GetFilmById(id);
            if (film == null)
            {
                return NotFound(); //returnstatus 404notfound
            }

            _filmService.DeleteFilm(id);
            return NoContent(); //returnstatus 204nocontent
        }

    }    
}