using PlaylistMates.Application.Model;

namespace PlaylistMates.Application.Documents
{
    public class Playlistd : SongCollectiond
    {
        public Playlistd(string description,string title, bool? isPublic)
        {
            Title = title;
            Description = description;
            IsPublic = isPublic;
        }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected Playlistd() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Description { get; set; }
        public bool? IsPublic { get; set; }
        public List<AccountPlaylist> AccountPlaylists { get; set; } = new();
        
        // TODO: way to store who playlist is shared with / who faved it (m:n) (waiting for account)
    }
}
