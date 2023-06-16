using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ResultTracker.API.Models.Domain;
using ResultTracker.API.Users.Domain;

namespace ResultTracker.API.Data
{
	public class ResultTrackerDbContext : IdentityDbContext
	{
        public ResultTrackerDbContext(DbContextOptions<ResultTrackerDbContext> options) : base(options)
        {
        }
		public DbSet<Account> Accounts { get; set; }
		public DbSet<TestResult> Results { get; set; }
		public DbSet<Subject> Subjects { get; set; }
		public DbSet<Topic> Topics { get; set; }
	}
}
