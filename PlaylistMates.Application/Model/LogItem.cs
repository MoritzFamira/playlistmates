using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlaylistMates.Application.Model
{
    public class LogItem : IEntity<int>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected LogItem() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        
        public LogItem(Account account, Action action)
        { 
            Account = account;
            Action = action;
        }
        [Key]
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public Account Account { get; set; }
        public Action Action { get; set; }
    }
}
