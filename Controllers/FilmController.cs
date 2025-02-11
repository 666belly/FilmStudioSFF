using Microsoft.AspNetCore.Mvc;
using FilmStudioSFF.Services;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using FilmStudioSFF.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FilmStudioSFF.Controllers
{
    [Route ("api/[controller]")]
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

            Console.WriteLine($"Received GET request for film with ID {id}");

            var film = _filmService.GetFilmById(id);
            if (film == null)
            {
                return NotFound(); // Return status 404 if the film is not found
            }

            return Ok(film); //returnstatus 200ok
        }

        //POST: api/film
        [HttpPost]
        // [Authorize(Roles = "Admin")]
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


        //PATCH: api/film/id
        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
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

            _filmService.UpdateFilm(updatedFilm, id);
            return Ok(updatedFilm);
        }

        //POST: api/filmstudio/{studioId}/rent
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

        //POST: api/filmstudio/return
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

        private int? GetAuthenticatedStudioId(System.Security.Claims.ClaimsPrincipal user)
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