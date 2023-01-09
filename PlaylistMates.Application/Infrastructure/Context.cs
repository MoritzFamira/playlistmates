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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasIndex(a => a.Email).IsUnique();

            modelBuilder.Entity<Song>().OwnsMany(s => s.Artists);
            modelBuilder.Entity<Album>().OwnsMany(a => a.Artists);
            // converts the enum int values to string values
            modelBuilder.Entity<LogItem>().Property(l => l.Action).HasConversion<string>();
            modelBuilder.Entity<AccountPlaylist>().Property(a => a.Role).HasConversion<string>();
            modelBuilder.Entity<AccountPlaylist>().Property(a => a.Role).HasDefaultValue(PlaylistRole.LISTENER);

            // composite key for the AccountPlaylist entity/relation
            modelBuilder.Entity<AccountPlaylist>().HasKey(a => new { a.PlaylistId, a.AccountId });

            modelBuilder.Entity<LogItem>().Property(l => l.TimeStamp).HasDefaultValueSql("CURRENT_TIMESTAMP");

        }

    }
}
