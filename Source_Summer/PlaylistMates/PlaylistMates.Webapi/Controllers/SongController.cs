using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlaylistMates.Webapi.Services;
using PlaylistMates.Application.Infrastructure;
using PlaylistMates.Application.Model;

namespace PlaylistMates.Webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SongController : ControllerBase
    {
        private readonly ILogger<SongController> _logger;
        private readonly Context _context;


        public SongController(ILogger<SongController> logger, Context context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("[songId]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<Song>> GetSong(int songid)
        {
            Song songs = await _context.Songs.FindAsync(songid);

            // TODO: This currently basically outputs our whole schema because navigation properties have not been set to jsonignore
            if (songs == null) { return NotFound(); }
            return songs;
        }   
    }
}