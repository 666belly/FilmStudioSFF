using Microsoft.AspNetCore.Mvc;
using FilmStudioSFF.Services;
using FilmStudioSFF.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FilmStudioSFF.Controllers
{
    [Route("api/mystudio")]
    [ApiController]
    [Authorize(Roles = "filmstudio")] 
    public class MyStudioController : ControllerBase
    {
        private readonly FilmStudioService _filmStudioService;
        private readonly AuthenticationService _authenticationService;


        public MyStudioController(FilmStudioService filmStudioService, AuthenticationService authenticationService)
        {
            _filmStudioService = filmStudioService;
            _authenticationService = authenticationService;

        }

        // GET: api/mystudio/rentals
        // 404 not found?? Inga uthyrda filmer.??? varför
        [HttpGet("rentals")]
        public ActionResult<IEnumerable<FilmCopy>> GetRentalsForStudio()
        {
            var studioId = GetAuthenticatedStudioId();
            if (!studioId.HasValue)
            {
                return Unauthorized("Behörighet krävs.");
            }

            var rentals = _filmStudioService.GetRentalsForStudio(studioId.Value);
            if (rentals == null || !rentals.Any())
            {
                return NotFound("Inga uthyrda filmer.");
            }

            return Ok(rentals);
        }

        private int? GetAuthenticatedStudioId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int studioId))
            {
                return studioId;
            }
            return null;
        }
    }
}
