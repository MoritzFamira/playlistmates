using PlaylistMates.Application.Model;

namespace PlaylistMates.Application.Documents
{
    public class AccountPlaylistd : IDocument<Guid>
    {

        /*
         * This entity represents the m:n relationship between Account and Playlist.
         * It is an explicit relationship since the extra column "Role" needed could not be implemented otherwise.
        */
        public AccountPlaylistd(Playlistd playlist, Accountd account, PlaylistRoled role) 
        {
            Playlist = playlist;
            PlaylistId = playlist.Id;
            Account = account;
            AccountId = account.Id;
            Role = role;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected AccountPlaylistd() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Guid Id { get; set; }
        public Playlistd Playlist { get;  set; }
        public Guid PlaylistId { get; set; }
        public Accountd Account { get;  set; }
        public Guid AccountId { get; set; }

        public PlaylistRoled Role { get; set; }
    }
}
