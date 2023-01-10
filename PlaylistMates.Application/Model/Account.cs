﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlaylistMates.Application.Model
{
    [Index(nameof(Email), IsUnique = true)]
    public class Account : IEntity<int>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected Account() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public Account(string email, string accountName, string salt)
        {
            Email = email;
            AccountName = accountName;
            Salt = salt;
        }
        public int Id { get; set; }
        public string Email { get; private set; }
        public string AccountName { get; set; }
        public string Salt { get; set; }
        public List<Platform> Platforms { get;  } = new();
        public List<Playlist> Playlists { get;  } = new();
        public List<AccountPlaylist> AccountPlaylists { get;  } = new();

    }
}
