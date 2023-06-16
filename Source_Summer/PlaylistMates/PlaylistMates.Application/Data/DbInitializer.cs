using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlaylistMates.Application.Infrastructure;
using PlaylistMates.Application.Model;
using Bogus;
using System.Diagnostics;
using System.Data;


namespace PlaylistMates.Application.Data
{
    public class DbInitializer
    {
        protected readonly Context _db;

        public DbInitializer(DbContextOptions options)
        {
            _db = new Context(options);
        }
        public async Task<bool> EnsureDbConnectionAsync(int timeoutInSeconds)
        {
            var connection = _db.Database.GetDbConnection();
            var stopwatch = Stopwatch.StartNew();

            while (true)
            {
                try
                {
                    await connection.OpenAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    if (stopwatch.Elapsed.TotalSeconds >= timeoutInSeconds)
                    {
                        throw;
                    }

                    await Task.Delay(5000); // wait for 5 seconds before trying again
                }
                finally
                {
                    // close the connection in finally block, ensuring it gets closed no matter what
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }

        }

        public void Init()
        {
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();

            Randomizer.Seed = new Random(1335);
            string salt = GenerateRandom();
            var accounts = new Faker<Account>()
                .CustomInstantiator(a => new Account(
                    email: a.Internet.Email(),
                    accountName: a.Person.UserName,
                    salt: salt,
                    hashedPassword: CalculateHash("1234", salt)
                    ))
                .Generate(25)
                .ToList();

            _db.Accounts.AddRange(accounts);
            _db.SaveChanges();



            var platforms = new List<Platform>();
            platforms.Add(new Platform("Spotify"));
            platforms.Add(new Platform("Apple_Music"));
            platforms.Add(new Platform("Amazon_Music"));
            _db.Platforms.AddRange(platforms);
            _db.SaveChanges();

            var accountPlatforms = new Faker<AccountPlatforms>()
                .CustomInstantiator(a => new AccountPlatforms(
                    account: a.PickRandom(accounts),
                    platform: a.PickRandom(platforms),
                    authtoken: a.Internet.Password() // password is used instead of an authtoken for simplicity
                    ))
                .Generate(15)
                .GroupBy(a => new { a.AccountId, a.PlatformId }).Select(g => g.First())
                .ToList();
            _db.AccountPlatforms.AddRange(accountPlatforms);
            _db.SaveChanges();

            var artists = new Faker<Artist>()
                .CustomInstantiator(a => new Artist(
                    Name: a.Person.UserName
                    ))
                .Generate(20)
                .ToList();

            var songs = new Faker<Song>()
                .CustomInstantiator(s =>
                {
                    return new Song(
                        isrcCode: s.Lorem.Word(),  // The isrc code is a 12-digit string.
                        title: s.Lorem.Word(),
                        releaseDate: s.Date.Recent(1000),
                        durationInMillis: s.Random.Int(1000, 100000),
                        artists: (List<Artist>)s.Random.ListItems(artists),
                        platforms: (List<Platform>)s.Random.ListItems(platforms))
                    { Guid = s.Random.Guid() };

                })
                .Generate(15)
                .GroupBy(s => s.IsrcCode).Select(g => g.First())
                .ToList();

            _db.Songs.AddRange(songs);
            _db.SaveChanges();

            //        var songCollections = new Faker<SongCollection>()
            //.CustomInstantiator(s =>
            //{
            //    var songCollection = new SongCollection(
            //        title: s.Lorem.Word(),
            //        creationDate: s.Date.Recent(1000))
            //    { Guid = s.Random.Guid() };

            //    var songsToAdd = s.Random.ListItems(songs);
            //    foreach (var song in songsToAdd)
            //    {
            //        songCollection.AddSong(song);
            //    }

            //    return songCollection;
            //})
            //.Generate(35)
            //.ToList();
            //        _db.SongCollections.AddRange(songCollections);
            //        _db.SaveChanges();


            var playlists = new Faker<Playlist>()
                .CustomInstantiator(p =>
                {
                    var playlist = new Playlist(
                        description: p.Lorem.Sentence(),
                        isPublic: p.Random.Bool())
                    {
                        Title = p.Lorem.Word(),
                        CreationDate = p.Date.Recent(1000),
                        Guid = p.Random.Guid()

                    };
                    var songsToAdd = p.Random.ListItems(songs);
                    foreach (var song in songsToAdd)
                    {
                        playlist.AddSong(song);
                    }
                    return playlist;
                })
            .Generate(30)
            .ToList();

            _db.Playlists.AddRange(playlists);
            _db.SaveChanges();


            var accountPlaylist = new Faker<AccountPlaylist>()
                .CustomInstantiator(a => new AccountPlaylist(
                   account: a.PickRandom(accounts),
                   playlist: a.PickRandom(playlists),
                   role: a.PickRandom<PlaylistRole>()
               ))
                .Generate(25)
                .GroupBy(a => new { a.AccountId, a.PlaylistId }).Select(g => g.First())
                .ToList();

            _db.AccountPlaylists.AddRange(accountPlaylist);
            _db.SaveChanges();


            var albums = new Faker<Album>()
                .CustomInstantiator(a =>
                {
                    var album = new Album(
                        artists: (List<Artist>)a.Random.ListItems(artists))
                    {
                        Title = a.Lorem.Word(),
                        CreationDate = a.Date.Recent(1000),
                        Guid = a.Random.Guid()
                    };
                    var songsToAdd = a.Random.ListItems(songs);
                    foreach (var song in songsToAdd)
                    {
                        album.AddSong(song);
                    }
                    return album;
                })
                .Generate(20)
                .ToList();

            _db.Albums.AddRange(albums);
            _db.SaveChanges();


            var logItems = new Faker<LogItem>()
                .CustomInstantiator(l => new LogItem(
                    account: l.PickRandom(accounts),
                    action: l.PickRandom<Application.Model.Action>()))
                .Generate(30)
                .ToList();
            _db.LogItems.AddRange(logItems);


            _db.SaveChanges();
        }

        public static string GenerateRandom(int length = 128)
        {
            // Salt erzeugen.
            byte[] salt = new byte[length / 8];
            using (System.Security.Cryptography.RandomNumberGenerator rnd =
                System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rnd.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }
        protected static string CalculateHash(string password, string salt)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(salt))
            {
                throw new ArgumentException("Invalid Salt or Passwort.");
            }
            byte[] saltBytes = Convert.FromBase64String(salt);
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);

            System.Security.Cryptography.HMACSHA256 myHash =
                new System.Security.Cryptography.HMACSHA256(saltBytes);

            byte[] hashedData = myHash.ComputeHash(passwordBytes);

            // Das Bytearray wird als Hexstring zurückgegeben.
            string hashedPassword = Convert.ToBase64String(hashedData);
            return hashedPassword;
        }
    }
}
