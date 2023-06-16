using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistMates.Application.Dto
{
    public class PlaylistCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
    public class PlaylistUpdateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
    }
    public class PlaylistDto
    {
        public string Title { get; set; }
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public string Role { get; set; }
        public List<SongDto> Songs { get; set; } // Add the Songs property
    }

}
