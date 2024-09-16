using ExpertClinicTestTask.Domain.Core.Account;
using ExpertClinicTestTask.Domain.Interfaces.Account;
using Microsoft.EntityFrameworkCore;

namespace ExpertClinicTestTask.Infrastructure.Data.Repository;

public class AccountRepository : IAccountRepository
{
    private readonly DbContextFactory _dbContextFactory;

    public AccountRepository(DbContextFactory dbContextFactory)
    {
        this._dbContextFactory = dbContextFactory;
    }

    public async Task<User> CreateUser(User user)
    {
        using (var db = _dbContextFactory.Create(typeof(AccountRepository)))
        {
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
            return user;
        }
    }
    public async Task<IQueryable<User>> GetUsersAsync()
    {
        var db = _dbContextFactory.Create(typeof(AccountRepository));
        return db.Users;
    }
}