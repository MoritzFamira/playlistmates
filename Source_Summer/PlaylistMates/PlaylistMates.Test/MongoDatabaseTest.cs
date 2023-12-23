

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
            db.PlaylistRepository.InsertOne(new Playlistd("Test", "Test", true));
            db.PlaylistRepository.Queryable.ToList().ForEach(p => Console.WriteLine(p.Title));
            // db.Seed();
            // var repo = db.StudentRepository;
            // Assert.True(repo.Queryable.Any());
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