using ResultTracker.API.Models.Domain;

namespace ResultTracker.API.Repositories.Interfaces
{
	public interface ITopicRepository
	{
		public Task<List<Topic>> GetAllAsync();
		public Task<Topic?> GetByIdAsync(Guid id);
		public Task<Topic?> CreateTopicAsync(Topic topic);
		public Task<Topic?> DeleteTopicAsync(Guid id);
		public Task<Topic?> UpdateTopicAsync(Guid id, Topic topic);

	}
}
