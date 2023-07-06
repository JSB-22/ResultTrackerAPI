using System.ComponentModel.DataAnnotations;

namespace ResultTracker.UI.Models.Dto
{
	public class AddTopicRequestDto
	{
		[StringLength(20)]
        [Required(ErrorMessage = "Topic name is required")]
        public string Name { get; set; }
		[Required]
		[RegularExpression(@"^R$|^1[0-3]|[1-9]$", ErrorMessage = "Allowed entries Reception (R) to 13.")]
		public string Year { get; set; }
	}
}
