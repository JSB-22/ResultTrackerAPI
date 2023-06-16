using Microsoft.EntityFrameworkCore;
using ResultTracker.API.Models.Domain;

namespace ResultTracker.API.Data
{
	public class ResultTrackerDbContext : DbContext
	{
        public ResultTrackerDbContext(DbContextOptions<ResultTrackerDbContext> options) : base(options)
        {
        }
		public DbSet<TestResult> Results { get; set; }
		public DbSet<Subject> Subjects { get; set; }
		public DbSet<Topic> Topics { get; set; }
	}
}
