using ResultTracker.API.Models.Domain;
using ResultTracker.API.Users;

namespace ResultTracker.API.Models.Dto
{
	public class TestResultDto
	{
		public Guid Id { get; set; }
		public string? Notes { get; set; }
		public int PercentageResult { get; set; }
		public TopicDto? topic { get; set; }
		public SubjectDto? Subject { get; set; }

		public AccountDto? StudentAccount { get; set; }
	}
}
