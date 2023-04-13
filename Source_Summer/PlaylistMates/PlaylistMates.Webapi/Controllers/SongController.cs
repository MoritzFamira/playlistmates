﻿using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMapper _mapper;

        public SongController(ILogger<SongController> logger, Context context, IMapper mapper)
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

            if (User.FindFirstValue(ClaimTypes.Name) != null)
            {
                _logger.LogInformation("{User} got song with GUID {Guid} at {DT}",User.FindFirstValue(ClaimTypes.Name),guid,DateTime.UtcNow.ToLongTimeString());
            }
            _logger.LogInformation("Non-user got song with GUID {Guid} at {DT}",guid,DateTime.UtcNow.ToLongTimeString());
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
                            .AsNoTracking()
                            .Select(s => new
                            {
                                s.Guid,
                                s.Titel,
                                s.IsrcCode,
                                s.Artists,
                                s.DurationInMillis,

                            });
            if (song is null) { return NotFound(); }
            if (User.FindFirstValue(ClaimTypes.Name) != null)
            {
                _logger.LogInformation("{User} got songs at {DT}",User.FindFirstValue(ClaimTypes.Name),DateTime.UtcNow.ToLongTimeString());
            }
            _logger.LogInformation("Non-user got songs at {DT}",DateTime.UtcNow.ToLongTimeString());
            return Ok(song);
        }
    }
}