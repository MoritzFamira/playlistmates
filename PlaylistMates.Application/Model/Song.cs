using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PlaylistMates.Application.Model
{


    public class Song : IEntity<string>
    {
        public Song(string id, string title, DateTime releaseDate, int durationInMillis, ICollection<Artist> artists, ICollection<Platform> platforms)
        {
            // id is the isrc code of the song
            Id = id;
            Titel = title;
            ReleaseDate = releaseDate;
            DurationInMillis = durationInMillis;
            Artists = artists;
            Platforms = platforms;
        }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected Song() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; private set; }  // ISRC Code with ID (standardized with iso 3901)
        public string Titel { get; set; }  // name of the song
        public DateTime ReleaseDate { get; set; }  
        public int DurationInMillis { get; set; }   
        public ICollection<Artist> Artists { get; set; }
        public ICollection<Platform> Platforms { get; set; }
    }
}
