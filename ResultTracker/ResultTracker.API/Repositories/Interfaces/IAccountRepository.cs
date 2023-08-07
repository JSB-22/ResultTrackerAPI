using ResultTracker.API.Models.Domain;
using ResultTracker.API.Users.Domain;

namespace ResultTracker.API.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        public Task<List<Account>> GetAllAsync();
    }
}
