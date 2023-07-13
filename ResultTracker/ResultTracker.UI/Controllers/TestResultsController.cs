using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ResultTracker.UI.Models.Dto;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ResultTracker.UI.Controllers
{
    [Authorize]
    public class TestResultsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public TestResultsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        [Authorize(Roles ="Admin,Teacher,Student")]
        public async Task<IActionResult> Index()
        {
            List<TestResultDto> response = new List<TestResultDto>();
            //Get all regions from web API: (HTTPclient class, way to consume web API's).
            try
            {
                var client = httpClientFactory.CreateClient();

                string token = HttpContext.User.FindFirst("TokenClaim").Value;

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage httpResponseMessage; 

                if (HttpContext.User.IsInRole("Student"))
                {
                    var studentName = HttpContext.User.Identity.Name;
					httpResponseMessage = await client.GetAsync($"https://localhost:7168/api/TestResults?filterOn=Student&filterQuery={studentName}");
				}
                else 
                {
					httpResponseMessage = await client.GetAsync("https://localhost:7168/api/TestResults"); //Goes to host and then the url. 
				}

                httpResponseMessage.EnsureSuccessStatusCode();

                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<TestResultDto>>());
            }
            catch (Exception ex)
            {

                throw;
            }

            return View(response);
        }
        [Authorize(Roles ="Admin,Teacher")]
        public async Task<IActionResult> Add()
        {
            var client = httpClientFactory.CreateClient();

            string token = HttpContext.User.FindFirst("TokenClaim").Value;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var topicData = await GetTopics(client);
            var subjectData = await GetSubjects(client);
            var accountData = await GetAccounts(client);

            var model = new AddTestResultViewModel();
            model.TopicSelectList = new List<SelectListItem>();
            foreach (var topic in topicData)
            {
                model.TopicSelectList.Add(new SelectListItem {Text = $"Year {topic.Year} : {topic.Name}",Value = topic.Id.ToString("D") });
            }
            model.SubjectSelectList = new List<SelectListItem>();
            foreach (var subject in subjectData)
            {
                model.SubjectSelectList.Add(new SelectListItem { Text = $"{subject.ExamBoard} - {subject.Name}", Value = subject.Id.ToString("D") });
            }
            model.AccountSelectList = new List<SelectListItem>();
            foreach (var account in accountData)
            {
                model.AccountSelectList.Add(new SelectListItem { Text = $"{account.StudentName}", Value = account.StudentId});
            }
            return View(model);
        }
        [HttpPost]
		[Authorize(Roles = "Admin,Teacher")]
		public async Task<IActionResult> Add(AddTestResultViewModel model)
        {
			var client = httpClientFactory.CreateClient();

			string token = HttpContext.User.FindFirst("TokenClaim").Value;

			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //Conversion to input Dto: 
            var convertedModel = new AddTestResultRequestDto();
            convertedModel.Notes = model.Notes;
            convertedModel.PercentageResult = model.PercentageResult;
            convertedModel.TopicId = Guid.Parse(model.SelectedTopicId);
            convertedModel.SubjectId = Guid.Parse(model.SelectedSubjectId);
            convertedModel.StudentId = model.SelectedAccountId;
            //

			var httpRequestMessage = new HttpRequestMessage()
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri("https://localhost:7168/api/TestResults"),
				Content = new StringContent(JsonSerializer.Serialize(convertedModel), Encoding.UTF8, "application/json")
			};
			var httpResponseMessage = await client.SendAsync(httpRequestMessage);

			if (httpResponseMessage.IsSuccessStatusCode)
			{
				var response = await httpResponseMessage.Content.ReadFromJsonAsync<TestResultDto>();
				if (response is not null) return RedirectToAction("Index", "TestResults");
			}
			return View(model);
		}
        #region HelpersForAdd
        public async Task<List<TopicDto>> GetTopics(HttpClient client)
        {
            try
            {
                var response = new List<TopicDto>();

                var httpResponseMessage = await client.GetAsync("https://localhost:7168/api/Topics"); //Goes to host and then the url. 

                httpResponseMessage.EnsureSuccessStatusCode();

                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<TopicDto>>());

                return response; 
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<List<SubjectDto>> GetSubjects(HttpClient client)
        {
            try
            {
                var response = new List<SubjectDto>();

                var httpResponseMessage = await client.GetAsync("https://localhost:7168/api/Subjects"); //Goes to host and then the url. 

                httpResponseMessage.EnsureSuccessStatusCode();

                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<SubjectDto>>());

                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<List<AccountDto>> GetAccounts(HttpClient client)
        {
            try
            {
                var response = new List<AccountDto>();

                var httpResponseMessage = await client.GetAsync("https://localhost:7168/api/Accounts"); //Goes to host and then the url. 

                httpResponseMessage.EnsureSuccessStatusCode();

                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<AccountDto>>());

                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
    }
}
