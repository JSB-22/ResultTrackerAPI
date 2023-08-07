using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ResultTracker.API.Repositories.Interfaces;
using ResultTracker.API.Users.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ResultTracker.API.Repositories
{
	public class TokenRepository : ITokenRepository
	{
		private readonly IConfiguration _config;
		public TokenRepository(IConfiguration configuration)
		{
			_config = configuration;
		}
		public string CreateJWTToken(IdentityUser user, List<string> roles)
		{
			var accountUser = (Account)user;

			//Create claims from roles: 
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Email, accountUser.Email),
				new Claim(ClaimTypes.Name, accountUser.FullName)
			};

			foreach (var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:key"]));

			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], claims, expires: DateTime.Now.AddMinutes(15), signingCredentials: credentials);
			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
