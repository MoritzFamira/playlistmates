using PlaylistMates.Application.Model;

namespace PlaylistMates.Application.Documents
{
    public class AccountPlatformsd
    {
        public AccountPlatformsd(Account account, Platform platform, string authtoken)
        {
            Platform = platform;
            Account = account;
            PlatformId = platform.Id;
            AccountId = account.Id;
            Authtoken = authtoken;
            Id = Guid.NewGuid();
        }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected AccountPlatformsd() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Guid Id { get; set; }
        public Account Account { get; set; }
        public int AccountId { get; set; }
        public Platform Platform { get; set; }
        public int PlatformId { get; set; }

        public string Authtoken { get; set; }
    }
}

