using ExpertClinicTestTask.Domain.Core.Account;

namespace ExpertClinicTestTask.Domain.Interfaces.Account;

public interface IAccountRepository
{
    public Task<User> CreateUser(User user);
    public Task<IQueryable<User>> GetUsersAsync();
}