using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlaylistMates.Application.Model
{
    public class Account : IEntity<Email>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected Account() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public Account(Email email, string accountName, string salt, ICollection<Platform> platforms)
        {
            Email = email;
            AccountName = accountName;
            Salt = salt;
            Platforms = platforms;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Email Email { get; set; }
        public string AccountName { get; set; }
        public string Salt { get; set; }
        public ICollection<Platform> Platforms { get; set; }
        public Email Id => Email;
    }
}
