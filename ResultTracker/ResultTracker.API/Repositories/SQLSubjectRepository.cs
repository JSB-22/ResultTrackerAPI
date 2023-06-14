using Microsoft.EntityFrameworkCore;
using ResultTracker.API.Data;
using ResultTracker.API.Models.Domain;
using ResultTracker.API.Repositories.Interfaces;

namespace ResultTracker.API.Repositories
{
	public class SQLSubjectRepository : ISubjectRepository
	{
		private readonly ResultTrackerDbContext _context;

		public SQLSubjectRepository(ResultTrackerDbContext context)
		{
			_context = context;
		}

		public async Task<Subject?> CreateSubjectAsync(Subject subject)
		{
			await _context.Subjects.AddAsync(subject);
			await _context.SaveChangesAsync();
			return subject;
		}

		public async Task<Subject?> DeleteSubjectAsync(Guid id)
		{
			var existingSubject = await _context.Subjects.FirstOrDefaultAsync(s=>s.Id == id);
			if (existingSubject == null) return null;
			_context.Subjects.Remove(existingSubject);
			await _context.SaveChangesAsync();
			return existingSubject;
		}

		public async Task<List<Subject>> GetAllAsync()
		{
			return await _context.Subjects.ToListAsync();
		}

		public async Task<Subject?> GetByIdAsync(Guid id)
		{
			return await _context.Subjects.FirstOrDefaultAsync(t => t.Id == id);
		}

		public async Task<Subject?> UpdateSubjectAsync(Guid id, Subject subject)
		{
			var existingSubject = await _context.Subjects.FirstOrDefaultAsync(s => s.Id == id);
			if (existingSubject == null) return null;
			existingSubject.Name = subject.Name;
			existingSubject.ExamBoard = subject.ExamBoard;
			await _context.SaveChangesAsync();
			return existingSubject;
		}
	}
}
