using ResultTracker.API.Data;
using ResultTracker.API.Models.Domain;
using ResultTracker.API.Repositories.Interfaces;

namespace ResultTracker.API.Repositories
{
	public class SQLTestResultRepository : ITestResultRepository
	{
		private readonly ResultTrackerDbContext _context;

		public SQLTestResultRepository(ResultTrackerDbContext context)
		{
			_context = context;
		}

		public async Task<TestResult> CreateAsync(TestResult result)
		{
			await _context.Results.AddAsync(result);
			await _context.SaveChangesAsync();
			return result;
		}

		public Task<TestResult?> DeleteAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		public Task<List<TestResult>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
		{
			throw new NotImplementedException();
		}

		public Task<TestResult?> GetByIdAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		public Task<TestResult?> UpdateAsync(Guid id, TestResult result)
		{
			throw new NotImplementedException();
		}
	}
}
