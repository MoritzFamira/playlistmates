using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistMates.Application.Model
{
    public class Platform : IEntity<int>
    {
        public Platform(int id, string name)
        {
            Id = id;
            Name= name;
        }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected Platform() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public int Id { get; set; }
        public string Name { get; set; }
        public List<AccountPlatforms> AccountPlatforms { get; set; } = new();
        // TODO: think of more fields
    }
}
