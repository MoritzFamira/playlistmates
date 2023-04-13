using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistMates.Application.Dto
{
    public class AccountDtoWithPassword
    {
        public int Id { get; set; }
        public string Email { get; private set; }
        public string AccountName { get; set; }
        public string Password { get; set; }
    }

    public class AccountDtoWithoutPassword
    {
        public int Id { get; set; }
        public string Email { get; private set; }
        public string AccountName { get; set; }
    }
}
