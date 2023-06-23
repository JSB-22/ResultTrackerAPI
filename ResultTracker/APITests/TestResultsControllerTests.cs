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
	public class TestResultsControllerTests
	{
		private TestResultsController _sut;
		private TestResult fakeResult;
		private TestResultDto fakeResultDto;

		[SetUp]
		public void Setup()
		{

			fakeResult = Helper.CreateFakeTestResult("1",Guid.Parse("26816fed-b02e-4e6c-89b9-a895b0af884f"), Guid.Parse("26816fed-b02e-4e6c-89b9-a895b0af884f"), Guid.Parse("26816fed-b02e-4e6c-89b9-a895b0af884f"));
			fakeResultDto = new TestResultDto() { Notes = "TestDto", Id = Guid.Parse("26816fed-b02e-4e6c-89b9-a895b0af884f"), PercentageResult = 100 };
		}


		[Test]
		[Category("Instantiation")]
		public void BeAbleToBeInstantiated()
		{
			var mockRepository = new Mock<ITestResultRepository>();
			var mockMapper = new Mock<IMapper>();
			_sut = new TestResultsController(mockMapper.Object,mockRepository.Object);
			Assert.That(_sut, Is.InstanceOf<TestResultsController>());
		}
		[Test]
		[Category("Create")]
		public async Task WhenCreate_ThenController_ReturnsOk()
		{
			var mockRepository = new Mock<ITestResultRepository>();
			mockRepository.Setup(t => t.CreateAsync(It.IsAny<TestResult>())).ReturnsAsync(fakeResult);

			var mockMapper = new Mock<IMapper>();
			mockMapper.Setup(t => t.Map<TestResult>(It.IsAny<AddTestResultRequestDto>())).Returns(fakeResult);
			mockMapper.Setup(t => t.Map<TestResultDto>(It.IsAny<TestResult>())).Returns(fakeResultDto);

			_sut = new TestResultsController(mockMapper.Object, mockRepository.Object);

			var result = await _sut.CreateResult(It.IsAny<AddTestResultRequestDto>());
			Assert.That(result, Is.InstanceOf<OkObjectResult>());
		}
		[Test]
		[Category("GetAll")]
		public async Task WhenGetAll_ReturnsOk()
		{
			var mockRepository = new Mock<ITestResultRepository>();
			mockRepository.Setup(t => t.GetAllAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),It.IsAny<bool>(),It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new List<TestResult>() {fakeResult});

			var mockMapper = new Mock<IMapper>();
			mockMapper.Setup(t => t.Map<TestResultDto>(It.IsAny<TestResult>())).Returns(fakeResultDto);

			_sut = new TestResultsController(mockMapper.Object, mockRepository.Object);

			var result = await _sut.GetAllTestResults(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>());
			Assert.That(result, Is.InstanceOf<OkObjectResult>());
		}
		[Test]
		[Category("GetById/Happy")]
		public async Task WhenGetByIdWithValidId_Then_ReturnsOk()
		{
			var mockRepository = new Mock<ITestResultRepository>();
			mockRepository.Setup(t => t.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(fakeResult);

			var mockMapper = new Mock<IMapper>();
			mockMapper.Setup(t => t.Map<TestResultDto>(It.IsAny<TestResult>())).Returns(fakeResultDto);

			_sut = new TestResultsController(mockMapper.Object, mockRepository.Object);

			var result = await _sut.GetById(It.IsAny<Guid>());
			Assert.That(result, Is.InstanceOf<OkObjectResult>());
		}
		[Test]
		[Category("GetById/Sad")]
		public async Task WhenGetByIdWithInvalidId_Then_ReturnsNotFound()
		{
			var mockRepository = new Mock<ITestResultRepository>();
			TestResult? nullResult = null;
			mockRepository.Setup(t => t.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(nullResult);

			var mockMapper = new Mock<IMapper>();
			mockMapper.Setup(t => t.Map<TestResultDto>(It.IsAny<TestResult>())).Returns(fakeResultDto);

			_sut = new TestResultsController(mockMapper.Object, mockRepository.Object);

			var result = await _sut.GetById(It.IsAny<Guid>());
			Assert.That(result, Is.InstanceOf<NotFoundResult>());
		}
		[Test]
		[Category("Update/Happy")]
		public async Task WhenUpdateWithValidId_Then_ReturnsOk()
		{
			var mockRepository = new Mock<ITestResultRepository>();
			mockRepository.Setup(t => t.UpdateAsync(It.IsAny<Guid>(),It.IsAny<TestResult>())).ReturnsAsync(fakeResult);

			var mockMapper = new Mock<IMapper>();
			mockMapper.Setup(t => t.Map<TestResultDto>(It.IsAny<TestResult>())).Returns(fakeResultDto);
			mockMapper.Setup(t => t.Map<TestResult>(It.IsAny<UpdateTestResultRequestDto>())).Returns(fakeResult);

			_sut = new TestResultsController(mockMapper.Object, mockRepository.Object);

			var result = await _sut.Update(It.IsAny<Guid>(), It.IsAny<UpdateTestResultRequestDto>());
			Assert.That(result, Is.InstanceOf<OkObjectResult>());
		}
		[Test]
		[Category("Update/Sad")]
		public async Task WhenUpdateWithInvalidId_Then_ReturnsOk()
		{
			var mockRepository = new Mock<ITestResultRepository>();
			TestResult? nullResult = null;
			mockRepository.Setup(t => t.UpdateAsync(It.IsAny<Guid>(), It.IsAny<TestResult>())).ReturnsAsync(nullResult);

			var mockMapper = new Mock<IMapper>();
			mockMapper.Setup(t => t.Map<TestResultDto>(It.IsAny<TestResult>())).Returns(fakeResultDto);
			mockMapper.Setup(t => t.Map<TestResult>(It.IsAny<UpdateTestResultRequestDto>())).Returns(fakeResult);

			_sut = new TestResultsController(mockMapper.Object, mockRepository.Object);

			var result = await _sut.Update(It.IsAny<Guid>(), It.IsAny<UpdateTestResultRequestDto>());
			Assert.That(result, Is.InstanceOf<NotFoundResult>());
		}
		[Test]
		[Category("Delete/Happy")]
		public async Task WhenDeleteWithValidId_Then_ReturnsOk()
		{
			var mockRepository = new Mock<ITestResultRepository>();
			mockRepository.Setup(t => t.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(fakeResult);

			var mockMapper = new Mock<IMapper>();
			mockMapper.Setup(t => t.Map<TestResultDto>(It.IsAny<TestResult>())).Returns(fakeResultDto);

			_sut = new TestResultsController(mockMapper.Object, mockRepository.Object);

			var result = await _sut.Delete(It.IsAny<Guid>());
			Assert.That(result, Is.InstanceOf<OkObjectResult>());
		}
		[Test]
		[Category("Delete/Happy")]
		public async Task WhenDeleteWithInvalidId_Then_ReturnsOk()
		{
			var mockRepository = new Mock<ITestResultRepository>();
			TestResult? nullResult = null;
			mockRepository.Setup(t => t.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(nullResult);

			var mockMapper = new Mock<IMapper>();
			mockMapper.Setup(t => t.Map<TestResultDto>(It.IsAny<TestResult>())).Returns(fakeResultDto);

			_sut = new TestResultsController(mockMapper.Object, mockRepository.Object);

			var result = await _sut.Delete(It.IsAny<Guid>());
			Assert.That(result, Is.InstanceOf<NotFoundResult>());
		}
	}
}


