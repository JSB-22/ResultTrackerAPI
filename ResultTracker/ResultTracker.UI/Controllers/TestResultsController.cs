﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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

            var model = new AddTestResultViewModel();
            await PopulateModel<AddTestResultViewModel>(model, client);
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
        #region HelpersForSelectListItems
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
        public async Task PopulateModel<T>(T inputModel,HttpClient client)
        {
            //Utilise repsonse obj for clean up. 
            
            if (inputModel is null) throw new Exception();

			var topicData = await GetTopics(client);
			var subjectData = await GetSubjects(client);
			var accountData = await GetAccounts(client);

			if (HttpContext.User.IsInRole("Teacher"))
			{
				accountData = accountData.Where(t => t.TeacherName == HttpContext.User.Identity.Name).ToList();
			}

			if (inputModel is EditTestResultViewModel)
            {
                EditTestResultViewModel model = inputModel as EditTestResultViewModel;
				model.TopicSelectList = new List<SelectListItem>();
				foreach (var topic in topicData)
				{
					model.TopicSelectList.Add(new SelectListItem { Text = $"Year {topic.Year} : {topic.Name}", Value = topic.Id.ToString("D") });
				}
				model.SubjectSelectList = new List<SelectListItem>();
				foreach (var subject in subjectData)
				{
					model.SubjectSelectList.Add(new SelectListItem { Text = $"{subject.ExamBoard} - {subject.Name}", Value = subject.Id.ToString("D") });
				}
				model.AccountSelectList = new List<SelectListItem>();
				foreach (var account in accountData)
				{
					model.AccountSelectList.Add(new SelectListItem { Text = $"{account.StudentName}", Value = account.StudentId });
				}
			}
            else if (inputModel is AddTestResultViewModel)
            {
                AddTestResultViewModel model = inputModel as AddTestResultViewModel;
				model.TopicSelectList = new List<SelectListItem>();
				foreach (var topic in topicData)
				{
					model.TopicSelectList.Add(new SelectListItem { Text = $"Year {topic.Year} : {topic.Name}", Value = topic.Id.ToString("D") });
				}
				model.SubjectSelectList = new List<SelectListItem>();
				foreach (var subject in subjectData)
				{
					model.SubjectSelectList.Add(new SelectListItem { Text = $"{subject.ExamBoard} - {subject.Name}", Value = subject.Id.ToString("D") });
				}
				model.AccountSelectList = new List<SelectListItem>();
				foreach (var account in accountData)
				{
					model.AccountSelectList.Add(new SelectListItem { Text = $"{account.StudentName}", Value = account.StudentId });
				}
			}
            else
            {
                throw new Exception("Cannot handle obbjects of this type.");
            }

		}
        #endregion
        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var client = httpClientFactory.CreateClient();

                string token = HttpContext.User.FindFirst("TokenClaim").Value;

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var httpResponseMessage = await client.DeleteAsync($"https://localhost:7168/api/testresults/{id}");

                httpResponseMessage.EnsureSuccessStatusCode();

                return RedirectToAction("Index", "TestResults");
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id) // Must match asp route in index.  
        {
            var client = httpClientFactory.CreateClient();

            string token = HttpContext.User.FindFirst("TokenClaim").Value;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetFromJsonAsync<TestResultDto>($"https://localhost:7168/api/testresults/{id}");
            //
            if (response is null) return View(null);
			//
			var model = new EditTestResultViewModel();
			//
			model.Id = id;
			model.PercentageResult = response.PercentageResult;
			model.Notes = response.Notes;
			model.SelectedAccountId = response.Account.StudentId;
			model.SelectedTopicId = response.Topic.Id.ToString("D");
			model.SelectedSubjectId = response.Subject.Id.ToString("D");
            //
            await PopulateModel<EditTestResultViewModel>(model, client);
			return View(model);
        }
		[HttpPost]
		[Authorize(Roles = "Admin,Teacher")]
		public async Task<IActionResult> Edit(EditTestResultViewModel model)
		{
			var client = httpClientFactory.CreateClient();

			string token = HttpContext.User.FindFirst("TokenClaim").Value;

			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			//Conversion to input Dto: 
			var convertedModel = new EditTestResultRequestDto();
			convertedModel.Notes = model.Notes;
			convertedModel.PercentageResult = model.PercentageResult;
			convertedModel.TopicId = Guid.Parse(model.SelectedTopicId);
			convertedModel.SubjectId = Guid.Parse(model.SelectedSubjectId);
			convertedModel.StudentId = model.SelectedAccountId;
			//

			var httpRequestMessage = new HttpRequestMessage()
			{
				Method = HttpMethod.Put,
				RequestUri = new Uri($"https://localhost:7168/api/TestResults/{model.Id}"),
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
	}
}
