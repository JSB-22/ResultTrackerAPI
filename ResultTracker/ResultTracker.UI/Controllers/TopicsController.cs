using Microsoft.AspNetCore.Mvc;
using ResultTracker.UI.Models.Dto;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Identity;
using System.Net.Http.Headers;

namespace ResultTracker.UI.Controllers
{
	public class TopicsController : Controller
	{
		private readonly IHttpClientFactory httpClientFactory;

		public TopicsController(IHttpClientFactory httpClientFactory)
		{
			this.httpClientFactory = httpClientFactory;
		}
		public async  Task<IActionResult> Index()
		{
			List<TopicDto> response = new List<TopicDto>();
			//Get all regions from web API: (HTTPclient class, way to consume web API's).
			try
			{
				var client = httpClientFactory.CreateClient();

				string? token;

				HttpContext.Request.Cookies.TryGetValue("token", out token);

				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

				var httpResponseMessage = await client.GetAsync("https://localhost:7168/api/Topics"); //Goes to host and then the url. 

				httpResponseMessage.EnsureSuccessStatusCode();

				response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<TopicDto>>());
			}
			catch (Exception ex)
			{

				throw;
			}

			return View(response);
		}
	}
}
