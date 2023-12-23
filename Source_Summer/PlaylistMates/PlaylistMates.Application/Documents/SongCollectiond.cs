using MongoDB.Bson.Serialization.Attributes;
using PlaylistMates.Application.Model;

namespace PlaylistMates.Application.Documents
{
    [BsonDiscriminator(RootClass = true)]
    [BsonKnownTypes(typeof(Playlistd))]
    public class SongCollectiond : IDocument<Guid>
    {
        public SongCollectiond(string title, DateTime creationDate)
        {
            Title = title;
            CreationDate = creationDate;
            Id = Guid.NewGuid();
        }
        public SongCollectiond(string title, DateTime creationDate, List<Songd> songs)
        {
            Title = title;
            CreationDate = creationDate;
            Songs = songs;
            Id = Guid.NewGuid();
        }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected SongCollectiond() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Guid Id { get; private set; }
        public Guid Guid { get; set; }
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }
        
        public List<Songd> Songs = new();
    }
}
