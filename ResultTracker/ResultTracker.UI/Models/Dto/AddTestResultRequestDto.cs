using System.ComponentModel.DataAnnotations;

namespace ResultTracker.UI.Models.Dto
{
	public class AddTestResultRequestDto
	{
		[StringLength(100)]
		public string? Notes { get; set; }
		[Required(ErrorMessage = "A grade is required")]
		public int PercentageResult { get; set; }
		[Required(ErrorMessage = "A Topic must be assigned")]
		public Guid TopicId { get; set; }
		[Required(ErrorMessage = "A Subject must be assigned")]
		public Guid SubjectId { get; set; }
		[Required(ErrorMessage = "A Student must be assigned")]
		public string StudentId { get; set; } = "";
	}
}
