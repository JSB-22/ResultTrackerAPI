using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ResultTracker.API.Data;
using ResultTracker.API.Models.Domain;
using ResultTracker.API.Repositories;
using ResultTracker.API.Users.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Microsoft.Identity.Client;

namespace APITests
{
	public class SQLTestResultRepositoryTests
	{
		private SQLTestResultRepository? _sut;
		private ResultTrackerDbContext _context;
		private Mock<UserManager<Account>> _userManager;

		[SetUp]
		public void Setup()
		{
			_userManager = Helper.MockUserManager<Account>(new List<Account>());
			var options = new DbContextOptionsBuilder<ResultTrackerDbContext>().UseInMemoryDatabase(databaseName: "fakeDatabase").Options;
			_context = new ResultTrackerDbContext(options);
			_context.RemoveRange(_context.Accounts);
			_context.RemoveRange(_context.Topics);
			_context.RemoveRange(_context.Subjects);
			_context.RemoveRange(_context.Results);
			_context.Topics.Add(Helper.CreateFakeTopic(Guid.Parse("abd44265-5a60-4ae1-a17d-845e64adcf86")));
			_context.Subjects.Add(Helper.CreateFakeSubject(Guid.Parse("3528b040-dd35-493e-baa4-0bbb00341f97")));
			_context.SaveChanges();
			_context.Results.AddRange(new List<TestResult>()
			{
				Helper.CreateFakeTestResult("1",Guid.Parse("dabd4d63-cea5-4fec-9e1c-764b8efc3f96"),Guid.Parse("abd44265-5a60-4ae1-a17d-845e64adcf86"),Guid.Parse("3528b040-dd35-493e-baa4-0bbb00341f97"))
			});
			_context.SaveChanges();
		}
		[Test]
		[Category("Instantiation")]
		public void SUTInstantiatedCorrectly()
		{
			_sut = new SQLTestResultRepository(_context);
			Assert.That(_sut, Is.InstanceOf<SQLTestResultRepository>());
		}
		[Test]
		[Category("CreateTestResult/Happy")]
		public async Task WhenTestResultAdded_ThenCreateAsyncReturnsResult()
		{
			_sut = new SQLTestResultRepository(_context);
			var fakeTestResult = Helper.CreateFakeTestResult("2",Guid.Parse("26816fed-b02e-4e6c-89b9-a895b0af884f"), Guid.Parse("abd44265-5a60-4ae1-a17d-845e64adcf86"), Guid.Parse("3528b040-dd35-493e-baa4-0bbb00341f97"));
			var returnedResult = await _sut.CreateAsync(fakeTestResult);
			Assert.That(returnedResult, Is.TypeOf<TestResult>());
			Assert.That(returnedResult.Id, Is.EqualTo(Guid.Parse("26816fed-b02e-4e6c-89b9-a895b0af884f")));
		}
		[Test]
		[Category("DeleteTestResult/Happy")]
		public async Task WhenValidTestResultDelete_ThenDeleteAsyncReturnsTestResult()
		{
			_sut = new SQLTestResultRepository(_context);
			var deletedTestResult = await _sut.DeleteAsync(Guid.Parse("dabd4d63-cea5-4fec-9e1c-764b8efc3f96"));
			Assert.That(deletedTestResult.Id, Is.EqualTo(Guid.Parse("dabd4d63-cea5-4fec-9e1c-764b8efc3f96")));
			Assert.That(deletedTestResult.Notes, Is.EqualTo("Test Notes"));
		}
		[Test]
		[Category("DeleteTestResult/Sad")]
		public async Task WhenInvalidTestResultDelete_ThenDeleteAsyncReturnsNull()
		{
			_sut = new SQLTestResultRepository(_context);
			var deletedTestResult = await _sut.DeleteAsync(Guid.Parse("dabd4d63-cea5-4fec-9e1c-764b8efc3f97"));
			Assert.That(deletedTestResult,Is.Null);
		}
		[Test]
		[Category("GetAllTestResult/Happy")]
		public async Task WhenGetAll_ReturnsListOfTestResults()
		{
			_sut = new SQLTestResultRepository(_context);
			var testResultsReturned = await _sut.GetAllAsync();
			Assert.That(testResultsReturned, Is.TypeOf<List<TestResult>>());
			Assert.That(testResultsReturned.ToList().Count, Is.EqualTo(_context.Results.ToList().Count));
		}
		//Need to still add further tests for GetAll paths. 
		[Test]
		[Category("GetByIdTestResult/Happy")]
		public async Task WhenGetById_ReturnsCorrectTestResult()
		{
			_sut = new SQLTestResultRepository(_context);
			var testResultReturned = await _sut.GetByIdAsync(Guid.Parse("dabd4d63-cea5-4fec-9e1c-764b8efc3f96"));
			Assert.That(testResultReturned, Is.TypeOf<TestResult>());
			Assert.That(testResultReturned.PercentageResult, Is.EqualTo(100));
			Assert.That(testResultReturned.Notes, Is.EqualTo("Test Notes"));
			Assert.That(testResultReturned.Topic.Name, Is.EqualTo("Test Topic"));
			Assert.That(testResultReturned.Subject.Name, Is.EqualTo("Test Subject"));
		}
		[Test]
		[Category("GetByIdTestResult/Sad")]
		public async Task WhenGetByInvalidId_ReturnsNull()
		{
			_sut = new SQLTestResultRepository(_context);
			var testResultReturned = await _sut.GetByIdAsync(Guid.Parse("dabd4d63-cea5-4fec-9e1c-764b8efc3f97"));
			Assert.That(testResultReturned, Is.Null);
		}
		[Test]
		[Category("UpdateTestResult/Happy")]
		public async Task WhenUpdateValid_ReturnsCorrectUpdateTestResult()
		{
			_sut = new SQLTestResultRepository(_context);
			var resultUpdate = new TestResult() {
				Id = Guid.Parse("dabd4d63-cea5-4fec-9e1c-764b8efc3f96"),
				Notes = "Test Update",
				PercentageResult = 100,
				Student = Helper.CreateFakeAccount("1"),
				TopicId = Guid.Parse("abd44265-5a60-4ae1-a17d-845e64adcf86"),
				SubjectId = Guid.Parse("3528b040-dd35-493e-baa4-0bbb00341f97")
			};
			var testResultToUpdate = await _sut.UpdateAsync(Guid.Parse("dabd4d63-cea5-4fec-9e1c-764b8efc3f96"), resultUpdate);
			Assert.That(testResultToUpdate.Notes, Is.EqualTo("Test Update"));
		}
		[Test]
		[Category("UpdateTestResult/Sad")]
		public async Task WhenUpdateInvalid_ReturnsNull()
		{
			_sut = new SQLTestResultRepository(_context);
			var resultUpdate = new TestResult()
			{
				Id = Guid.Parse("dabd4d63-cea5-4fec-9e1c-764b8efc3f96"),
				Notes = "Test Update",
				PercentageResult = 100,
				Student = Helper.CreateFakeAccount("1"),
				TopicId = Guid.Parse("abd44265-5a60-4ae1-a17d-845e64adcf86"),
				SubjectId = Guid.Parse("3528b040-dd35-493e-baa4-0bbb00341f97")
			};
			var testResultToUpdate = await _sut.UpdateAsync(Guid.Parse("dabd4d63-cea5-4fec-9e1c-764b8efc3f97"), resultUpdate);
			Assert.That(testResultToUpdate, Is.Null);
		}

	}
}
