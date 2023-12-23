using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text.Json.Serialization;

namespace PlaylistMates.Application.Model
{
    [Index(nameof(Email), IsUnique = true)]
    public class Account : IEntity<int>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected Account() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public Account(string email, string accountName, string salt, string hashedPassword)
        {
            Email = email;
            AccountName = accountName;
            Salt = salt;
            HashedPassword = hashedPassword;
        }

        public int Id { get; private set; }
        public string Email { get; private set; }
        public string AccountName { get; set; }
        [JsonIgnore]
        public string HashedPassword { get; set; } 
        public string Salt { get; private set; } 
        public List<Platform> Platforms { get; } = new();
        public List<Playlist> Playlists { get; } = new();
        public List<AccountPlaylist> AccountPlaylists { get; } = new();
        public List<AccountPlatforms> AccountPlatforms { get; } = new();

    }
}
