using Microsoft.EntityFrameworkCore;
using ResultTracker.API.Models.Domain;

namespace ResultTracker.API.Data
{
	public class ResultTrackerDbContext : DbContext
	{
        public ResultTrackerDbContext(DbContextOptions options) : base(options)
        {
        }
		public DbSet<Result> Results { get; set; }
		public DbSet<Subject> Subjects { get; set; }
		public DbSet<Topic> Topics { get; set; }
	}
}
