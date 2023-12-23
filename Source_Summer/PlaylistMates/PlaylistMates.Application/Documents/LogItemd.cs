using System.ComponentModel.DataAnnotations;
using PlaylistMates.Application.Model;
using Action = PlaylistMates.Application.Model.Action;

namespace PlaylistMates.Application.Documents
{
    public class LogItemd : IDocument<Guid>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected LogItemd() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        
        public LogItemd(Account account, Action action)
        { 
            Account = account;
            Action = action;
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public Account Account { get; set; }
        public Action Action { get; set; }
    }
}
