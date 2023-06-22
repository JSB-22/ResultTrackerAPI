using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ResultTracker.API.Controllers;
using ResultTracker.API.Models.Domain;
using ResultTracker.API.Models.Dto;
using ResultTracker.API.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITests
{
	public class SubjectControllerTests
	{
		private SubjectsController? _sut;
		private Subject fakeSubject;
		private SubjectDto fakeSubjectDto;

		[SetUp]
		public void Initalise()
		{
			fakeSubject = Helper.CreateFakeSubject(Guid.Parse("26816fed-b02e-4e6c-89b9-a895b0af884f"));
			fakeSubjectDto = new SubjectDto() { Id = Guid.Parse("26816fed-b02e-4e6c-89b9-a895b0af884f"), Name = "Test SubjectDto" };
		}

		[Test]
		[Category("Instantiation")]
		public void BeAbleToBeInstantiated()
		{
			var mockRepository = new Mock<ISubjectRepository>();
			var mockMapper = new Mock<IMapper>();
			_sut = new SubjectsController(mockRepository.Object, mockMapper.Object);
			Assert.That(_sut, Is.InstanceOf<SubjectsController>());
		}

		[Test]
		[Category("GetAllSubjects")]
		public void GetAllSubjects_WithSuccessfulResponse_ReturnsOK()
		{
			var mockRepository = new Mock<ISubjectRepository>();
			var mockMapper = new Mock<IMapper>();
			mockRepository.Setup(s => s.GetAllAsync().Result).Returns(Mock.Of<List<Subject>>());
			mockMapper.Setup(s => s.Map<SubjectDto>(It.IsAny<Subject>())).Returns(It.IsAny<SubjectDto>());
			_sut = new SubjectsController(mockRepository.Object, mockMapper.Object);


			var result = _sut.GetAllSubjects().Result;

			Assert.That(result, Is.InstanceOf<OkObjectResult>());
		}
		[Test]
		[Category("GetById/Happy")]
		public void GetById_WithSuccessfulResponse_ReturnsOK()
		{
			var mockRepository = new Mock<ISubjectRepository>();
			var mockMapper = new Mock<IMapper>();
			mockRepository.Setup(s => s.GetByIdAsync(It.IsAny<Guid>()).Result).Returns(fakeSubject);
			mockMapper.Setup(s => s.Map<SubjectDto>(It.IsAny<Subject>())).Returns(It.IsAny<SubjectDto>());
			_sut = new SubjectsController(mockRepository.Object, mockMapper.Object);


			var result = _sut.GetById(It.IsAny<Guid>()).Result;

			Assert.That(result, Is.InstanceOf<OkObjectResult>());
		}
		[Test]
		[Category("GetById/Sad")]
		public void GetById_WithUnSuccessfulResponse_ReturnsNotFound()
		{
			var mockRepository = new Mock<ISubjectRepository>();
			var mockMapper = new Mock<IMapper>();
			Subject nullSubject = null;
			mockRepository.Setup(s => s.GetByIdAsync(It.IsAny<Guid>()).Result).Returns(nullSubject);
			mockMapper.Setup(s => s.Map<SubjectDto>(It.IsAny<Subject>())).Returns(It.IsAny<SubjectDto>());
			_sut = new SubjectsController(mockRepository.Object, mockMapper.Object);


			var result = _sut.GetById(It.IsAny<Guid>()).Result;

			Assert.That(result, Is.InstanceOf<NotFoundResult>());
		}
		[Test]
		[Category("Delete/Happy")]
		public void Delete_WithSuccessfulResponse_ReturnsOK()
		{
			var mockRepository = new Mock<ISubjectRepository>();
			var mockMapper = new Mock<IMapper>();
			mockRepository.Setup(s => s.DeleteSubjectAsync(It.IsAny<Guid>()).Result).Returns(fakeSubject);
			mockMapper.Setup(s => s.Map<SubjectDto>(It.IsAny<Subject>())).Returns(It.IsAny<SubjectDto>());
			_sut = new SubjectsController(mockRepository.Object, mockMapper.Object);


			var result = _sut.Delete(It.IsAny<Guid>()).Result;

			Assert.That(result, Is.InstanceOf<OkObjectResult>());
		}
		[Test]
		[Category("Delete/Sad")]
		public void Delete_WithUnSuccessfulResponse_ReturnsNotFound()
		{
			var mockRepository = new Mock<ISubjectRepository>();
			var mockMapper = new Mock<IMapper>();
			Subject nullSubject = null;
			mockRepository.Setup(s => s.DeleteSubjectAsync(It.IsAny<Guid>()).Result).Returns(nullSubject);
			mockMapper.Setup(s => s.Map<SubjectDto>(It.IsAny<Subject>())).Returns(It.IsAny<SubjectDto>());
			_sut = new SubjectsController(mockRepository.Object, mockMapper.Object);


			var result = _sut.Delete(It.IsAny<Guid>()).Result;

			Assert.That(result, Is.InstanceOf<ObjectResult>());

			var objResult = result as ObjectResult;
			Assert.That((int)objResult.StatusCode, Is.EqualTo(500));
		}
		[Test]
		[Category("Create/Happy")]
		public void Create_WithSuccessfulResponse_ReturnsOK()
		{
			var mockRepository = new Mock<ISubjectRepository>();
			var mockMapper = new Mock<IMapper>();
			mockRepository.Setup(s => s.CreateSubjectAsync(It.IsAny<Subject>()).Result).Returns(fakeSubject);
			mockMapper.Setup(s => s.Map<SubjectDto>(It.IsAny<Subject>())).Returns(fakeSubjectDto);
			mockMapper.Setup(s => s.Map<Subject>(It.IsAny<AddSubjectRequestDto>())).Returns(It.IsAny<Subject>());
			_sut = new SubjectsController(mockRepository.Object, mockMapper.Object);


			var result = _sut.Create(It.IsAny<AddSubjectRequestDto>()).Result;

			Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
		}
		[Test]
		[Category("Update/Happy")]
		public void Update_WithSuccessfulResponse_ReturnsOK()
		{
			var mockRepository = new Mock<ISubjectRepository>();
			var mockMapper = new Mock<IMapper>();
			mockRepository.Setup(s => s.UpdateSubjectAsync(It.IsAny<Guid>(), It.IsAny<Subject>()).Result).Returns(fakeSubject);
			mockMapper.Setup(s => s.Map<SubjectDto>(It.IsAny<Subject>())).Returns(fakeSubjectDto);
			_sut = new SubjectsController(mockRepository.Object, mockMapper.Object);


			var result = _sut.Update(It.IsAny<Guid>(), It.IsAny<UpdateSubjectRequestDto>()).Result;

			Assert.That(result, Is.InstanceOf<OkObjectResult>());
		}
		[Test]
		[Category("Update/Sad")]
		public void Update_WithUnSuccessfulResponse_ReturnsNotFound()
		{
			var mockRepository = new Mock<ISubjectRepository>();
			var mockMapper = new Mock<IMapper>();
			Subject nullSubject = null;
			mockRepository.Setup(s => s.UpdateSubjectAsync(It.IsAny<Guid>(), It.IsAny<Subject>()).Result).Returns(nullSubject);
			mockMapper.Setup(s => s.Map<SubjectDto>(It.IsAny<Subject>())).Returns(It.IsAny<SubjectDto>());
			_sut = new SubjectsController(mockRepository.Object, mockMapper.Object);


			var result = _sut.Update(It.IsAny<Guid>(), It.IsAny<UpdateSubjectRequestDto>()).Result;

			Assert.That(result, Is.InstanceOf<NotFoundResult>());
		}
	}
}
