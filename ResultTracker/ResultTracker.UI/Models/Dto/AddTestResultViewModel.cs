using Microsoft.AspNetCore.Mvc.Rendering;

namespace ResultTracker.UI.Models.Dto
{
    public class AddTestResultViewModel
    {

        // For Drop down list functionality:
        public List<SelectListItem> TopicSelectList { get; set; }
        public string SelectedTopicId { get; set; } = string.Empty;
        public List<SelectListItem> SubjectSelectList { get; set; }
        public string SelectedSubjectId { get; set; } = string.Empty; //Convert back to GUID before post. 
        public List<SelectListItem> AccountSelectList { get; set; }
        public string SelectedAccountId { get; set; } = string.Empty;

        // Properties not requiring drop down: 
        public string? Notes { get; set; }
        public int PercentageResult { get; set; }
    }
}
