using PlaylistMates.Application.Model;

namespace PlaylistMates.Application.Documents
{
    public class Albumd : SongCollection
    {
        public Albumd(List<Artist> artists)
        {
            _artists = artists;
        }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected Albumd() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected List<Artist> _artists { get; set; }
        public IReadOnlyCollection<Artist> Artists => _artists;

        // TODO: think about more fields

        // Is add really necessary? maybe only for flexibility
    }
}
