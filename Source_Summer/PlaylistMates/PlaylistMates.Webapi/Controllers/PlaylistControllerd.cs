using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlaylistMates.Application.Documents;
using PlaylistMates.Application.Dto;
using PlaylistMates.Application.Infrastructure;

namespace PlaylistMates.Webapi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class PlaylistControllerd :ControllerBase
{
    private readonly MongoDatabase db = new MongoDatabase();

    public PlaylistControllerd()
    {
        
    }
    [HttpGet("all")]
    public ActionResult<List<Playlistd>> GetAllPlaylists()
    {
        return Ok(db.PlaylistRepository.Queryable.ToList());
    }
    
    [HttpPost("page/{page}")]
    public ActionResult<List<PlaylistDtod>> GetPlaylistsByPage(int page)
    {
        return Ok(db.PlaylistRepository.Queryable.Skip((page-1)*10).Take(10).ToList());
    }
    [HttpPost("pageAlphabetical/{page}")]
    public ActionResult<List<PlaylistDtod>> GetPlaylistsByPageAlphabetically(int page)
    {
        return Ok(db.PlaylistRepository.Queryable
            .OrderBy(p => p.Title)
            .Select(p => new PlaylistDtod { Title = p.Title, guid = p.Id, Songs = p.Songs
                        .Select(s => new SongDtod { Titel = s.Titel, guid = s.Id }).ToList()})
            .Skip((page-1)*10)
            .Take(10)
            .ToList());
    }
    
    [HttpPost("delete/{guid}")]
    public ActionResult DeletePlaylistById(Guid guid)
    {
        db.PlaylistRepository.DeleteOne(guid);
        return Ok();
    }
    [HttpPost("update")]
    public ActionResult UpdateSongTitleOfPlaylist(Guid SongId, Guid PlaylistId, string newTitle)
    {
        var playlist = db.PlaylistRepository.FindById(PlaylistId);
        var song = playlist.Songs.Find(s => s.Id == SongId);
        song.Titel = newTitle;
        db.PlaylistRepository.UpdateOne(playlist);
        return Ok(playlist);
    }
}