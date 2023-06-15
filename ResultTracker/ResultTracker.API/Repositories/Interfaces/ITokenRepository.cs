using Microsoft.AspNetCore.Identity;

namespace ResultTracker.API.Repositories.Interfaces
{
	public interface ITokenRepository
	{
		string CreateJWTToken(IdentityUser user, List<string> roles);
	}
}
