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
	public class TopicsController : ControllerBase
	{
		private readonly ITopicRepository _repository;
		private readonly IMapper _mapper;
		public TopicsController(ITopicRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}
		[HttpGet]
		public async Task<IActionResult> GetAllTopics()
		{
			var topics = await _repository.GetAllAsync();
			return Ok(topics.Select(t=>_mapper.Map<TopicDto>(t)));
		}
		[HttpGet]
		[Route("{id:Guid}")]
		public async Task<IActionResult> GetById(Guid id)
		{
			var topic = await _repository.GetByIdAsync(id);
			if (topic == null) return NotFound();
			return Ok(_mapper.Map<TopicDto>(topic));
		}
		[HttpDelete]
		[Route("{id:Guid}")]
		public async Task<IActionResult> Delete(Guid id)
		{
			var topicToDelete = await _repository.DeleteTopicAsync(id);
			if (topicToDelete == null) return Problem($"No topic matching the id: {id}");
			return Ok(topicToDelete);
		}
		[HttpPost]
		public async Task<IActionResult> Create(AddTopicRequestDto addTopicRequestDto)
		{
			var domainModelTopic = _mapper.Map<Topic>(addTopicRequestDto);
			var createdTopic = await _repository.CreateTopicAsync(domainModelTopic);
			var createdTopicDto = _mapper.Map<TopicDto>(createdTopic);
			return CreatedAtAction(nameof(GetById), new { id = createdTopicDto.Id }, createdTopicDto);
		}
	}
}
