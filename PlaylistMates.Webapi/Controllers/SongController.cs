using Microsoft.AspNetCore.Mvc;
using PlaylistMates.Application.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace PlaylistMates.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongController : Controller
    {
        private readonly Context _context;
        public SongController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSong()
        {
            var result = await (from s in _context.Songs
                                select new
                                {
                                    Id = s.Id,
                                    Isrc = s.IsrcCode,
                                    Title = s.Titel,
                                    ReleaseDate = s.ReleaseDate,
                                    DurationInMillis = s.DurationInMillis,
                                    Artists = s.Artists,
                                    Platforms = s.Platforms
                                }).ToListAsync();
            return Ok(result);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
