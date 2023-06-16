using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ResultTracker.API.Users.Domain;

namespace ResultTracker.API.Data
{
	public class ResultTrackerAuthDbContext : IdentityDbContext
	{
        public ResultTrackerAuthDbContext(DbContextOptions<ResultTrackerAuthDbContext> options) : base(options)
        {
        }

		public DbSet<Account> Accounts { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			var studentId = "c2cd8a78-af3b-4088-9f65-323f3d7c49b9";
			var teacherId = "d3ac5e91-8ccb-4eca-8a2b-6dbb5d155b3b";
			var adminId = "d7dc84a1-aa48-4576-b482-e424dc697675";


			var roles = new List<IdentityRole>
			{
				new IdentityRole{ Id = studentId,ConcurrencyStamp = studentId,Name="Student",NormalizedName = "Student".ToUpper() },
				new IdentityRole{ Id = teacherId,ConcurrencyStamp = teacherId,Name="Teacher",NormalizedName = "Teacher".ToUpper() },
				new IdentityRole{ Id = adminId,ConcurrencyStamp = adminId,Name="Admin",NormalizedName = "Admin".ToUpper() }
			};

			builder.Entity<IdentityRole>().HasData(roles);
		}
	}
}
