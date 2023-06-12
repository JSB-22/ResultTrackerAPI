using ResultTracker.API.Models.Domain;

namespace ResultTracker.API.Repositories.Interfaces
{
	public interface ITestResultRepository
	{
		public Task<TestResult> CreateAsync(TestResult result);
		public Task<List<TestResult>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000);
		public Task<TestResult?> GetByIdAsync(Guid id);
		public Task<TestResult?> UpdateAsync(Guid id, TestResult result);
		public Task<TestResult?> DeleteAsync(Guid id);
	}
}
