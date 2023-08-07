using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ResultTracker.UI.Models.Dto
{
	public class EditTestResultViewModel
	{
		public Guid Id { get; set; }
		// For Drop down list functionality:
		public List<SelectListItem> TopicSelectList { get; set; }
		public List<SelectListItem> SubjectSelectList { get; set; }
		public List<SelectListItem> AccountSelectList { get; set; }

		// Fields the drop down lists pass to: (Note subject and topic need reconverting to GUID). 
		[BindRequired]
		[Required(ErrorMessage = "A Topic is required")]
		public string SelectedTopicId { get; set; } = string.Empty;

		[BindRequired]
		[Required(ErrorMessage = "A Subject is required")]
		public string SelectedSubjectId { get; set; } = string.Empty;

		[BindRequired]
		[Required(ErrorMessage = "A Student must be assigned")]
		public string SelectedAccountId { get; set; } = string.Empty;

		// Properties not requiring drop down: 
		[StringLength(100)]
		public string? Notes { get; set; }
		[Range(0, 100)]
		[Display(Name = "Result %")]
		[Required(ErrorMessage = "A grade is required")]
		public int PercentageResult { get; set; }
	}
}
