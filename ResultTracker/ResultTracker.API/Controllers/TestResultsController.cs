using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResultTracker.API.Models.Domain;
using ResultTracker.API.Models.Dto;
using ResultTracker.API.Repositories.Interfaces;

namespace ResultTracker.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TestResultsController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly ITestResultRepository _repository;

		public TestResultsController(IMapper mapper, ITestResultRepository repository)
		{
			_mapper = mapper;
			_repository = repository;
		}

		[HttpPost]
		public async Task<IActionResult> CreateResult(AddTestResultRequestDto addTestResultRequestDto)
		{
			var domainModelTestResult = _mapper.Map<TestResult>(addTestResultRequestDto);
			var createdTestResult = await _repository.CreateAsync(domainModelTestResult);
			return Ok(_mapper.Map<TestResultDto>(createdTestResult));
		}
	}
}
