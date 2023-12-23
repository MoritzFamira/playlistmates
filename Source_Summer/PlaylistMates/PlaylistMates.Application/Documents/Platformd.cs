using PlaylistMates.Application.Model;

namespace PlaylistMates.Application.Documents
{
    public class Platformd : IDocument<Guid>
    {
        public Platformd(string name)
        {
            Name= name;
            Id = Guid.NewGuid();
        }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected Platformd() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Guid Id { get; set; }
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public List<AccountPlatforms> AccountPlatforms { get; set; } = new();
        // TODO: think of more fields
    }
}
