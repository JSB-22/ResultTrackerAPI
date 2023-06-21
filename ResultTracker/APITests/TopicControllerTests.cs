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
	public class TopicControllerTests
	{
		private TopicsController? _sut;
		private Topic fakeTopic;
		private TopicDto fakeTopicDto;

		[SetUp]
		public void Initalise()
		{
			fakeTopic = Helper.CreateFakeTopic(Guid.Parse("26816fed-b02e-4e6c-89b9-a895b0af884f"));
			fakeTopicDto = new TopicDto() { Id = Guid.Parse("26816fed-b02e-4e6c-89b9-a895b0af884f"), Name = "Test TopicDto" };
		}

		[Test]
		[Category("Instantiation")]
		public void BeAbleToBeInstantiated()
		{
			var mockRepository = new Mock<ITopicRepository>();
			var mockMapper = new Mock<IMapper>();
			_sut = new TopicsController(mockRepository.Object,mockMapper.Object);
			Assert.That(_sut, Is.InstanceOf<TopicsController>());
		}

		[Test]
		[Category("GetAllTopics")]
		public void GetAllTopics_WithSuccessfulResponse_ReturnsOK()
		{
			var mockRepository = new Mock<ITopicRepository>();
			var mockMapper = new Mock<IMapper>();
			mockRepository.Setup(s => s.GetAllAsync().Result).Returns(Mock.Of<List<Topic>>());
			mockMapper.Setup(s => s.Map<TopicDto>(It.IsAny<Topic>())).Returns(It.IsAny<TopicDto>());
			_sut = new TopicsController(mockRepository.Object, mockMapper.Object);


			var result = _sut.GetAllTopics().Result;

			Assert.That(result, Is.InstanceOf<OkObjectResult>());
		}
		[Test]
		[Category("GetById/Happy")]
		public void GetById_WithSuccessfulResponse_ReturnsOK()
		{
			var mockRepository = new Mock<ITopicRepository>();
			var mockMapper = new Mock<IMapper>();
			mockRepository.Setup(s => s.GetByIdAsync(It.IsAny<Guid>()).Result).Returns(fakeTopic);
			mockMapper.Setup(s => s.Map<TopicDto>(It.IsAny<Topic>())).Returns(It.IsAny<TopicDto>());
			_sut = new TopicsController(mockRepository.Object, mockMapper.Object);


			var result = _sut.GetById(It.IsAny<Guid>()).Result;

			Assert.That(result, Is.InstanceOf<OkObjectResult>());
		}
		[Test]
		[Category("GetById/Sad")]
		public void GetById_WithUnSuccessfulResponse_ReturnsNotFound()
		{
			var mockRepository = new Mock<ITopicRepository>();
			var mockMapper = new Mock<IMapper>();
			Topic nullTopic = null;
			mockRepository.Setup(s => s.GetByIdAsync(It.IsAny<Guid>()).Result).Returns(nullTopic);
			mockMapper.Setup(s => s.Map<TopicDto>(It.IsAny<Topic>())).Returns(It.IsAny<TopicDto>());
			_sut = new TopicsController(mockRepository.Object, mockMapper.Object);


			var result = _sut.GetById(It.IsAny<Guid>()).Result;

			Assert.That(result, Is.InstanceOf<NotFoundResult>());
		}
		[Test]
		[Category("Delete/Happy")]
		public void Delete_WithSuccessfulResponse_ReturnsOK()
		{
			var mockRepository = new Mock<ITopicRepository>();
			var mockMapper = new Mock<IMapper>();
			mockRepository.Setup(s => s.DeleteTopicAsync(It.IsAny<Guid>()).Result).Returns(fakeTopic);
			mockMapper.Setup(s => s.Map<TopicDto>(It.IsAny<Topic>())).Returns(It.IsAny<TopicDto>());
			_sut = new TopicsController(mockRepository.Object, mockMapper.Object);


			var result = _sut.Delete(It.IsAny<Guid>()).Result;

			Assert.That(result, Is.InstanceOf<OkObjectResult>());
		}
		[Test]
		[Category("Delete/Sad")]
		public void Delete_WithUnSuccessfulResponse_ReturnsNotFound()
		{
			var mockRepository = new Mock<ITopicRepository>();
			var mockMapper = new Mock<IMapper>();
			Topic nullTopic = null;
			mockRepository.Setup(s => s.DeleteTopicAsync(It.IsAny<Guid>()).Result).Returns(nullTopic);
			mockMapper.Setup(s => s.Map<TopicDto>(It.IsAny<Topic>())).Returns(It.IsAny<TopicDto>());
			_sut = new TopicsController(mockRepository.Object, mockMapper.Object);


			var result = _sut.Delete(It.IsAny<Guid>()).Result;

			Assert.That(result, Is.InstanceOf<ObjectResult>());

			var objResult = result as ObjectResult;
			Assert.That((int)objResult.StatusCode, Is.EqualTo(500));
		}
		[Test]
		[Category("Create/Happy")]
		public void Create_WithSuccessfulResponse_ReturnsOK()
		{
			var mockRepository = new Mock<ITopicRepository>();
			var mockMapper = new Mock<IMapper>();
			mockRepository.Setup(s => s.CreateTopicAsync(It.IsAny<Topic>()).Result).Returns(fakeTopic);
			mockMapper.Setup(s => s.Map<TopicDto>(It.IsAny<Topic>())).Returns(fakeTopicDto);
			mockMapper.Setup(s => s.Map<Topic>(It.IsAny<AddTopicRequestDto>())).Returns(It.IsAny<Topic>());
			_sut = new TopicsController(mockRepository.Object, mockMapper.Object);


			var result = _sut.Create(It.IsAny<AddTopicRequestDto>()).Result;

			Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
		}
		[Test]
		[Category("Update/Happy")]
		public void Update_WithSuccessfulResponse_ReturnsOK()
		{
			var mockRepository = new Mock<ITopicRepository>();
			var mockMapper = new Mock<IMapper>();
			mockRepository.Setup(s => s.UpdateTopicAsync(It.IsAny<Guid>(),It.IsAny<Topic>()).Result).Returns(fakeTopic);
			mockMapper.Setup(s => s.Map<TopicDto>(It.IsAny<Topic>())).Returns(fakeTopicDto);
			_sut = new TopicsController(mockRepository.Object, mockMapper.Object);


			var result = _sut.Update(It.IsAny<Guid>(), It.IsAny<UpdateTopicRequestDto>()).Result;

			Assert.That(result, Is.InstanceOf<OkObjectResult>());
		}
		[Test]
		[Category("Update/Sad")]
		public void Update_WithUnSuccessfulResponse_ReturnsNotFound()
		{
			var mockRepository = new Mock<ITopicRepository>();
			var mockMapper = new Mock<IMapper>();
			Topic nullTopic = null;
			mockRepository.Setup(s => s.UpdateTopicAsync(It.IsAny<Guid>(),It.IsAny<Topic>()).Result).Returns(nullTopic);
			mockMapper.Setup(s => s.Map<TopicDto>(It.IsAny<Topic>())).Returns(It.IsAny<TopicDto>());
			_sut = new TopicsController(mockRepository.Object, mockMapper.Object);


			var result = _sut.Update(It.IsAny<Guid>(),It.IsAny<UpdateTopicRequestDto>()).Result;

			Assert.That(result, Is.InstanceOf<NotFoundResult>());
		}
	}
}
