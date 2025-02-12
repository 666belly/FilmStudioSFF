using Microsoft.AspNetCore.Mvc;
using FilmStudioSFF.Services;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using FilmStudioSFF.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Text.Json;

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
        //Kinda done
        // Fetches the films from the database but not new films added via POST AddFilm? 
        [HttpGet]
        public ActionResult<IEnumerable<Film>> GetAllFilms()
        {
            var films = _filmService.GetAllFilms();
            if (films == null || !films.Any())
            {
                return NoContent();  // Returnera 204 om inga filmer finns
            }

            return Ok(films);  //returnstatus 200ok
        }

        //GET: api/film/id
        //DONE - works
        //Fetches a film by ID from the database
        [HttpGet("{id}")]
        public ActionResult<Film> GetFilmById(int id)
        {

            Console.WriteLine($"Received GET request for film with ID {id}");

            var film = _filmService.GetFilmById(id);
            if (film == null)
            {
                return NotFound("Det finns ingen film med detta ID."); // Return status 404 if the film is not found
            }

            return Ok(film); //returnstatus 200ok
        }

        // POST: api/films
[HttpPost]
public ActionResult<Film> AddFilm([FromBody] Film newFilm)
{
    Console.WriteLine($"Received: {JsonSerializer.Serialize(newFilm)}");

    if (newFilm == null)
    {
        return BadRequest("Film data is required.");
    }

    var addedFilm = _filmService.AddFilm(newFilm);
    return CreatedAtAction(nameof(GetAllFilms), new { id = addedFilm.FilmId }, addedFilm);
}

        //DELETE: api/film/id
        //Returns 204nocontent but does not remove the film from the list
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

            return Ok(updatedFilm);
        }

        //POST: api/filmstudio/{studioId}/rent
        //Returns 200ok but does not rent the film to the studio, does not add to list
        [HttpPost("{studioId}/rent")]
        public IActionResult RentFilmToStudio(int studioId, [FromBody] FilmCopy filmCopy)
        {
            Console.WriteLine($"Received rent request for studio ID: {studioId} and FilmCopy ID: {filmCopy.FilmCopyId}");
            
            if (filmCopy == null)
            {
                Console.WriteLine("Film copy is null.");
                return BadRequest("Ogiltig filmkopia.");
            }

            // Kontrollera om filmkopian finns i systemet och inte redan är uthyrd
            var film = _filmService.GetFilmById(filmCopy.FilmCopyId);
            if (film == null)
            {
                Console.WriteLine($"Film with ID {filmCopy.FilmCopyId} not found.");
                return NotFound("Filmen kunde inte hittas.");
            }

            var filmCopyToRent = film.FilmCopies.FirstOrDefault(fc => fc.FilmCopyId == filmCopy.FilmCopyId);
            if (filmCopyToRent == null)
            {
                Console.WriteLine($"Film copy with ID {filmCopy.FilmCopyId} not found.");
                return NotFound($"Filmkopia med ID {filmCopy.FilmCopyId} hittades inte.");
            }

            if (filmCopyToRent.IsRented)
            {
                Console.WriteLine($"Film copy {filmCopy.FilmCopyId} is already rented.");
                return BadRequest("Filmkopian är redan uthyrd.");
            }

            // Kontrollera om filmstudion existerar
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var isAdmin = userRole == "admin";
            var studio = _filmStudioService.GetFilmStudioById(studioId, userRole, isAdmin);
            if (studio == null)
            {
                Console.WriteLine($"Studio with ID {studioId} not found.");
                return NotFound($"Filmstudio med ID {studioId} hittades inte.");
            }

            // Om allting är korrekt, markera filmen som uthyrd
            filmCopyToRent.IsRented = true;
            Console.WriteLine($"Film copy {filmCopy.FilmCopyId} rented successfully to studio {studioId}");

            return Ok($"Filmkopian hyrdes ut till filmstudio med ID {studioId}.");
        }

        //POST: api/filmstudio/return
        //save to test
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