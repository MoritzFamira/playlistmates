namespace PlaylistMates.Test;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using PlaylistMates.Application.Model;

//Unit test for repository not included as discussed in lesson


[Collection("Sequential")] // A file database does not support parallel test execution.
public class ContextTests : DatabaseTest
{
    [Fact]
    public void CreateDatabaseTest()
    {
        _db.Database.EnsureDeleted();
        _db.Database.EnsureCreated();
    }

    [Fact]
    public void SeedTest()
    {
        _db.Database.EnsureDeleted();
        _db.Database.EnsureCreated();

        Randomizer.Seed = new Random(1335);

        var accounts = new Faker<Account>()
            .CustomInstantiator(a => new Account(
                email: a.Internet.Email(),
                accountName: a.Person.UserName
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

        var songCollections = new Faker<SongCollection>()
            .CustomInstantiator(s => new SongCollection(
                title: s.Lorem.Word(),
                creationDate: s.Date.Recent(1000)
                ))
            .Generate(30)
            .ToList();
        _db.SongCollections.AddRange(songCollections);
        _db.SaveChanges();

        var playlists = new Faker<Playlist>()
            .CustomInstantiator(p => new Playlist(
                description: p.Lorem.Sentence(),
                isPublic: p.Random.Bool()
                ))
            .RuleFor(p => p.Title, f => f.Lorem.Word())
            .RuleFor(p => p.CreationDate, f => f.Date.Recent(1000))
            .Generate(20)
            .ToList();

        _db.Playlists.AddRange(playlists);
        _db.SaveChanges();




        // error here
        // the data is correctly saved but when retreiving the data an exception is thrown
        var accountPlaylist = new Faker<AccountPlaylist>()
            .CustomInstantiator(a => new AccountPlaylist(
               account: a.PickRandom(accounts),
               playlist: a.PickRandom(playlists),
               role: a.PickRandom<PlaylistRole>()
           ))
            .Generate(15)
            .GroupBy(a => new { a.AccountId, a.PlaylistId }).Select(g => g.First())
            .ToList();

        _db.AccountPlaylists.AddRange(accountPlaylist);
        _db.SaveChanges();

        var artists = new Faker<Artist>()
            .CustomInstantiator(a => new Artist(
                Name: a.Person.UserName
                ))
            .Generate(20)
            .ToList();

        var songs = new Faker<Song>()
            .CustomInstantiator(s => new Song(
                isrcCode: s.Lorem.Word(),  // The isrc code is a 12-digit string.
                title: s.Lorem.Word(),
                releaseDate: s.Date.Recent(1000),
                durationInMillis: s.Random.Int(1000,100000),
                artists: (List<Artist>) s.Random.ListItems(artists),
                platforms: (List<Platform>) s.Random.ListItems(platforms)
                ))
            .Generate(15)
            .GroupBy(s => s.IsrcCode).Select(g => g.First())
            .ToList();

        _db.Songs.AddRange(songs);
        _db.SaveChanges();

        var albums = new Faker<Album>()
            .CustomInstantiator(a => new Album(
                artists: (List<Artist>)a.Random.ListItems(artists)))
            .RuleFor(p => p.Title, f => f.Lorem.Word())
            .RuleFor(p => p.CreationDate, f => f.Date.Recent(1000))
            .Generate(15)
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

        _db.ChangeTracker.Clear();

        Assert.True(_db.Accounts.Any());
        Assert.True(_db.Playlists.Any());
        Assert.True(_db.Platforms.Count() == 3);
        Assert.True(_db.AccountPlatforms.Any());
        Assert.True(_db.SongCollections.Any());
        Assert.True(_db.AccountPlaylists.Any());
        Assert.True(_db.Albums.Any());
        Assert.True(_db.Songs.Any());
    }
    [Fact]
    public void AddTest()
    {
        _db.Database.EnsureDeleted();
        _db.Database.EnsureCreated();

        var songCollections = new Faker<SongCollection>()
    .CustomInstantiator(s => new SongCollection(
        title: s.Lorem.Word(),
        creationDate: s.Date.Recent(1000)
        ))
    .Generate(1)
    .ToList();
        _db.SongCollections.AddRange(songCollections);
        _db.SaveChanges();


        var platforms = new List<Platform>();
        platforms.Add(new Platform("Spotify"));
        platforms.Add(new Platform("Apple_Music"));
        platforms.Add(new Platform("Amazon_Music"));
        _db.Platforms.AddRange(platforms);
        _db.SaveChanges();


        var artist = new Artist("testArtist");
        var artistList = new List<Artist>();
        artistList.Add(artist);
        var song = new Song("testISRCCode", "Title", new DateTime(), 10000, artistList, platforms);
        _db.Songs.Add(song);
        _db.SaveChanges();

        Assert.True(_db.SongCollections.Single(sc => sc.Id == 1).Songs.Count == 0);

        _db.SongCollections.Single(sc => sc.Id == 1).AddSong(song);
        _db.SaveChanges();
        Assert.Contains(song, _db.SongCollections.Single(sc => sc.Id == 1).Songs);
        Assert.True(_db.SongCollections.Single(sc => sc.Id == 1).Songs.Count == 1);
    }

    [Fact]
    public void testValueConverter()
    {
        _db.Database.EnsureDeleted();
        _db.Database.EnsureCreated();

        Randomizer.Seed = new Random(1335);

        var accounts = new Faker<Account>()
            .CustomInstantiator(a => new Account(
                email: a.Internet.Email(),
                accountName: a.Person.UserName
                ))
            .Generate(1)
            .ToList();

        _db.Accounts.AddRange(accounts);
        _db.SaveChanges();


        var playlists = new Faker<Playlist>()
            .CustomInstantiator(p => new Playlist(
                description: p.Lorem.Sentence(),
                isPublic: p.Random.Bool()
                ))
            .RuleFor(p => p.Title, f => f.Lorem.Word())
            .RuleFor(p => p.CreationDate, f => f.Date.Recent(1000))
            .Generate(3)
            .ToList();

        _db.Playlists.AddRange(playlists);
        _db.SaveChanges();


        var accountPlaylistOwner = new Faker<AccountPlaylist>()
            .CustomInstantiator(a => new AccountPlaylist(
               account: a.PickRandom(accounts),
               playlist: playlists[0],
               role: PlaylistRole.OWNER
           ))
            .Generate(1)
            .GroupBy(a => new { a.AccountId, a.PlaylistId }).Select(g => g.First())
            .ToList();

        var accountPlaylistCollaborator = new Faker<AccountPlaylist>()
            .CustomInstantiator(a => new AccountPlaylist(
               account: a.PickRandom(accounts),
               playlist: playlists[1],
               role: PlaylistRole.COLLABORATOR
           ))
            .Generate(1)
            .GroupBy(a => new { a.AccountId, a.PlaylistId }).Select(g => g.First())
            .ToList();

        var accountPlaylistListener = new Faker<AccountPlaylist>()
            .CustomInstantiator(a => new AccountPlaylist(
               account: a.PickRandom(accounts),
               playlist: playlists[2],
               role: PlaylistRole.LISTENER
           ))
            .Generate(1)
            .GroupBy(a => new { a.AccountId, a.PlaylistId }).Select(g => g.First())
            .ToList();

        _db.AccountPlaylists.AddRange(accountPlaylistOwner);
        _db.AccountPlaylists.AddRange(accountPlaylistCollaborator);
        _db.AccountPlaylists.AddRange(accountPlaylistListener);

        _db.SaveChanges();


        Assert.True(_db.AccountPlaylists.Any());

        Assert.True(_db.AccountPlaylists.Single(ap => ap.AccountId == 1 && ap.PlaylistId == 1).Role == PlaylistRole.OWNER);
        Assert.True(_db.AccountPlaylists.Single(ap => ap.AccountId == 1 && ap.PlaylistId == 2).Role == PlaylistRole.COLLABORATOR);
        Assert.True(_db.AccountPlaylists.Single(ap => ap.AccountId == 1 && ap.PlaylistId == 3).Role == PlaylistRole.LISTENER);

    }

}
