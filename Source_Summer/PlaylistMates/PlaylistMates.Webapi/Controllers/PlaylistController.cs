using System.Security.Claims;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlaylistMates.Webapi.Services;
using PlaylistMates.Application.Infrastructure;
using PlaylistMates.Application.Model;

namespace PlaylistMates.Webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaylistController : ControllerBase
    {
        private readonly ILogger<PlaylistController> _logger;
        private readonly Context _context;


        public PlaylistController(ILogger<PlaylistController> logger, Context context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("{playlistId}")]
        [Authorize(Policy = "PlaylistAnyRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<Playlist>> GetPlaylist(int playlistId)
        {
            // Navigationen in der Entity Klasse Pupil können über 
            // [System.Text.Json.Serialization.JsonIgnore] ausgeschlossen werden.
            Playlist playlist = await _context.Playlists.FindAsync(playlistId);
            // get the name of the user currently logged in

            // TODO: This currently basically outputs our whole schema because navigation properties have not been set to jsonignore
            if (playlist == null) { return NotFound(); }
            if (User.FindFirstValue(ClaimTypes.Name) != null)
            {
                _logger.LogInformation("{User} got playlist with id {Id} at {DT}",User.FindFirstValue(ClaimTypes.Name),playlistId,DateTime.UtcNow.ToLongTimeString());
            }
            _logger.LogInformation("Non-user got playlist with id {Id} at {DT}",playlistId,DateTime.UtcNow.ToLongTimeString());
            return playlist;
        }
        
        [HttpGet("/api/Playlist/byUser/{email}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<List<Playlist>>> GetPlaylistsByUser(String email)
        {
            _logger.LogDebug(email);
            var loggedInUserEmail = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            
            if(loggedInUserEmail == null)
            {
                return Forbid(); // if there's no email claim in the token
            }

            if (!string.Equals(loggedInUserEmail, email, StringComparison.OrdinalIgnoreCase))
            {
                return Forbid(); // if the logged in user email does not match the requested user email
            }

            List<Playlist> playlists = _context.Accounts
                .Include(a => a.AccountPlaylists)
                .ThenInclude(ap => ap.Playlist)
                .Where(a => a.Email == email)
                .SelectMany(a => a.AccountPlaylists
                    .Select(ap => ap.Playlist)).ToList();
            Console.WriteLine(playlists.ToString());
            if (playlists.IsNullOrEmpty()) { return Forbid(); }
            
            return playlists;
        }
        //[HttpPost]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<ActionResult<Playlist>> PostPlaylist()
        //{

        //}
    }
}
