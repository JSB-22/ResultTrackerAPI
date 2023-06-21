using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using ResultTracker.API.Data;
using ResultTracker.API.Models.Domain;
using ResultTracker.API.Repositories;
using ResultTracker.API.Users.Domain;
using System.Diagnostics;

namespace APITests
{
	public class SQLTopicRepositoryTests
	{
		private SQLTopicRepository? _sut;
		private ResultTrackerDbContext _context;

		[SetUp]
		public void Setup()
		{
			var options = new DbContextOptionsBuilder<ResultTrackerDbContext>().UseInMemoryDatabase(databaseName: "fakeDatabase").Options;
			_context = new ResultTrackerDbContext(options);
			_context.RemoveRange(_context.Accounts);
			_context.RemoveRange(_context.Topics);
			_context.RemoveRange(_context.Subjects);
			_context.RemoveRange(_context.Results);
			_context.Topics.AddRange(new List<Topic>()
			{
				new Topic(){ Name = "Test Topic",Year = "4",Id= Guid.Parse("50348f5a-b7e9-4337-82bc-e2a4f72da120")}
			});
			_context.SaveChanges();
		}

		[Test]
		[Category("Instantiation")]
		public void SUTInstantiatedCorrectly()
		{
			_sut = new SQLTopicRepository(_context);
			Assert.That(_sut, Is.InstanceOf<SQLTopicRepository>());
		}
		[Test]
		[Category("Create/Happy")]
		public async Task  WhenTopicCreated_ThenTopicCountIncreases()
		{
			_sut = new SQLTopicRepository(_context);
			var beforeCount = _context.Topics.ToList().Count;
			var topicToAdd = new Topic() { Name = "Test Topic 2", Year = "4"};
			await _sut.CreateTopicAsync(topicToAdd);
			Assert.That(_context.Topics.ToList().Count, Is.EqualTo(beforeCount+1));
		}
		[Test]
		[Category("Create/Sad")]
		public void WhenTopicNotCreated_ThenException()
		{
			_sut = new SQLTopicRepository(_context);
			var invalidTopicToAdd = new Topic();
			Assert.That(()=>_sut.CreateTopicAsync(invalidTopicToAdd),Throws.TypeOf<DbUpdateException>());
		}
		[Test]
		[Category("Delete/Happy")]
		public async Task WhenTopicDeleted_ThenTopicCountDecreases()
		{
			_sut = new SQLTopicRepository(_context);
			var beforeCount = _context.Topics.ToList().Count;
			var id = Guid.Parse("50348f5a-b7e9-4337-82bc-e2a4f72da120");
			await _sut.DeleteTopicAsync(id);
			Assert.That(_context.Topics.ToList().Count, Is.EqualTo(beforeCount-1));
		}
		[Test]
		[Category("Delete/Happy")]
		public async Task WhenTopicDeleted_ThenTopicReturnedIsCorrect()
		{
			_sut = new SQLTopicRepository(_context);
			var id = Guid.Parse("50348f5a-b7e9-4337-82bc-e2a4f72da120");
			var topicReturned = await _sut.DeleteTopicAsync(id);
			Assert.That(topicReturned.Id, Is.EqualTo(id));
			Assert.That(topicReturned.Name, Is.EqualTo("Test Topic"));
			Assert.That(topicReturned.Year, Is.EqualTo("4"));
		}
		[Test]
		[Category("Delete/Sad")]
		public async Task WhenInvalidTopicDeleted_ThenTopicReturnedNull()
		{
			_sut = new SQLTopicRepository(_context);
			var id = Guid.Parse("43ca7d16-5883-4c2d-8b17-8763b3ae28e5");
			var topicReturned = await _sut.DeleteTopicAsync(id);
			Assert.That(topicReturned,Is.Null);
		}
		[Test]
		[Category("GetAllAsync/Happy")]
		public async Task WhenGetAll_ReturnListTopics()
		{
			_sut = new SQLTopicRepository(_context);
			var topics = await _sut.GetAllAsync();
			Assert.That(topics, Is.InstanceOf<List<Topic>>());
			Assert.That(topics.Count, Is.EqualTo(_context.Topics.ToList().Count));
		}
		[Test]
		[Category("GetByIdAsync/Happy")]
		public async Task WhenGetByIdAsync_ReturnTopic()
		{
			_sut = new SQLTopicRepository(_context);
			var id = Guid.Parse("50348f5a-b7e9-4337-82bc-e2a4f72da120");
			var topic = await _sut.GetByIdAsync(id);
			Assert.That(topic.Name, Is.EqualTo("Test Topic"));
			Assert.That(topic.Year, Is.EqualTo("4"));
		}
		[Test]
		[Category("GetByIdAsync/Sad")]
		public async Task WhenInvalidIdGetByIdAsync_ReturnNull()
		{
			_sut = new SQLTopicRepository(_context);
			var id = Guid.Parse("50348f5a-b7e9-4337-82bc-e2a4f72da121");
			var topic = await _sut.GetByIdAsync(id);
			Assert.That(topic,Is.Null);
		}
		[Test]
		[Category("UpdateTopicAsync/Happy")]
		public async Task WhenUpdateCalledCorrectly_TopicUpdated()
		{
			_sut = new SQLTopicRepository(_context);
			var id = Guid.Parse("50348f5a-b7e9-4337-82bc-e2a4f72da120");
			var updateToTopic = new Topic() { Name = "Update", Year = "3" };
			var returnedTopic = await _sut.UpdateTopicAsync(id, updateToTopic);
			Assert.That(returnedTopic.Name, Is.EqualTo("Update"));
			Assert.That(returnedTopic.Year, Is.EqualTo("3"));
		}
		[Test]
		[Category("UpdateTopicAsync/Sad")]
		public async Task WhenUpdateCalledInCorrectly_TopicUpdateFailed()
		{
			_sut = new SQLTopicRepository(_context);
			var id = Guid.Parse("50348f5a-b7e9-4337-82bc-e2a4f72da121");
			var updateToTopic = new Topic() { Name = "Update", Year = "3" };
			var returnedTopic = await _sut.UpdateTopicAsync(id, updateToTopic);
			Assert.That(returnedTopic, Is.Null);
		}
	}
}