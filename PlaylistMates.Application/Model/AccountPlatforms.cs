using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;


namespace PlaylistMates.Application.Model
{
    public class AccountPlatforms
    {
        public AccountPlatforms(int account, int platform, string authtoken)
        {
            PlatformId = platform;
            AccountId = account;
            Authtoken = authtoken;
        }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected AccountPlatforms() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [Column(Order = 1)]
        [ForeignKey("Account")]
        public int AccountId { private set; get; }
        [Column(Order = 2)]
        [ForeignKey("Platform")]
        public int PlatformId { private set; get; }

        public string Authtoken { get; set; }
    }
}

