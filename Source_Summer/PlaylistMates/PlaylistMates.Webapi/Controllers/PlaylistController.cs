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
            Playlist playlist = await _context.Playlists
        .Include(p => p.Songs)
        .FirstOrDefaultAsync(p => p.Id == playlistId);
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
        public async Task<ActionResult<List<PlaylistDto>>> GetPlaylistsByUser(string email)
        {
            var loggedInUserEmail = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;

            if (loggedInUserEmail == null)
            {
                return Forbid(); // if there's no email claim in the token
            }

            if (!string.Equals(loggedInUserEmail, email, StringComparison.OrdinalIgnoreCase))
            {
                return Forbid(); // if the logged-in user email does not match the requested user email
            }

            var account = await _context.Accounts
                .Include(a => a.AccountPlaylists)
                    .ThenInclude(ap => ap.Playlist)
                .ThenInclude(p => p.Songs)
                .SingleOrDefaultAsync(a => a.Email == email);

            if (account == null)
            {
                return NotFound(); // if the account is not found
            }

            var playlists = account.AccountPlaylists.Select(ap => new PlaylistDto
            {
                Title = ap.Playlist.Title,
                Id = ap.Playlist.Id,
                Description = ap.Playlist.Description,
                IsPublic = ap.Playlist.IsPublic,
                Role = ap.Role.ToString(),
                Songs = ap.Playlist.Songs.Select(s => new SongDto(
                    s.Guid,
                    s.IsrcCode,
                    s.Titel,
                    s.ReleaseDate,
                    s.DurationInMillis,
                    s.Artists
                )).ToList()
            }).ToList();

            return playlists;
        }


        [HttpPut("{playlistId}")]
        [Authorize(Policy = "PlaylistCollaboratorOrOwner")]
        public IActionResult UpdatePlaylist(int playlistId, [FromBody] PlaylistUpdateDto playlistDto)
        {
            
                var existingPlaylist = _context.Playlists.Find(playlistId);

                if (existingPlaylist == null)
                {
                    return NotFound();
                }

                existingPlaylist.Title = playlistDto.Title;
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

            playlistDto.isPublic = false;
            // Getting the email from the authenticated user
            var email = User.FindFirst(ClaimTypes.Name)?.Value;

            // Finding the account associated with the email
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email);

            if (account == null)
            {
                // The account is not found, handle this case
                return NotFound("Account not found");
            }

            var playlist = new Playlist(playlistDto.Title,playlistDto.Description, playlistDto.isPublic);
            //var playlist = _mapper.Map<Playlist>(playlistDto);

            await _context.Playlists.AddAsync(playlist);
            await _context.SaveChangesAsync();

            // Creating a new AccountPlaylist record
            var accountPlaylist = new AccountPlaylist(playlist, account, PlaylistRole.OWNER);

            await _context.AccountPlaylists.AddAsync(accountPlaylist);
            await _context.SaveChangesAsync();

            var createdPlaylistDto = _mapper.Map<PlaylistDto>(playlist);

            return CreatedAtAction(nameof(GetPlaylist), new { id = playlist.Id }, createdPlaylistDto);
        }

        [HttpDelete("{playlistId}")]
        [Authorize(Policy ="PlaylistOwnerPolicy")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeletePlaylist(int playlistId)
        {
            
                var playlist = _context.Playlists.Find(playlistId);

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
            var playlist = await _context.Playlists
                .Include(p => p.Songs)
                .FirstOrDefaultAsync(p => p.Id == playlistId);

            if (playlist == null)
            {
                return NotFound();
            }

            var song = playlist.Songs.FirstOrDefault(s => s.Id == songId);

            if (song == null)
            {
                return NotFound();
            }

            playlist.RemoveSong(song);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("/api/Playlist/{playlistId}/role")]
        [Authorize]
        public ActionResult<string> GetUserRoleForPlaylist(int playlistId)
        {
            var loggedInUserEmail = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(loggedInUserEmail))
            {
                return Unauthorized();
            }

            var playlist = _context.Playlists.Find(playlistId);

            if (playlist == null)
            {
                return NotFound();
            }

            var account = _context.Accounts
                .Include(a => a.AccountPlaylists)
                .SingleOrDefault(a => a.Email == loggedInUserEmail);

            if (account == null)
            {
                return NotFound();
            }

            var accountPlaylist = account.AccountPlaylists
                .SingleOrDefault(ap => ap.PlaylistId == playlistId);

            if (accountPlaylist == null)
            {
                return NotFound();
            }

            var userRole = accountPlaylist.Role.ToString();
            return Ok(userRole);
        }


    }
}
