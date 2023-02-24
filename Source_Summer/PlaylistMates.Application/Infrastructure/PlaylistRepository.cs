using Microsoft.EntityFrameworkCore;
using PlaylistMates.Application.Infrastructure;
using PlaylistMates.Application.Model;

namespace PlaylistMates.Application.Infrastructure;


public class PlaylistRepository : Repository<Playlist, int>
{
    protected readonly DbSet<Playlist> _accounts;
    
    public PlaylistRepository(DbContext context) : base(context)
    {
        _accounts = context.Set<Playlist>();
    }

    public void AddSong(Song song)
    {
        // add Song to Playlist
    }
}