using PlaylistMates.Application.Model;

namespace PlaylistMates.Application.Documents
{


    public class Songd : IDocument<Guid>
    {
        public Songd(string isrcCode, string title, DateTime releaseDate, int durationInMillis, List<Artist> artists, List<Platform> platforms)
        {
            // id is the isrc code of the song
            IsrcCode = isrcCode;
            Titel = title;
            ReleaseDate = releaseDate;
            DurationInMillis = durationInMillis;
            Artists = artists;
            Platforms = platforms;
            Guid = Guid.NewGuid();
            Id = Guid.NewGuid();
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected Songd() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        
        public Guid Id { get; private set; }
        public Guid Guid { get; set; }
        public string IsrcCode { get; set; }  // ISRC Code with ID (standardized with iso 3901)
        public string Titel { get; set; }  // name of the song
        public DateTime ReleaseDate { get; set; }  
        public int DurationInMillis { get; set; }   
        public List<Artist> Artists { get; set; }
        public List<Platform> Platforms { get; set; }
        protected List<SongCollection> _songCollections = new();
        public IReadOnlyCollection<SongCollection> SongCollections => _songCollections;


        public void AddSongCollection(SongCollection songCollection)
        {
            _songCollections.Add(songCollection);
        }
        public void RemoveSongCollection(SongCollection songCollection)
        {
            _songCollections.Remove(songCollection);
        }
    }
}
