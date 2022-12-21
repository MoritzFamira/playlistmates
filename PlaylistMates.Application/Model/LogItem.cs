using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistMates.Application.Model
{
    public class LogItem
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected LogItem() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        
        public LogItem(int id, DateTime timeStamp, Account account)
        {
            Id = id;
            TimeStamp = timeStamp;
            Account = account;
        }
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public Account Account { get; set; }
    }
}
