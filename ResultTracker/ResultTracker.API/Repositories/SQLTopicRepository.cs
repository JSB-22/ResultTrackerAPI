using Microsoft.EntityFrameworkCore;
using ResultTracker.API.Data;
using ResultTracker.API.Models.Domain;
using ResultTracker.API.Repositories.Interfaces;

namespace ResultTracker.API.Repositories
{
	public class SQLTopicRepository : ITopicRepository
	{
		private readonly ResultTrackerDbContext _context;

		public SQLTopicRepository(ResultTrackerDbContext context)
		{
			_context = context;
		}

		public async Task<Topic?> CreateTopicAsync(Topic topic)
		{
			await _context.Topics.AddAsync(topic);
			await _context.SaveChangesAsync();
			return topic;
		}

		public async Task<Topic?> DeleteTopicAsync(Guid id)
		{
			var existingTopic = await _context.Topics.FirstOrDefaultAsync(t => t.Id == id);
			if (existingTopic == null) return null;
			_context.Topics.Remove(existingTopic);
			await _context.SaveChangesAsync();
			return existingTopic;
		}

		public async Task<List<Topic>> GetAllAsync()
		{
			return await _context.Topics.ToListAsync();
		}

		public async Task<Topic?> GetByIdAsync(Guid id)
		{
			return await _context.Topics.FirstOrDefaultAsync(t => t.Id == id);
		}

		public async Task<Topic?> UpdateTopicAsync(Guid id, Topic topic)
		{
			var existingTopic = await _context.Topics.FirstOrDefaultAsync(t => t.Id == id);
			if (existingTopic == null) return null;
			existingTopic.Name = topic.Name;
			existingTopic.Year = topic.Year;
			await _context.SaveChangesAsync();
			return existingTopic;
		}
	}
}
