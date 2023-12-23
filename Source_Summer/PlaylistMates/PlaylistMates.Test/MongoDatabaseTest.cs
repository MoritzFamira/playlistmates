

using System.Diagnostics;
using Bogus;
using MongoDB.Bson;
using PlaylistMates.Application.Documents;
using PlaylistMates.Application.Dto;
using PlaylistMates.Application.Infrastructure;
using PlaylistMates.Application.Model;
using Xunit.Abstractions;

namespace PlaylistMates.Test
{
    public class ExamDatabaseTests
    {
        private readonly ITestOutputHelper _output;

        public ExamDatabaseTests(ITestOutputHelper output)
        {
            _output = output;
        }
        /*[Fact]
        public void SeedDatabaseTest()
        {
            var db = new MongoDatabase();
            db.PlaylistRepository.DeleteAll();
            
            Randomizer.Seed = new Random(1454);
            var rnd = new Randomizer();
            List <Playlistd> playlistDocuments= new Faker<Playlistd>("de")
                .CustomInstantiator(f =>
                {
                    var playlist = new Playlistd(f.Lorem.Sentence(), f.Lorem.Word(), f.Random.Bool());
                    playlist.Songs.AddRange(new Faker<Songd>("de").CustomInstantiator(f =>
                    {
                        var song = new Songd(f.Lorem.Sentence(), f.Lorem.Sentence(), f.Date.Past(),
                            f.Random.Int(1000, 100000), new List<string>(), new List<Platform>());
                        var artists = new Faker<string>("de").CustomInstantiator(f =>
                        {
                            var artistName = f.Name.FullName();
                            return artistName;
                        }).Generate(2);
                        song.Artists.AddRange(artists);
                        return song;
                    }).Generate(20));
                    return playlist;
                })
                .Generate(100000);
            // time who long this takes to run
            var watch = Stopwatch.StartNew();
            db.PlaylistRepository.InsertMany(playlistDocuments);
            watch.Stop();
            _output.WriteLine($"Inserting 100000 playlists took {watch.ElapsedMilliseconds} ms");
        }*/
        
        [Fact]
        public void SeedTest100k()
        {
            var db = new MongoDatabase();
            db.PlaylistRepository.DeleteAll();
            var playlistFaker = new Faker<Playlistd>("de")
                .RuleFor(p => p.Title, f => f.Lorem.Word())
                .RuleFor(p => p.Description, f => f.Lorem.Sentence())
                .RuleFor(p => p.IsPublic, f => f.Random.Bool())
                .RuleFor(p => p.Songs, f => GenerateSongs(f.Random.Int(1, 30)))
                .RuleFor(p => p.CreationDate, f => f.Date.Past())
                .RuleFor(p => p.AccountPlaylists, f => new List<AccountPlaylist>())
                .RuleFor(p => p.Guid, f => Guid.NewGuid());
            var playlists = playlistFaker.Generate(100000);
            
            var watch = Stopwatch.StartNew();
            db.PlaylistRepository.InsertMany(playlists);
            watch.Stop();
            _output.WriteLine($"Inserting 100000 playlists took {watch.ElapsedMilliseconds} ms");
        }
        [Fact]
        public void SeedTest10k()
        {
            var db = new MongoDatabase();
            db.PlaylistRepository.DeleteAll();
            var playlistFaker = new Faker<Playlistd>("de")
                .RuleFor(p => p.Title, f => f.Lorem.Word())
                .RuleFor(p => p.Description, f => f.Lorem.Sentence())
                .RuleFor(p => p.IsPublic, f => f.Random.Bool())
                .RuleFor(p => p.Songs, f => GenerateSongs(f.Random.Int(1, 30)))
                .RuleFor(p => p.CreationDate, f => f.Date.Past())
                .RuleFor(p => p.AccountPlaylists, f => new List<AccountPlaylist>())
                .RuleFor(p => p.Guid, f => Guid.NewGuid());
            var playlists = playlistFaker.Generate(10000);
            
            var watch = Stopwatch.StartNew();
            db.PlaylistRepository.InsertMany(playlists);
            watch.Stop();
            _output.WriteLine($"Inserting 10000 playlists took {watch.ElapsedMilliseconds} ms");
        }
        [Fact]
        public void SeedTest1k()
        {
            var db = new MongoDatabase();
            db.PlaylistRepository.DeleteAll();
            var playlistFaker = new Faker<Playlistd>("de")
                .RuleFor(p => p.Title, f => f.Lorem.Word())
                .RuleFor(p => p.Description, f => f.Lorem.Sentence())
                .RuleFor(p => p.IsPublic, f => f.Random.Bool())
                .RuleFor(p => p.Songs, f => GenerateSongs(f.Random.Int(1, 30)))
                .RuleFor(p => p.CreationDate, f => f.Date.Past())
                .RuleFor(p => p.AccountPlaylists, f => new List<AccountPlaylist>())
                .RuleFor(p => p.Guid, f => Guid.NewGuid());
            var playlists = playlistFaker.Generate(1000);
            
            var watch = Stopwatch.StartNew();
            db.PlaylistRepository.InsertMany(playlists);
            watch.Stop();
            _output.WriteLine($"Inserting 1000 playlists took {watch.ElapsedMilliseconds} ms");
        }
        private List<Songd> GenerateSongs(int count)
        {
            var songFaker = new Faker<Songd>("de")
                .RuleFor(s => s.IsrcCode, f => f.Lorem.Sentence())
                .RuleFor(s => s.Titel, f => f.Lorem.Word())
                .RuleFor(s => s.ReleaseDate, f => f.Date.Past())
                .RuleFor(s => s.DurationInMillis, f => f.Random.Int(1000, 100000))
                .RuleFor(s => s.Artists, f => GenerateArtists(f.Random.Int(1, 3)))
                .RuleFor(s => s.Id, f => Guid.NewGuid());
                //.RuleFor(s => s.Platforms, f => GeneratePlatforms(f.Random.Int(1, 3)));
            return songFaker.Generate(count);
        }
        private List<string> GenerateArtists(int count)
        {
            var artistFaker = new Faker<string>("de")
                .CustomInstantiator(f => f.Name.FullName());
            return artistFaker.Generate(count);
        }
        // ohne filter
        [Fact]
        public void GetAllPlaylistsTest()
        {
            var db = new MongoDatabase();
            var watch = Stopwatch.StartNew();
            db.PlaylistRepository.Queryable.ToList();
            watch.Stop();
            _output.WriteLine($"Getting all Playlists took {watch.ElapsedMilliseconds} ms");
        }
        
        [Fact]
        public void GetAllPlaylistsAlphabeticallyTest()
        {
            var db = new MongoDatabase();
            var watch = Stopwatch.StartNew();
            db.PlaylistRepository.Queryable.OrderBy(p => p.Title).ToList();
            watch.Stop();
            _output.WriteLine($"Getting all Playlists sorted alphabetically took {watch.ElapsedMilliseconds} ms");
        }
        // mit filter
        [Fact]
        public void GetAllPlaylistsThatArePublicTest()
        {
            var db = new MongoDatabase();
            var watch = Stopwatch.StartNew();
            db.PlaylistRepository.Queryable.Where(p => p.IsPublic == true).ToList();
            watch.Stop();
            _output.WriteLine($"Getting all public Playlists took {watch.ElapsedMilliseconds} ms");
        }
        // mit filter und sortierung und projection
        [Fact]
        public void GetAllPlaylistsThatArePublicSortedAlphabeticallyTest()
        {
            var db = new MongoDatabase();
            var watch = Stopwatch.StartNew();
            
            db.PlaylistRepository.Queryable
                .Where(p => p.IsPublic == true)
                .OrderBy(p => p.Title)
                .Select(p => new PlaylistDtod { Title = p.Title, guid = p.Id, Songs = p.Songs
                    .Select(s => new SongDtod { Titel = s.Titel, guid = s.Id }).ToList()}).ToList();
            
            watch.Stop();
            _output.WriteLine($"Getting all public Playlists sorted alphabetically took {watch.ElapsedMilliseconds} ms");
        }

        [Fact]
        public void GetAllPlaylistsThatArePublicAsDtosTest()
        {
            var db = new MongoDatabase();
            var watch = Stopwatch.StartNew();
            db.PlaylistRepository.Queryable
                .Where(p => p.IsPublic == true)
                .Select(p => new PlaylistDtod { Title = p.Title, guid = p.Id, Songs = p.Songs
                        .Select(s => new SongDtod { Titel = s.Titel, guid = s.Id }).ToList()}).ToList();
            watch.Stop();
            _output.WriteLine($"Getting all public Playlists and converting them to Dtos (projection) took {watch.ElapsedMilliseconds} ms");
        }
        [Fact]
        public void DeleteAllTest()
        {
            var db = new MongoDatabase();
            var watch = Stopwatch.StartNew();
            db.PlaylistRepository.DeleteAll();
            watch.Stop();
            _output.WriteLine($"Deleting all playlists took {watch.ElapsedMilliseconds} ms");
        }
        [Fact]
        public void UpdateAllTitles()
        {
            var db = new MongoDatabase();
            var watch = Stopwatch.StartNew();
            db.PlaylistRepository.Queryable.ToList().ForEach(p => p.Title = p.Title + " updated");
            watch.Stop();
            _output.WriteLine($"Updating all playlists' titles took {watch.ElapsedMilliseconds} ms");
        }
        
        // [Fact]
        // public void CountGradedTest()
        // {
        //     var manager = new ExamDatabase("127.0.0.1", "Exams");
        //     manager.Seed();
        //     var examQuery = manager.ExamRepository.Queryable;
        //     List<GradedExam> graded = examQuery.OfType<GradedExam>().ToList();
        //     Assert.True(graded.Count > 0);
        // }
    }
}