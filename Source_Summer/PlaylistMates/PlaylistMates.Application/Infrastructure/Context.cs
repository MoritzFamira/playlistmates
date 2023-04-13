using Microsoft.EntityFrameworkCore;

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
        public DbSet<AccountPlaylist> AccountPlaylists => Set<AccountPlaylist>();
        public DbSet<AccountPlatforms> AccountPlatforms => Set<AccountPlatforms>();

        public Repository<SongCollection, int> SongCollectionRepository => new Repository<SongCollection, int>(this);
        public Repository<Album, int> AlbumRepository => new Repository<Album, int>(this);
        public PlaylistRepository PlaylistRepository => new PlaylistRepository(this);
        public Repository<Song, int> SongRepository => new Repository<Song, int>(this);
        public AccountRepository AccountRepository => new AccountRepository(this);
        //public Repository<Platform, int> PlatformRepository => new Repository<Platform, int>(this);
        public Repository<LogItem, int> LogItemRepository => new Repository<LogItem, int>(this);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasIndex(a => a.Email).IsUnique();

            modelBuilder.Entity<Song>().OwnsMany(s => s.Artists);
            modelBuilder.Entity<Album>().OwnsMany(a => a.Artists);
            // converts the enum int values to string values
            modelBuilder.Entity<LogItem>().Property(l => l.Action).HasConversion<string>();
            // modelBuilder.Entity<AccountPlaylist>().Property(a => a.Role).HasConversion<string>(); -- removed due to custom value converter
            modelBuilder.Entity<AccountPlaylist>().Property(a => a.Role).HasDefaultValue(PlaylistRole.LISTENER);

            // unique index for the AccountPlaylist entity/relation
            modelBuilder.Entity<AccountPlaylist>().HasIndex(a => new { a.PlaylistId, a.AccountId }).IsUnique();
            modelBuilder.Entity<AccountPlatforms>().HasIndex(a => new { a.PlatformId, a.AccountId }).IsUnique();

            modelBuilder.Entity<Song>().HasIndex(s => s.IsrcCode).IsUnique();

            modelBuilder.Entity<LogItem>().Property(l => l.TimeStamp).HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder
            .Entity<AccountPlaylist>()
            .Property(a => a.Role)
            .HasConversion<string>();
        }
        
        }
    }

