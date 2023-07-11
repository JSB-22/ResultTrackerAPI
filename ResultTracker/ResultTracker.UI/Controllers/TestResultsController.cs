using Microsoft.AspNetCore.Mvc;
using ResultTracker.UI.Models.Dto;
using System.Net.Http.Headers;

namespace ResultTracker.UI.Controllers
{
    public class TestResultsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public TestResultsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> Index()
        {
            List<TestResultDto> response = new List<TestResultDto>();
            //Get all regions from web API: (HTTPclient class, way to consume web API's).
            try
            {
                var client = httpClientFactory.CreateClient();

                string token = HttpContext.User.FindFirst("TokenClaim").Value;

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var httpResponseMessage = await client.GetAsync("https://localhost:7168/api/TestResults"); //Goes to host and then the url. 

                httpResponseMessage.EnsureSuccessStatusCode();

                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<TestResultDto>>());
            }
            catch (Exception ex)
            {

                throw;
            }

            return View(response);
        }
    }
}
