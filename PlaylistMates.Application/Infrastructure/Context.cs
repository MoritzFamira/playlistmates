using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlaylistMates.Application.Model;

namespace PlaylistMates.Application.Infrastructure
{
    public class Context : DbContext
    {
        public Context(DbContextOptions opt) : base(opt) { }

        public DbSet<SongCollection> SongCollections => Set<SongCollection>();
        public DbSet<Album> Albums => Set<Album>();
        public DbSet<Playlist> Playlists => Set<Playlist>();
        public DbSet<Song> Songs => Set<Song>();
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Platform> Platforms => Set<Platform>();
        public DbSet<LogItem> LogItems => Set<LogItem>();

    }
}
