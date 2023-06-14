using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
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

		public async Task<TestResult?> DeleteAsync(Guid id)
		{
			var testResultToDelete = await _context.Results.FirstOrDefaultAsync(tr => tr.Id == id);
			if (testResultToDelete == null) return null;
			_context.Results.Remove(testResultToDelete);
			await _context.SaveChangesAsync();
			return testResultToDelete;
		}

		public async Task<List<TestResult>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
		{
			var testResults = _context.Results.Include("Topic").Include("Subject");
			//Filter query:
			if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
			{
				if (filterOn.Equals("Topic", StringComparison.OrdinalIgnoreCase))
				{
					testResults = testResults.Where(tr => tr.Topic.Name.ToLower().Contains(filterQuery.ToLower()));
				}
				else 
				{
					testResults = testResults.Where(tr => tr.Subject.Name.ToLower().Contains(filterQuery.ToLower()));
				}
			}
			//Sorting:
			if(!string.IsNullOrWhiteSpace(sortBy))
			{
				if (sortBy.Equals("Topic", StringComparison.OrdinalIgnoreCase))
				{
					testResults = isAscending ? testResults.OrderBy(tr => tr.Topic.Name) : testResults.OrderByDescending(tr => tr.Topic.Name);
				}
				else if (sortBy.Equals("Subject", StringComparison.OrdinalIgnoreCase))
				{
					testResults = isAscending ? testResults.OrderBy(tr => tr.Subject.Name) : testResults.OrderByDescending(tr => tr.Subject.Name);
				}
				else 
				{
					testResults = isAscending ? testResults.OrderBy(tr => tr.PercentageResult) : testResults.OrderByDescending(tr => tr.PercentageResult);
				}
			}

			//Pagination: 
			var skipped = (pageNumber - 1) * pageSize;
			return await testResults.Skip(skipped).Take(pageSize).ToListAsync();
		}

		public async Task<TestResult?> GetByIdAsync(Guid id)
		{
			return await _context.Results.Include(r => r.Subject).Include(r => r.Topic).FirstOrDefaultAsync(r => r.Id == id);
		}

		public async Task<TestResult?> UpdateAsync(Guid id, TestResult result)
		{
			var exisitingTestResult = await _context.Results.FirstOrDefaultAsync(tr => tr.Id == id);
			if (exisitingTestResult == null) return null;

			exisitingTestResult.Notes = result.Notes;
			exisitingTestResult.PercentageResult = result.PercentageResult;
			exisitingTestResult.SubjectId = result.SubjectId;
			exisitingTestResult.TopicId = result.TopicId;
			await _context.SaveChangesAsync();
			return exisitingTestResult;
		}
	}
}
