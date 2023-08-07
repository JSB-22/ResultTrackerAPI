using Microsoft.EntityFrameworkCore;
using ResultTracker.API.Data;
using ResultTracker.API.Models.Domain;
using ResultTracker.API.Repositories.Interfaces;
using ResultTracker.API.Users.Domain;

namespace ResultTracker.API.Repositories
{
    public class SQLAccountRepository : IAccountRepository
    {
        private readonly ResultTrackerDbContext _context;

        public SQLAccountRepository(ResultTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<List<Account>> GetAllAsync()
        {
            return await _context.Accounts.Include("Teacher").ToListAsync();
        }
    }
}
