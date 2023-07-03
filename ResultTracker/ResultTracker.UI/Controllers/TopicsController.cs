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
		//
		// ADD
		//
		[HttpGet]
		public IActionResult Add()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Add(AddTopicRequestDto model)
		{
			var client = httpClientFactory.CreateClient();

			string? token;

			HttpContext.Request.Cookies.TryGetValue("token", out token);

			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			var httpRequestMessage = new HttpRequestMessage()
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri("https://localhost:7168/api/topics"),
				Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")
			};
			var httpResponseMessage = await client.SendAsync(httpRequestMessage);
			httpResponseMessage.EnsureSuccessStatusCode();

			var response = await httpResponseMessage.Content.ReadFromJsonAsync<TopicDto>();
			if (response is not null) return RedirectToAction("Index", "Topics");
			return View();

		}
		//
		// EDIT
		//
		[HttpGet]
		public async Task<IActionResult> Edit(Guid id) // Must match asp route in index.  
		{
			var client = httpClientFactory.CreateClient();

			string? token;

			HttpContext.Request.Cookies.TryGetValue("token", out token);

			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			var response = await client.GetFromJsonAsync<TopicDto>($"https://localhost:7168/api/topics/{id}");
			if (response is not null) return View(response);
			return View(null);
		}
		[HttpPost]
		public async Task<IActionResult> Edit(TopicDto request)
		{
			var client = httpClientFactory.CreateClient();

			string? token;

			HttpContext.Request.Cookies.TryGetValue("token", out token);

			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			var httpRequest = new HttpRequestMessage()
			{
				Method = HttpMethod.Put,
				RequestUri = new Uri($"https://localhost:7168/api/topics/{request.Id}"),
				Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
			};
			var httpReponseMessage = await client.SendAsync(httpRequest);
			httpReponseMessage.EnsureSuccessStatusCode();

			var response = await httpReponseMessage.Content.ReadFromJsonAsync<TopicDto>();
			if (response is not null) return RedirectToAction("Index", "Topics");
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Delete(Guid id)
		{
			try
			{
				var client = httpClientFactory.CreateClient();

				string? token;

				HttpContext.Request.Cookies.TryGetValue("token", out token);

				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
				var httpResponseMessage = await client.DeleteAsync($"https://localhost:7168/api/topics/{id}");

				httpResponseMessage.EnsureSuccessStatusCode();

				return RedirectToAction("Index", "Topics");
			}
			catch (Exception)
			{

				throw;
			}
		}
	}
}
