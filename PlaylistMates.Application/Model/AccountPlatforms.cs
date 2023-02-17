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
        public AccountPlatforms(Account account, Platform platform, string authtoken)
        {
            Platform = platform;
            Account = account;
            PlatformId = platform.Id;
            AccountId = account.Id;
            Authtoken = authtoken;
        }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected AccountPlatforms() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public int Id { get; set; }
        public Account Account { get; set; }
        public int AccountId { get; set; }
        public Platform Platform { get; set; }
        public int PlatformId { get; set; }

        public string Authtoken { get; set; }
    }
}

