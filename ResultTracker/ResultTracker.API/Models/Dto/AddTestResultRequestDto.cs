using ResultTracker.API.Models.Domain;

namespace ResultTracker.API.Models.Dto
{
	public class AddTestResultRequestDto
	{
		public string? Notes { get; set; }
		public int PercentageResult { get; set; }
		public Guid SubjectId { get; set; }
		public Guid TopicID { get; set; }
	}
}
