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
        //DONE - workds, fetches both films from mockdatabase and also films added via AddFilm
        [HttpGet]
        public ActionResult<IEnumerable<Film>> GetAllFilms()
        {
            var films = _filmService.GetAllFilms();
            if (films == null || !films.Any())
            {
                return NoContent();  // Returnera 204 om inga filmer finns
            }

            return Ok(films);  // Returnerar status 200 OK
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
        //DONE - works, films are added to the list
        [HttpPost]
        //[Authorize(Roles = "Admin")] // Säkerställ att endast administratörer kan anropa denna metod
        public ActionResult<IFilm> AddFilm([FromBody] ICreateFilm createFilm)
        {
            if (createFilm == null)
            {
                return BadRequest("Film data is required.");
            }

            // Skapa en ny film baserat på input
            var newFilm = new Film
            {
                FilmId = createFilm.FilmId == 0 ? _filmService.GetNewFilmId() : createFilm.FilmId,
                Title = createFilm.Title,
                Description = createFilm.Description,
                Genre = createFilm.Genre,
                Director = createFilm.Director,
                Year = createFilm.Year,
                IsAvailable = createFilm.IsAvailable,
                FilmCopies = new List<FilmCopy>()
            };

            // Skapa filmkopior baserat på AvailableCopies
            for (int i = 0; i < createFilm.AvailableCopies; i++)
            {
                newFilm.FilmCopies.Add(new FilmCopy
                {
                    Title = newFilm.Title + $" (Copy {i + 1})",
                    IsRented = false,
                    Film = newFilm,
                    FilmStudio = null // Set this to the appropriate FilmStudio object if available
                });
            }

            var addedFilm = _filmService.AddFilm(newFilm);

            return Ok(addedFilm); // Statuskod 200 och returnerar ett JSON-objekt av typen IFilm
        }


        //DELETE: api/film/id
        //Returns 204nocontent but does not remove the film from the list
        // [HttpDelete("{id}")]
        // public ActionResult DeleteFilm(int id)
        // {
        //     var film = _filmService.GetFilmById(id);
        //     if (film == null)
        //     {
        //         return NotFound(); //returnstatus 404notfound
        //     }

        //     _filmService.DeleteFilm(id);
        //     return NoContent(); //returnstatus 204nocontent
        // }


        //PATCH: api/film/id
        // [HttpPatch("{id}")]
        // [Authorize(Roles = "Admin")]
        // public IActionResult UpdateFilm(int id, [FromBody] Film updatedFilm)
        // {
        //     if (updatedFilm == null || id != updatedFilm.FilmId)
        //     {
        //         return BadRequest("Ogiltiga uppgifter.");
        //     }

        //     var existingFilm = _filmService.GetFilmById(id);
        //     if (existingFilm == null)
        //     {
        //         return NotFound($"Filmen med ID {id} hittades inte.");
        //     }

        //     return Ok(updatedFilm);
        // }

        //POST: api/filmstudio/{studioId}/rent
    [Authorize(Roles = "filmstudio")]
    [HttpPost("rent")]
    public IActionResult RentFilmToStudio(int filmId, int studioId)
    {
        var user = HttpContext.User; // Hämtar den autentiserade användaren
        bool success = _filmStudioService.RentFilmToStudio(filmId, studioId);

        if (!success)
        {
            return StatusCode(409, "Kunde inte hyra filmen."); // Generell felhantering, kan förbättras
        }
        return Ok("Film uthyrd framgångsrikt.");
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