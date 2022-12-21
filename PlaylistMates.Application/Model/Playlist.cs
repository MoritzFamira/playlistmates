using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistMates.Application.Model
{
    public class Playlist : SongCollection
    {
        public Playlist(string description, bool isPublic)
        {
            Description = description;
            IsPublic = isPublic;
        }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected Playlist() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        
        // TODO: way to store who playlist is shared with / who faved it (m:n) (waiting for account)
    }
}
