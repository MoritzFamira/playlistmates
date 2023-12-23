

using Bogus;
using MongoDB.Bson;
using PlaylistMates.Application.Documents;
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
        [Fact]
        public void SeedDatabaseTest()
        {
            var db = new MongoDatabase();
            db.PlaylistRepository.DeleteAll();
            
            Randomizer.Seed = new Random(1454);
            var rnd = new Randomizer();
            db.PlaylistRepository.InsertMany(new Faker<Playlistd>("de")
                .CustomInstantiator(f =>
                {
                    var playlist = new Playlistd(f.Lorem.Sentence(), f.Lorem.Word(), f.Random.Bool());
                    playlist.Songs.AddRange(new Faker<Songd>("de").CustomInstantiator(f =>
                    {
                        var song = new Songd(f.Lorem.Sentence(), f.Lorem.Sentence(), f.Date.Past(),
                            f.Random.Int(1000, 100000), new List<Artist>(), new List<Platform>());
                        return song;
                    }).Generate(20));
                    return playlist;
                })
                .Generate(10));
        }
        [Fact]
        public void AddSongToPlaylistTest()
        {
            var db = new MongoDatabase();
            var playlist = new Playlistd("Test", "Test", true);
            db.PlaylistRepository.InsertOne(playlist);
            var song = new Songd("asdfjkl;","title",new DateTime(2005,1,17),20000,new List<Artist>(),new List<Platform>());
            db.PlaylistRepository.AddSongToPlaylist(playlist, song);
            var updatedPlaylist = db.PlaylistRepository.FindOne(playlist.Id);
            _output.WriteLine(updatedPlaylist.ToJson());
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