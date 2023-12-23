using MongoDB.Driver;
using PlaylistMates.Application.Documents;

namespace PlaylistMates.Application.Infrastructure;

public class PlaylistRepositoryd : Repositoryd<Playlistd,Guid>
{
    public void AddSongToPlaylist(Playlistd playlist, Songd song)
    {
        playlist.Songs.Add(song);
        UpdateOne(playlist);
    }
    public Playlistd? FindOne(Guid id)
    {
        return FindById(id);
    }

    public PlaylistRepositoryd(IMongoCollection<Playlistd> coll) : base(coll)
    {
    }
}