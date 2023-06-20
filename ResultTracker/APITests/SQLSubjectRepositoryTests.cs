using Microsoft.EntityFrameworkCore;
using ResultTracker.API.Data;
using ResultTracker.API.Models.Domain;
using ResultTracker.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITests
{
	public class SQLSubjectRepositoryTests
	{
		private SQLSubjectRepository? _sut;
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
			_context.Subjects.AddRange(new List<Subject>()
			{
				new Subject(){ Name = "Test Subject",ExamBoard = "TestBoard",Id= Guid.Parse("50348f5a-b7e9-4337-82bc-e2a4f72da120")}
			});
			_context.SaveChanges();
		}

		[Test]
		[Category("Instantiation")]
		public void SUTInstantiatedCorrectly()
		{
			_sut = new SQLSubjectRepository(_context);
			Assert.That(_sut, Is.InstanceOf<SQLSubjectRepository>());
		}
		[Test]
		[Category("Create/Happy")]
		public async Task WhenSubjectCreated_ThenSubjectCountIncreases()
		{
			_sut = new SQLSubjectRepository(_context);
			var subjectToAdd = new Subject() { Name = "Test Subject 2", ExamBoard = "TestBoard2" };
			await _sut.CreateSubjectAsync(subjectToAdd);
			Assert.That(_context.Subjects.ToList().Count, Is.EqualTo(2));
		}
		[Test]
		[Category("Create/Sad")]
		public void WhenSubjectNotCreated_ThenException()
		{
			_sut = new SQLSubjectRepository(_context);
			var invalidsubjectToAdd = new Subject();
			Assert.That(() => _sut.CreateSubjectAsync(invalidsubjectToAdd), Throws.TypeOf<DbUpdateException>());
		}
		[Test]
		[Category("Delete/Happy")]
		public async Task WhenSubjectDeleted_ThenSubjectCountDecreases()
		{
			_sut = new SQLSubjectRepository(_context);
			var id = Guid.Parse("50348f5a-b7e9-4337-82bc-e2a4f72da120");
			await _sut.DeleteSubjectAsync(id);
			Assert.That(_context.Subjects.ToList().Count, Is.EqualTo(0));
		}
		[Test]
		[Category("Delete/Happy")]
		public async Task WhenSubjectDeleted_ThenSubjectReturnedIsCorrect()
		{
			_sut = new SQLSubjectRepository(_context);
			var id = Guid.Parse("50348f5a-b7e9-4337-82bc-e2a4f72da120");
			var subjectReturned = await _sut.DeleteSubjectAsync(id);
			Assert.That(subjectReturned.Id, Is.EqualTo(id));
			Assert.That(subjectReturned.Name, Is.EqualTo("Test Subject"));
			Assert.That(subjectReturned.ExamBoard, Is.EqualTo("TestBoard"));
		}
		[Test]
		[Category("Delete/Sad")]
		public async Task WhenInvalidSubjectDeleted_ThenSubjectReturnedNull()
		{
			_sut = new SQLSubjectRepository(_context);
			var id = Guid.Parse("43ca7d16-5883-4c2d-8b17-8763b3ae28e5");
			var subjectReturned = await _sut.DeleteSubjectAsync(id);
			Assert.That(subjectReturned, Is.Null);
		}
		[Test]
		[Category("GetAllAsync/Happy")]
		public async Task WhenGetAll_ReturnListSubjects()
		{
			_sut = new SQLSubjectRepository(_context);
			var subjects = await _sut.GetAllAsync();
			Assert.That(subjects, Is.InstanceOf<List<Subject>>());
			Assert.That(subjects.Count, Is.EqualTo(1));
		}
		[Test]
		[Category("GetByIdAsync/Happy")]
		public async Task WhenGetByIdAsync_ReturnSubject()
		{
			_sut = new SQLSubjectRepository(_context);
			var id = Guid.Parse("50348f5a-b7e9-4337-82bc-e2a4f72da120");
			var subject = await _sut.GetByIdAsync(id);
			Assert.That(subject.Name, Is.EqualTo("Test Subject"));
			Assert.That(subject.ExamBoard, Is.EqualTo("TestBoard"));
		}
		[Test]
		[Category("GetByIdAsync/Sad")]
		public async Task WhenInvalidIdGetByIdAsync_ReturnNull()
		{
			_sut = new SQLSubjectRepository(_context);
			var id = Guid.Parse("50348f5a-b7e9-4337-82bc-e2a4f72da121");
			var subject = await _sut.GetByIdAsync(id);
			Assert.That(subject, Is.Null);
		}
		[Test]
		[Category("UpdatesubjectAsync/Happy")]
		public async Task WhenUpdateCalledCorrectly_SubjectUpdated()
		{
			_sut = new SQLSubjectRepository(_context);
			var id = Guid.Parse("50348f5a-b7e9-4337-82bc-e2a4f72da120");
			var updateToSubject = new Subject() { Name = "Update", ExamBoard = "Update" };
			var returnedSubject = await _sut.UpdateSubjectAsync(id, updateToSubject);
			Assert.That(returnedSubject.Name, Is.EqualTo("Update"));
			Assert.That(returnedSubject.ExamBoard, Is.EqualTo("Update"));
		}
		[Test]
		[Category("UpdatesubjectAsync/Sad")]
		public async Task WhenUpdateCalledInCorrectly_SubjectUpdateFailed()
		{
			_sut = new SQLSubjectRepository(_context);
			var id = Guid.Parse("50348f5a-b7e9-4337-82bc-e2a4f72da121");
			var updateToSubject = new Subject() { Name = "Update", ExamBoard = "Update" };
			var returnedSubject = await _sut.UpdateSubjectAsync(id, updateToSubject);
			Assert.That(returnedSubject, Is.Null);
		}
	}
}
