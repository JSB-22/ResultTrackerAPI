namespace ResultTracker.UI.Models.Dto
{
	public class AddTestResultRequestDto
	{
		public string? Notes { get; set; }
		public int PercentageResult { get; set; }
		public Guid TopicId { get; set; }
		public Guid SubjectId { get; set; }

		public string StudentId { get; set; } = "";
	}
}
