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
        _db.SaveChanges();

        

        var platforms = new List<Platform>();
        platforms.Add(new Platform("Spotify"));
        platforms.Add(new Platform("Apple Music"));
        platforms.Add(new Platform("Amazon Music"));
        _db.Platforms.AddRange(platforms);
        _db.SaveChanges();

        var accountPlatforms = new Faker<AccountPlatforms>()
            .CustomInstantiator(a => new AccountPlatforms(
                account: a.PickRandom(accounts).Id,
                platform: a.PickRandom(platforms).Id, 
                // the above fields are primary keys and it is very bad practice to generate those randomly without ensuring uniqueneess, this is only a tempoary solution
                
                authtoken: a.Internet.Password() // password is used instead of an authtoken for simplicity
                ))
            .Generate(15)
            .GroupBy(a => new { a.AccountId, a.PlatformId }).Select(g => g.First())
            .ToList();
        _db.AccountPlatforms.AddRange(accountPlatforms);
        _db.SaveChanges();

        var playlists = new Faker<Playlist>()
            .CustomInstantiator(a => new Playlist(
                description: a.Lorem.Sentence(),
                isPublic: a.Random.Bool()
                ));

        _db.Playlists.AddRange(playlists);
        _db.SaveChanges();

        var accountPlaylist = new Faker<AccountPlaylist>()
            .CustomInstantiator(a => new AccountPlaylist(
               account: a.PickRandom(accounts).Id,
               playlist: a.Random.Int(),
               role: a.PickRandom<PlaylistRole>()
           ));

        _db.AccountPlaylists.AddRange(accountPlaylist);
        _db.SaveChanges();
        _db.ChangeTracker.Clear();

        Assert.True(_db.Accounts.Any());
        Assert.True(_db.Playlists.Any());
        Assert.True(_db.Platforms.Count() == 3);
        Assert.True(_db.AccountPlatforms.Any());
        Assert.True(_db.AccountPlaylists.Any());

    }
}