using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ResultTracker.API.Models.Dto;
using ResultTracker.API.Repositories.Interfaces;
using ResultTracker.API.Users.Domain;

namespace ResultTracker.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly UserManager<Account> _userManager;
		private readonly ITokenRepository _tokenRepository;
		public AuthController(UserManager<Account> userManager, ITokenRepository tokenRepository)
		{
			_userManager = userManager;
			_tokenRepository = tokenRepository;
		}
		//Post: /api/Auth/Register
		[HttpPost]
		[Route("Register")]
		public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
		{
			var identityUser = new Account
			{
				UserName = registerRequestDto.Username,
				Email = registerRequestDto.Username,
				FullName = registerRequestDto.FullName,
				TeacherId = registerRequestDto.TeacherId
			};
			var identityResult = await _userManager.CreateAsync(identityUser, registerRequestDto.Password);
			if (identityResult.Succeeded)
			{
					identityResult = await _userManager.AddToRolesAsync(identityUser, new string[] {"student"});
					if (identityResult.Succeeded)
					{
						return Ok("User was registered. You may now log in.");
					}
			}
			return BadRequest("Something went wrong...");
		}

		[HttpPost]
		[Route("Login")]
		public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
		{
			var user = await _userManager.FindByEmailAsync(loginRequestDto.Username);
			if (user != null)
			{
				var checkPasswordResult = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
				if (checkPasswordResult)
				{
					var roles = await _userManager.GetRolesAsync(user);
					if (roles != null)
					{
						//Create and return token;
						var newJwtToken = _tokenRepository.CreateJWTToken(user, roles.ToList());
						var repsonse = new LoginResponseDto() { JwtToken = newJwtToken };
						return Ok(repsonse);
					}

				}
				return BadRequest("Incorrect Password");
			}
			return BadRequest("Username or Password Incorrect");
		}


		[HttpPost]
		[Route("RoleChange")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> RoleChange([FromBody] RoleChangeRequestDto roleChangeRequestDto)
		{
			if (roleChangeRequestDto.Roles == null)
			{
				return BadRequest("New roles are required.");	
			}
			var userToBeEdited = await _userManager.FindByEmailAsync(roleChangeRequestDto.Username);
			if (userToBeEdited == null) return BadRequest("No Such user");
			var userRoles = await _userManager.GetRolesAsync(userToBeEdited);


			var editedResult = await _userManager.RemoveFromRolesAsync(userToBeEdited, userRoles);
			if (editedResult.Succeeded)
			{
				var updatedResult = await _userManager.AddToRolesAsync(userToBeEdited, roleChangeRequestDto.Roles);
				if (updatedResult.Succeeded)
				{
					string beforeRoles = "";
					string afterRoles = "";
					foreach (var role in userRoles)
					{
						beforeRoles += $"-{role}";
					}
					foreach (var role in roleChangeRequestDto.Roles)
					{
						afterRoles += $"-{role}";
					}
					return Ok($"User has been edited from: {beforeRoles} to: {afterRoles}");
				}
				return BadRequest("Roles cannot be updated");
			}
			return BadRequest("User cannot be removed from current role...");
		}
	}
}
