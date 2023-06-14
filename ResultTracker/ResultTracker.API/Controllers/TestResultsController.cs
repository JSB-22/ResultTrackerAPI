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
		[HttpGet]
		public async Task<IActionResult> GetAllTestResults(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
		{
			var testResultsDomaimModel = await _repository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);
			return Ok(testResultsDomaimModel.Select(tr => _mapper.Map<TestResultDto>(tr)));
		}
		[HttpGet]
		[Route("{id:Guid}")]
		public async Task<IActionResult> GetById(Guid id)
		{
			var testResultDomainModel = await _repository.GetByIdAsync(id);
			if (testResultDomainModel == null) return NotFound();
			return Ok(_mapper.Map<TestResultDto>(testResultDomainModel));
		}
		[HttpPut]
		[Route("{id:Guid}")]
		public async Task<IActionResult> Update(Guid id, UpdateTestResultRequestDto updateTestResultRequestDto)
		{
			var testResultDomainModel = _mapper.Map<TestResult>(updateTestResultRequestDto);
			testResultDomainModel = await _repository.UpdateAsync(id, testResultDomainModel);
			if (testResultDomainModel == null) return NotFound();
			return Ok(_mapper.Map<TestResultDto>(testResultDomainModel));
		}
		[HttpDelete]
		[Route("{id:Guid}")]
		public async Task<IActionResult> Delete(Guid id)
		{
			var testResultToDelete = await _repository.DeleteAsync(id);
			if (testResultToDelete == null) return NotFound();
			return Ok(_mapper.Map<TestResultDto>(testResultToDelete));
		}
	}
}
