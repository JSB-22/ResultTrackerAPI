using Microsoft.AspNetCore.Mvc;
using ResultTracker.UI.Models.Dto;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ResultTracker.UI.Controllers
{
	public class SubjectsController : Controller
	{
		private readonly IHttpClientFactory httpClientFactory;

		public SubjectsController(IHttpClientFactory httpClientFactory)
		{
			this.httpClientFactory = httpClientFactory;
		}
		public async Task<IActionResult> Index()
		{
			List<SubjectDto> response = new List<SubjectDto>();
			//Get all regions from web API: (HTTPclient class, way to consume web API's).
			try
			{
				var client = httpClientFactory.CreateClient();

				string token = HttpContext.User.FindFirst("TokenClaim").Value;

				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

				var httpResponseMessage = await client.GetAsync("https://localhost:7168/api/Subjects"); //Goes to host and then the url. 

				httpResponseMessage.EnsureSuccessStatusCode();

				response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<SubjectDto>>());
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
		public async Task<IActionResult> Add(AddSubjectRequestDto model)
		{
			var client = httpClientFactory.CreateClient();

			string token = HttpContext.User.FindFirst("TokenClaim").Value;

			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			var httpRequestMessage = new HttpRequestMessage()
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri("https://localhost:7168/api/subjects"),
				Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")
			};
			var httpResponseMessage = await client.SendAsync(httpRequestMessage);

			if (httpResponseMessage.IsSuccessStatusCode)
			{
				var response = await httpResponseMessage.Content.ReadFromJsonAsync<SubjectDto>();
				if (response is not null) return RedirectToAction("Index", "Subjects");
			}
			return View(model);
		}
		//
		// EDIT
		//
		[HttpGet]
		public async Task<IActionResult> Edit(Guid id) // Must match asp route in index.  
		{
			var client = httpClientFactory.CreateClient();

			string token = HttpContext.User.FindFirst("TokenClaim").Value;

			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			var response = await client.GetFromJsonAsync<SubjectDto>($"https://localhost:7168/api/subjects/{id}");
			if (response is not null) return View(response);
			return View(null);
		}
		[HttpPost]
		public async Task<IActionResult> Edit(SubjectDto request)
		{
			var client = httpClientFactory.CreateClient();

			string token = HttpContext.User.FindFirst("TokenClaim").Value;

			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			var httpRequest = new HttpRequestMessage()
			{
				Method = HttpMethod.Put,
				RequestUri = new Uri($"https://localhost:7168/api/subjects/{request.Id}"),
				Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
			};
			var httpReponseMessage = await client.SendAsync(httpRequest);
			httpReponseMessage.EnsureSuccessStatusCode();

			var response = await httpReponseMessage.Content.ReadFromJsonAsync<SubjectDto>();
			if (response is not null) return RedirectToAction("Index", "Subjects");
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Delete(Guid id)
		{
			try
			{
				var client = httpClientFactory.CreateClient();

				string token = HttpContext.User.FindFirst("TokenClaim").Value;

				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
				var httpResponseMessage = await client.DeleteAsync($"https://localhost:7168/api/subjects/{id}");

				httpResponseMessage.EnsureSuccessStatusCode();

				return RedirectToAction("Index", "SUbjects");
			}
			catch (Exception)
			{

				throw;
			}
		}
	}
}
