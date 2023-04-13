using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlaylistMates.Webapi.Services;
using PlaylistMates.Application.Infrastructure;
using PlaylistMates.Application.Model;
using PlaylistMates.Application.Dto;

namespace PlaylistMates.Webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SongController : ControllerBase
    {
        private readonly ILogger<SongController> _logger;
        private readonly Context _context;
        private readonly Mapper _mapper;

        public SongController(ILogger<SongController> logger, Context context, Mapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("guid")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<SongDto>> GetSong(Guid guid)
        {

            var song = _context.Songs
                            .Where(s => s.Guid == guid)
                            .Select(s => new
                            {
                                s.Guid,
                                s.Titel,
                                s.IsrcCode,
                                s.Artists,
                                s.DurationInMillis,
                                SongCollectionGuids = s.SongCollections.Select(sc => sc.Guid),
                                SongCollectionTitle = s.SongCollections.Select(sc => sc.Title)

                            })
                            .FirstOrDefault(s => s.Guid == guid);
            if (song is null) { return NotFound(); }
            return Ok(song);

        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<List<Song>>> GetSongs()
        {
            var song = _context.Songs
                            .Select(s => new
                            {
                                s.Guid,
                                s.Titel,
                                s.IsrcCode,
                                s.Artists,
                                s.DurationInMillis,

                            });
            if (song is null) { return NotFound(); }
            return Ok(song);
        }

        [HttpPut("{guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateSong(Guid guid, SongDto songDto)
        {
            var song = await _context.Songs.FindAsync(guid);
            if (song == null)
            {
                return NotFound();
            }

            _mapper.Map(songDto, song);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteSong(Guid guid)
        {
            var song = await _context.Songs.FindAsync(guid);
            if (song == null)
            {
                return NotFound();
            }

            _context.Songs.Remove(song);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
    }
}