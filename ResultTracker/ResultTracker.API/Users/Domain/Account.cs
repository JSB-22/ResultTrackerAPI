using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using ResultTracker.API.Models.Domain;

namespace ResultTracker.API.Users.Domain
{
	public class Account : IdentityUser
	{
		public string FullName { get; set; } = string.Empty;
		public string? TeacherId { get; set; }
		public Account Teacher { get; set; }
		public List<TestResult>? Results { get; set; }
	}
}
