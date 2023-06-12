using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlaylistMates.Application.Model
{
    public class AccountPlaylist : IEntity<int>
    {

        /*
         * This entity represents the m:n relationship between Account and Playlist.
         * It is an explicit relationship since the extra column "Role" needed could not be implemented otherwise.
        */
        public AccountPlaylist(Playlist playlist, Account account, PlaylistRole role) 
        {
            Playlist = playlist;
            PlaylistId = playlist.Id;
            Account = account;
            AccountId = account.Id;
            Role = role;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected AccountPlaylist() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public int Id { get; set; }
        public Playlist Playlist { get;  set; }
        public int PlaylistId { get; set; }
        public Account Account { get;  set; }
        public int AccountId { get; set; }

        public PlaylistRole Role { get; set; }
    }
}
