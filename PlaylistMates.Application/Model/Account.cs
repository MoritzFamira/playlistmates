using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlaylistMates.Application.Model
{
    public class Account : IEntity<string>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected Account() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public Account(string email, string accountName, string salt, ICollection<Platform> platforms, ICollection<Playlist> playlists, ICollection<AccountPlaylist> accountPlaylists)
        {
            Email = email;
            AccountName = accountName;
            Salt = salt;
            Platforms = platforms;
            Playlists = playlists;
            AccountPlaylists = accountPlaylists;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Email { get; private set; }
        public string AccountName { get; set; }
        public string Salt { get; set; }
        public ICollection<Platform> Platforms { get; set; }
        public ICollection<Playlist> Playlists { get; set; }
        public string Id => Email;
        public ICollection<AccountPlaylist> AccountPlaylists { get; set; }

    }
}
