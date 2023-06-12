using ResultTracker.API.Models.Domain;

namespace ResultTracker.API.Models.Dto
{
	public class TestResultDto
	{
		public Guid Id { get; set; }
		public string? Notes { get; set; }
		public int PercentageResult { get; set; }
		public TopicDto? topic { get; set; }
		public SubjectDto? Subject { get; set; }
	}
}
