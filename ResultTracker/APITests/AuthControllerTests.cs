using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NuGet.Frameworks;
using ResultTracker.API.Controllers;
using ResultTracker.API.Models.Dto;
using ResultTracker.API.Repositories;
using ResultTracker.API.Repositories.Interfaces;
using ResultTracker.API.Users.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace APITests
{
	public class AuthControllerTests
	{
		private AuthController _sut;

		[Test]
		[Category("Instantiation")]
		public void SUTInstantiatedCorrectly()
		{
			var _userManager = Helper.MockUserManager<Account>(new List<Account>());
			var _tokenRepository = new Mock<ITokenRepository>();
			_sut = new AuthController(_userManager.Object,_tokenRepository.Object);
			Assert.That(_sut, Is.InstanceOf<AuthController>());
		}
		[Test]
		[Category("Register/Happy")]
		public async Task WhenValidRegisterRequest_AuthController_ReturnsOK()
		{
			var _userManager = Helper.MockUserManager<Account>(new List<Account>());
			var _tokenRepository = new Mock<ITokenRepository>();
			_sut = new AuthController(_userManager.Object, _tokenRepository.Object);
			var validRequest = new RegisterRequestDto()
			{
				FullName = "Test",
				Roles = new string[] { "Test Role" },
				Username = "Email@Email.Com"
			};
			var result = await _sut.Register(validRequest);
			Assert.That(result, Is.TypeOf<OkObjectResult>());
		}
		[Test]
		[Category("Register/Sad")]
		public async Task WhenValidRegisterRequestNoRoles_AuthController_ReturnsBadRequest()
		{
			var _userManager = Helper.MockUserManager<Account>(new List<Account>());
			var _tokenRepository = new Mock<ITokenRepository>();
			_sut = new AuthController(_userManager.Object, _tokenRepository.Object);
			var validRequest = new RegisterRequestDto()
			{
				FullName = "Test",
				Username = "Email@Email.Com"
			};
			var result = await _sut.Register(validRequest);
			Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
		}
		[Test]
		[Category("Register/Sad")]
		public async Task WhenValidRegisterRequest_UserManagerSuceededFalse_ReturnsBadRequest()
		{
			var _userManager = Helper.MockUserManager<Account>(new List<Account>());
			_userManager.Setup(x => x.AddToRolesAsync(It.IsAny<Account>(), It.IsAny<IEnumerable<string>>())).ReturnsAsync(IdentityResult.Failed());


			var _tokenRepository = new Mock<ITokenRepository>();
			_sut = new AuthController(_userManager.Object, _tokenRepository.Object);
			var validRequest = new RegisterRequestDto()
			{
				FullName = "Test",
				Roles = new string[] { "Test role" },
				Username = "Email@Email.Com"
			};
			var result = await _sut.Register(validRequest);
			Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
		}
		//Login: 

		[Test]
		[Category("Login/Happy")]
		public async Task WhenValidLoginRequest_AuthController_ReturnsOk()
		{
			var fakeAccount = new Account() { FullName = "test" };
			var fakeRoles = new string[]{"test roles" };
			var _userManager = Helper.MockUserManager<Account>(new List<Account>());

			_userManager.Setup(t => t.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(fakeAccount);
			_userManager.Setup(t => t.CheckPasswordAsync(It.IsAny<Account>(), It.IsAny<string>())).ReturnsAsync(true);
			_userManager.Setup(t => t.GetRolesAsync(It.IsAny<Account>())).ReturnsAsync(fakeRoles);

			var _tokenRepository = new Mock<ITokenRepository>();
			_sut = new AuthController(_userManager.Object, _tokenRepository.Object);
			var validRequest = new LoginRequestDto()
			{
				Password = "Password",
				Username = "Email@Email.com"
			};
			var result = await _sut.Login(validRequest);
			Assert.That(result, Is.TypeOf<OkObjectResult>());
		}
		[Test]
		[Category("Login/Sad")]
		public async Task WhenInvalidLoginRequest_AuthController_ReturnsBadRequest()
		{
			var _userManager = Helper.MockUserManager<Account>(new List<Account>());

			Account? nullAccount = null;
			_userManager.Setup(t => t.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(nullAccount);

			var _tokenRepository = new Mock<ITokenRepository>();
			_sut = new AuthController(_userManager.Object, _tokenRepository.Object);
			var validRequest = new LoginRequestDto()
			{
				Password = "Password",
				Username = "Email@Email.com"
			};
			var result = await _sut.Login(validRequest);
			Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

			var badResult = result as BadRequestObjectResult;
			string strJson = JsonSerializer.Serialize<BadRequestObjectResult>(badResult);
			Assert.That(strJson,Does.Contain("Username or Password Incorrect"));
		}
		[Test]
		[Category("Login/Sad")]
		public async Task WhenInvalidLoginRequest_PasswordIncorrect_AuthController_ReturnsBadRequest()
		{
			var fakeAccount = new Account() { FullName = "test" };
			var fakeRoles = new string[] { "test roles" };
			var _userManager = Helper.MockUserManager<Account>(new List<Account>());

			_userManager.Setup(t => t.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(fakeAccount);
			_userManager.Setup(t => t.CheckPasswordAsync(It.IsAny<Account>(), It.IsAny<string>())).ReturnsAsync(true);

			var _tokenRepository = new Mock<ITokenRepository>();
			_sut = new AuthController(_userManager.Object, _tokenRepository.Object);

			var validRequest = new LoginRequestDto()
			{
				Password = "Password",
				Username = "Email@Email.com"
			};

			var result = await _sut.Login(validRequest);
			Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

			var badResult = result as BadRequestObjectResult;
			string strJson = JsonSerializer.Serialize<BadRequestObjectResult>(badResult);
			Assert.That(strJson, Does.Contain("Incorrect Password"));
		}

	}
}
