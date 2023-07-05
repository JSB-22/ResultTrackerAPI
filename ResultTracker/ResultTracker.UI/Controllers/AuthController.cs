using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResultTracker.UI.Models.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace ResultTracker.UI.Controllers
{
	[AllowAnonymous]
	public class AuthController : Controller
	{
		private readonly IHttpClientFactory httpClientFactory;

		
		public AuthController(IHttpClientFactory httpClientFactory)
		{
			this.httpClientFactory = httpClientFactory;
		}
		public IActionResult AccessDenied()
		{
			return View();
		}


		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
		{
			var client = httpClientFactory.CreateClient();

			var httpRequestMessage = new HttpRequestMessage()
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri("https://localhost:7168/api/Auth/Login"),
				Content = new StringContent(JsonSerializer.Serialize(loginRequestDto), Encoding.UTF8, "application/json")
			};

			var httpResponseMessage = await client.SendAsync(httpRequestMessage);
			if (!httpResponseMessage.IsSuccessStatusCode)
			{
				return Redirect("/login");
			}
			else
			{
				var token = await GetJWTTokenStringFromHttpResponse(httpResponseMessage);

				await SignInUser( GetUserRoleFromJWTTokenString(token),token );

				return RedirectToAction("Index","Home");
			}
			//HttpContext.Response.Cookies.Append("token", token, new CookieOptions() {Expires = DateTime.Now.AddMinutes(15)}); (OLD) 
		}
		[HttpPost]
		public async Task<IActionResult> Logout()
		{
            //Needs some editing and understanding here. 
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Cookies.Delete("RT_auth_cookie");
            return RedirectToAction("login", "Auth");
        }


		#region Note for claim reading: 
		// -- NO LONGER IN USE -- // 
		// -- Here for notes   -- // 
/*		[HttpGet]
		[Route("/read-claims")]
		public IActionResult TestClaimReading()
		{
			var userRoles = HttpContext.User.FindFirst("RoleClaim");
			var customClaim = HttpContext.User.FindFirst("TokenClaim");
			return Content($"User is in {userRoles} has custom claim value: {customClaim.Value}");
		}*/
		#endregion


		public async Task SignInUser(string userRole, string userToken)
		{
			var claims = new List<Claim>
			{
				new Claim("RoleClaim", userRole),
				new Claim("TokenClaim", userToken),
				new Claim(ClaimTypes.Role,userRole)
			};

			var claimsIdentity = new ClaimsIdentity(
				claims, CookieAuthenticationDefaults.AuthenticationScheme);

			await HttpContext.SignInAsync(
				CookieAuthenticationDefaults.AuthenticationScheme,
				new ClaimsPrincipal(claimsIdentity));
		
		}

		public string GetUserRoleFromJWTTokenString(string tokenString)
		{
			//Test: 
			var handler = new JwtSecurityTokenHandler();
			var jsonToken = handler.ReadToken(tokenString);
			var tokenS = jsonToken as JwtSecurityToken;
			//
			// Possible need for exception.
			// 
			if (tokenS is null) return "";
			var claim = tokenS.Claims.FirstOrDefault(j => j.Type.EndsWith("/role"));
			return claim is null ? "" : claim.Value; 
		}

		public async Task<string> GetJWTTokenStringFromHttpResponse(HttpResponseMessage httpResponseMessage)
		{
			if (httpResponseMessage.Content is null) throw new ArgumentNullException("No content response given");

			var response = await httpResponseMessage.Content.ReadFromJsonAsync<LoginResponseDto>();
			if (response is not null)
			{
				return response.JwtToken;
			}
			else
			{
				throw new ArgumentNullException("Response object is null");
			}
		}
	}
}
