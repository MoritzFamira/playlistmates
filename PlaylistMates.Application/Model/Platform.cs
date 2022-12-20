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
        public int Id { get; set; }
        public string Name { get; set; }
        // TODO: think of more fields
    }
}
