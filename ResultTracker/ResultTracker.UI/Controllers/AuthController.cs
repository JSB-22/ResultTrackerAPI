using Microsoft.AspNetCore.Mvc;
using ResultTracker.UI.Models.Dto;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace ResultTracker.UI.Controllers
{
	public class AuthController : Controller
	{
		private readonly IHttpClientFactory httpClientFactory;

		public AuthController(IHttpClientFactory httpClientFactory)
		{
			this.httpClientFactory = httpClientFactory;
		}
		public IActionResult Index()
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
			httpResponseMessage.EnsureSuccessStatusCode();

			var response = await httpResponseMessage.Content.ReadFromJsonAsync<LoginResponseDto>();
			string token;
			if (response is not null)
			{
				token = response.JwtToken;
			}
			else 
			{
				throw new NullReferenceException("Response payload is null");
			}

			HttpContext.Response.Cookies.Append("token", token, new CookieOptions() {Expires = DateTime.Now.AddMinutes(15)});

			return RedirectToAction("Index", "Home");
		}
	}
}
