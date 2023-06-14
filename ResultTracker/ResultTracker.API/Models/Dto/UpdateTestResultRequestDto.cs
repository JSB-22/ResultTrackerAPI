namespace ResultTracker.API.Models.Dto
{
	public class UpdateTestResultRequestDto
	{
		public string? Notes { get; set; }
		public int PercentageResult { get; set; }
		public Guid TopicId { get; set; }
		public Guid SubjectId { get; set; }
		
	}
}
