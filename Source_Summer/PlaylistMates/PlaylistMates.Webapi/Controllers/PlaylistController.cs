using System.Security.Claims;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlaylistMates.Webapi.Services;
using PlaylistMates.Application.Infrastructure;
using PlaylistMates.Application.Model;
using PlaylistMates.Application.Dto;
using AutoMapper;

namespace PlaylistMates.Webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaylistController : ControllerBase
    {
        private readonly ILogger<PlaylistController> _logger;
        private readonly Context _context;
        private readonly IMapper _mapper;



        public PlaylistController(ILogger<PlaylistController> logger, Context context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
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

        [HttpPut("{id}")]
        [Authorize(Policy = "PlaylistOwnerPolicy")]
        public IActionResult UpdatePlaylist(int id, [FromBody] PlaylistUpdateDto playlistDto)
        {
            
                var existingPlaylist = _context.Playlists.Find(id);

                if (existingPlaylist == null)
                {
                    return NotFound();
                }

                existingPlaylist.Description = playlistDto.Description;
                existingPlaylist.IsPublic = playlistDto.IsPublic;

                _context.SaveChanges();
            

            return NoContent();
        }


        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Playlist>> CreatePlaylist([FromBody] PlaylistCreateDto playlistDto)
        {
            if (playlistDto == null)
            {
                return BadRequest();
            }

            // Getting the email from the authenticated user
            var email = User.FindFirst(ClaimTypes.Name)?.Value;

            // Finding the account associated with the email
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email);

            if (account == null)
            {
                // The account is not found, handle this case
                return NotFound("Account not found");
            }

            var playlist = _mapper.Map<Playlist>(playlistDto);

            await _context.Playlists.AddAsync(playlist);
            await _context.SaveChangesAsync();

            // Creating a new AccountPlaylist record
            var accountPlaylist = new AccountPlaylist(playlist, account, PlaylistRole.OWNER);

            await _context.AccountPlaylists.AddAsync(accountPlaylist);
            await _context.SaveChangesAsync();

            var createdPlaylistDto = _mapper.Map<SongDto>(playlist);

            return CreatedAtAction(nameof(GetPlaylist), new { id = playlist.Id }, createdPlaylistDto);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy ="PlaylistOwnerPolicy")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeletePlaylist(int id)
        {
            
                var playlist = _context.Playlists.Find(id);

                if (playlist == null)
                {
                    return NotFound();
                }

                _context.Playlists.Remove(playlist);
                _context.SaveChanges();
            

            return NoContent();
        }

        [HttpPost("/api/Playlist/{playlistId}/songs/{songId}")]
        [Authorize(Policy ="PlaylistCollaboratorOrOwner")]
        public async Task<IActionResult> AddSongToPlaylist(int playlistId, int songId)
        {
            var playlist = await _context.Playlists.FindAsync(playlistId);
            var song = await _context.Songs.FindAsync(songId);

            if (playlist == null || song == null)
            {
                return NotFound();
            }

            playlist.AddSong(song);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("/api/Playlist/{playlistId}/songs/{songId}")]
        [Authorize(Policy = "PlaylistCollaboratorOrOwner")]
        public async Task<IActionResult> RemoveSongFromPlaylist(int playlistId, int songId)
        {
            var playlist = await _context.Playlists.FindAsync(playlistId);
            var song = await _context.Songs.FindAsync(songId);

            if (playlist == null || song == null)
            {
                return NotFound();
            }

            playlist.RemoveSong(song);
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
