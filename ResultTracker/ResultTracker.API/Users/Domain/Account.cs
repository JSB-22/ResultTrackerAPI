using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;

namespace ResultTracker.API.Users.Domain
{
	public class Account : IdentityUser
	{
		public string FullName { get; set; } = string.Empty;
		public string? TeacherId { get; set; }
		public Account Teacher { get; set; }
	}
}
