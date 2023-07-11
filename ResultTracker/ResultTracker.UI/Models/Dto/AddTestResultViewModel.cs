using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ResultTracker.UI.Models.Dto
{
    public class AddTestResultViewModel
    {

		// For Drop down list functionality:
		[Required(ErrorMessage = "A Topic must be assigned")]
		public List<SelectListItem> TopicSelectList { get; set; }
		[Required, Range(1, int.MaxValue, ErrorMessage = "Select a Topic")] // Range used so default option not usable. 
		public string SelectedTopicId { get; set; } = string.Empty;
		[Required(ErrorMessage = "A Subject must be assigned")]
		public List<SelectListItem> SubjectSelectList { get; set; }
		[Required, Range(1, int.MaxValue, ErrorMessage = "Select a Subject")]
		public string SelectedSubjectId { get; set; } = string.Empty; //Convert back to GUID before post. 
		[Required(ErrorMessage = "A Student must be assigned")]
		public List<SelectListItem> AccountSelectList { get; set; }
		[Required, Range(1, int.MaxValue, ErrorMessage = "Select a Student")]
		public string SelectedAccountId { get; set; } = string.Empty;

		// Properties not requiring drop down: 
		[StringLength(100)]
		public string? Notes { get; set; }
		[Required(ErrorMessage = "A grade is required")]
		public int PercentageResult { get; set; }
	}
}
