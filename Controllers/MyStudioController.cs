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

        [HttpGet("rentals")]
        public IActionResult GetRentedFilms()
        {
            var studioId = GetAuthenticatedStudioId();
            if (studioId == null)
            {
                return Unauthorized("Du måste vara inloggad som filmstudio.");
            }

            var rentedFilms = _filmStudioService.GetRentedFilms(studioId.Value);
            return Ok(rentedFilms);
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
