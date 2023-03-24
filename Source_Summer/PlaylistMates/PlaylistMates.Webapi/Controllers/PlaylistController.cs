﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

            // TODO: This currently basically outputs our whole schema because navigation properties have not been set to jsonignore
            if (playlist == null) { return NotFound(); }
            return playlist;
        }
    }
}
