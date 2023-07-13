using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResultTracker.API.Models.Dto;
using ResultTracker.API.Repositories.Interfaces;
using ResultTracker.API.Users;
using System.Data;
using System.Security.Claims;

namespace ResultTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Teacher, Admin")]
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
        [Authorize(Roles ="Admin,Teacher")]
        public async Task<IActionResult> GetAllAccounts()
        {
            var accounts = await _repository.GetAllAsync();
            return Ok(accounts.Select(t => _mapper.Map<AccountDto>(t)));
        }
    }
}
