using Microsoft.AspNetCore.Mvc;
using FilmStudioSFF.Services;
using FilmStudioSFF.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace FilmStudioSFF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmController : ControllerBase
    {
        private readonly FilmService _filmService;
        private readonly FilmStudioService _filmStudioService;

        public FilmController(FilmService filmService, FilmStudioService filmStudioService)
        {
            _filmStudioService = filmStudioService;
            _filmService = filmService;
        }

        // GET: api/film
        //DONE WORKS
        //both for unauthorized, admin and filmstudio to access
        [HttpGet]
        public ActionResult<IEnumerable<Film>> GetAllFilms()
        {
            var films = _filmService.GetAllFilms();
            if (films == null || !films.Any())
            {
                return NoContent();
            }

            return Ok(films);
        }

        // GET: api/film/{id}
        //DONE WORKS
        [HttpGet("{id}")]
        public ActionResult<Film> GetFilmById(int id)
        {
            var film = _filmService.GetFilmById(id);
            if (film == null)
            {
                return NotFound("Det finns ingen film med detta ID.");
            }

            return Ok(film);
        }

        // POST: api/films
        //DONE WORKS
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult<IFilm> AddFilm([FromBody] CreateFilm createFilm)
        {
            if (createFilm == null)
            {
                return BadRequest("Film data is required.");
            }

            var newFilm = new Film
            {
                FilmId = _filmService.GetNewFilmId(),
                Title = createFilm.Title,
                Description = createFilm.Description,
                Genre = createFilm.Genre,
                Director = createFilm.Director,
                Year = createFilm.Year,
                IsAvailable = createFilm.IsAvailable,
                FilmCopies = new List<FilmCopy>()
            };

            var addedFilm = _filmService.AddFilm(newFilm);

            return Ok(addedFilm);
        }

        // DELETE: api/film/{id}
        //DONE WORKS
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteFilm(int id)
        {
            var film = _filmService.GetFilmById(id);
            if (film == null)
            {
                return NotFound();
            }

            _filmService.DeleteFilm(id);
            return NoContent();
        }

        // PATCH: api/film/{id}
        [HttpPatch("{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult UpdateFilm(int id, [FromBody] Film updatedFilm)
        {
            if (updatedFilm == null || id != updatedFilm.FilmId)
            {
                return BadRequest("Ogiltiga uppgifter.");
            }

            var existingFilm = _filmService.GetFilmById(id);
            if (existingFilm == null)
            {
                return NotFound($"Filmen med ID {id} hittades inte.");
            }

            _filmService.UpdateFilm(updatedFilm);
            return Ok(updatedFilm);
        }

        // POST: api/filmstudio/{studioId}/rent
        //DONE WORKS
        [Authorize(Roles = "filmstudio")]
        [HttpPost("rent")]
        public IActionResult RentFilmToStudio(int filmId, int studioId)
        {
            var user = HttpContext.User; 
            bool success = _filmStudioService.RentFilmToStudio(filmId, studioId);

            if (!success)
            {
                return StatusCode(409, "Kunde inte hyra filmen.");
            }

            return Ok("Film uthyrd framgångsrikt.");
        }

        // POST: api/filmstudio/return
        [HttpPost("return")]
        [Authorize(Roles = "filmstudio")]
        public IActionResult ReturnFilm([FromBody] ReturnRequest returnRequest)
        {
            if (returnRequest == null || returnRequest.FilmCopyId <= 0)
            {
                return BadRequest("Ogiltig begäran.");
            }

            var studioId = GetAuthenticatedStudioId(User);
            if (studioId == null)
            {
                return Unauthorized("Du måste vara inloggad som filmstudio.");
            }

            var success = _filmStudioService.ReturnRequest(studioId.Value, returnRequest.FilmCopyId);
            if (!success)
            {
                return NotFound("Filmen kunde inte returneras.");
            }

            return Ok("Filmen har returnerats.");
        }

        private int? GetAuthenticatedStudioId(ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int studioId))
            {
                return studioId;
            }

            return null;
        }
    }
}