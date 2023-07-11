using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResultTracker.API.Models.Dto;
using ResultTracker.API.Repositories.Interfaces;
using ResultTracker.API.Users;

namespace ResultTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepository _repository;
        private readonly IMapper _mapper;
        public AccountsController(IAccountRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAccounts()
        {
            var subjects = await _repository.GetAllAsync();
            return Ok(subjects.Select(t => _mapper.Map<AccountDto>(t)));
        }
    }
}
