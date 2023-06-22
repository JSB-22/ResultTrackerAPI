using Microsoft.AspNetCore.Identity;
using Moq;
using ResultTracker.API.Models.Domain;
using ResultTracker.API.Users.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITests
{
	public class Helper
	{
		public static Mock<UserManager<TUser>> MockUserManager<TUser>(List<TUser> ls) where TUser : class
		{
			var store = new Mock<IUserStore<TUser>>();
			var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
			mgr.Object.UserValidators.Add(new UserValidator<TUser>());
			mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

			mgr.Setup(x => x.DeleteAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
			mgr.Setup(x => x.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<TUser, string>((x, y) => ls.Add(x));
			mgr.Setup(x => x.UpdateAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
			mgr.Setup(x => x.AddToRolesAsync(It.IsAny<TUser>(), It.IsAny<IEnumerable<string>>())).ReturnsAsync(IdentityResult.Success);

			return mgr;
		}

		public static Account CreateFakeAccount(string id)
		{
			var fakeAccount = new Account()
			{
				Id = id,
				FullName = "Test Account"
			};
			return fakeAccount;
		}
		public static Topic CreateFakeTopic(Guid id)
		{
			var fakeTopic = new Topic()
			{
				Id = id,
				Name = "Test Topic",
				Year = "4"
			};
			return fakeTopic;
		}
		public static Subject CreateFakeSubject(Guid id)
		{
			var fakeSubject = new Subject()
			{
				Id = id,
				Name = "Test Subject",
				ExamBoard = "TestBoard"
			};
			return fakeSubject;
		}

		public static TestResult CreateFakeTestResult(string accountId,Guid resultId,Guid topicId, Guid subjectId)
		{
			var fakeTestResult = new TestResult()
			{
				Id = resultId,
				Notes = "Test Notes",
				PercentageResult = 100,
				Student = CreateFakeAccount(accountId),
				TopicId = topicId,
				SubjectId = subjectId
				
			};
			return fakeTestResult;
		}
	}
}
