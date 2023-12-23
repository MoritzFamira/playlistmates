using MongoDB.Bson.Serialization.Attributes;
using PlaylistMates.Application.Model;

namespace PlaylistMates.Application.Documents
{
    [BsonDiscriminator("Playlistd")]
    public class Playlistd : SongCollectiond
    {
        public Playlistd(string description,string title, bool? isPublic) : base(title, DateTime.Now, new List<Songd>())
        {
            Description = description;
            IsPublic = isPublic;
        }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        //protected Playlistd() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Playlistd()
        {
        }

        public string Description { get; set; }
        public bool? IsPublic { get; set; }
        public List<AccountPlaylist> AccountPlaylists { get; set; } = new();
    }
}
