using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlaylistMates.Application.Model
{
    public class AccountPlaylist
    {

        /*
         * This entity represents the m:n relationship between Account and Playlist.
         * It is an explicit relationship since the extra column "Role" needed could not be implemented otherwise.
        */
        public AccountPlaylist(int playlist, int account, PlaylistRole role)
        {
            PlaylistId = playlist;
            AccountId = account;
            Role = role;
        }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected AccountPlaylist() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [Column(Order = 1)]
        [ForeignKey("Playlist")]
        public int PlaylistId { private set; get; }
        [Column(Order = 2)]
        [ForeignKey("Account")]
        public int AccountId { private set; get; }

        public PlaylistRole Role { get; set; }
    }
}
