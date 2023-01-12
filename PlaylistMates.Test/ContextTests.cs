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



        var platforms = new List<Platform>();
        platforms.Add(new Platform("Spotify"));
        platforms.Add(new Platform("Apple_Music"));
        platforms.Add(new Platform("Amazon_Music"));
        _db.Platforms.AddRange(platforms);

        var accountPlatforms = new Faker<AccountPlatforms>()
            .CustomInstantiator(a => new AccountPlatforms(
                account: a.PickRandom(accounts).Id,
                platform: a.PickRandom(platforms).Id,
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

        


        // error here
        var accountPlaylist = new Faker<AccountPlaylist>()
            .CustomInstantiator(a => new AccountPlaylist(
               account: a.PickRandom(accounts).Id,
               playlist: a.PickRandom(playlists).Id,
               role: a.PickRandom<PlaylistRole>()
           ))
            .Generate(15)
            .GroupBy(a => new { a.AccountId, a.PlaylistId }).Select(g => g.First())
            .ToList();

        _db.AccountPlaylists.AddRange(accountPlaylist);

        var artists = new Faker<Artist>()
            .CustomInstantiator(a => new Artist(
                Name: a.Person.UserName
                ))
            .Generate(20)
            .ToList();

        var songs = new Faker<Song>()
            .CustomInstantiator(s => new Song(
                isrcCode: s.Lorem.Word(),
                title: s.Lorem.Word(),
                releaseDate: s.Date.Recent(1000),
                durationInMillis: s.Random.Int(1000,100000),
                artists: (List<Artist>) s.Random.ListItems(artists),
                platforms: (List<Platform>) s.Random.ListItems(platforms)
                ))
            .Generate(15)
            .ToList();

        _db.Songs.AddRange(songs);

        var albums = new Faker<Album>()
            .CustomInstantiator(a => new Album(
                artists: (List<Artist>)a.Random.ListItems(artists)))
            .Generate(15)
            .ToList();
        _db.Albums.AddRange(albums);

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
}