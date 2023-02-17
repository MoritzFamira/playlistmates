using Microsoft.EntityFrameworkCore;
using PlaylistMates.Application.Infrastructure;
using PlaylistMates.Application.Model;

namespace PlaylistMates.Application.Infrastructure;


public class AccountRepository : Repository<Account, int>
{
    protected readonly DbSet<Account> _accounts;
    
    public AccountRepository(DbContext context) : base(context)
    {
        _accounts = context.Set<Account>();
    }

    public void UpdateName(string accountName)
    {
        // check if accountName exists etc
    }
}