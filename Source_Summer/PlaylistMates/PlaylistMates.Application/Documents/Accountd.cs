using System.Text.Json.Serialization;
using PlaylistMates.Application.Model;

namespace PlaylistMates.Application.Documents
{
    public class Accountd : IDocument<Guid>
    {
        public Accountd(string email, string accountName, string salt, string hashedPassword)
        {
            Email = email;
            AccountName = accountName;
            Salt = salt;
            HashedPassword = hashedPassword;
            Id = Guid.NewGuid();
        }
        public string Email { get; private set; }
        public string AccountName { get; set; }
        [JsonIgnore]
        public string HashedPassword { get; set; } 
        public string Salt { get; private set; } 
        public List<Platform> Platforms { get; } = new();
        public List<Playlist> Playlists { get; } = new();
        public List<AccountPlaylist> AccountPlaylists { get; } = new();
        public List<AccountPlatforms> AccountPlatforms { get; } = new();

        public Guid Id { get; }
    }
}
