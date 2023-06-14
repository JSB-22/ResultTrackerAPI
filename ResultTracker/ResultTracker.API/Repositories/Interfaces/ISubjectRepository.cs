using ResultTracker.API.Models.Domain;

namespace ResultTracker.API.Repositories.Interfaces
{
	public interface ISubjectRepository
	{
		public Task<List<Subject>> GetAllAsync();
		public Task<Subject?> GetByIdAsync(Guid id);
		public Task<Subject?> CreateSubjectAsync(Subject subject);
		public Task<Subject?> DeleteSubjectAsync(Guid id);
		public Task<Subject?> UpdateSubjectAsync(Guid id, Subject subject);
	}
}
