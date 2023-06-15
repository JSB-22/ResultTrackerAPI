using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResultTracker.API.Models.Domain;
using ResultTracker.API.Models.Dto;
using ResultTracker.API.Repositories.Interfaces;

namespace ResultTracker.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Teacher, Admin")]
	public class SubjectsController : ControllerBase
	{
		private readonly ISubjectRepository _repository;
		private readonly IMapper _mapper;
		public SubjectsController(ISubjectRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}
		[HttpGet]
		public async Task<IActionResult> GetAllSubjects()
		{
			var subjects = await _repository.GetAllAsync();
			return Ok(subjects.Select(t => _mapper.Map<SubjectDto>(t)));
		}
		[HttpGet]
		[Route("{id:Guid}")]
		public async Task<IActionResult> GetById(Guid id)
		{
			var subject = await _repository.GetByIdAsync(id);
			if (subject == null) return NotFound();
			return Ok(_mapper.Map<SubjectDto>(subject));
		}
		[HttpDelete]
		[Route("{id:Guid}")]
		public async Task<IActionResult> Delete(Guid id)
		{
			var subjectToDelete = await _repository.DeleteSubjectAsync(id);
			if (subjectToDelete == null) return Problem($"No subject matching the id: {id}");
			return Ok(subjectToDelete);
		}
		[HttpPost]
		public async Task<IActionResult> Create(AddSubjectRequestDto addSubjectRequestDto)
		{
			var domainModelSubject = _mapper.Map<Subject>(addSubjectRequestDto);
			var createdSubject = await _repository.CreateSubjectAsync(domainModelSubject);
			var createdSubjectDto = _mapper.Map<SubjectDto>(createdSubject);
			return CreatedAtAction(nameof(GetById), new { id = createdSubjectDto.Id }, createdSubjectDto);
		}
		[HttpPut]
		[Route("{id:Guid}")]
		public async Task<IActionResult> Update(Guid id, UpdateSubjectRequestDto updateSubjectRequestDto)
		{
			var existingSubject = await _repository.UpdateSubjectAsync(id, _mapper.Map<Subject>(updateSubjectRequestDto));
			if (existingSubject == null) return null;
			return Ok(_mapper.Map<SubjectDto>(existingSubject));
		}
	}
}
