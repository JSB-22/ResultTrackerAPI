using System.ComponentModel.DataAnnotations;

namespace ResultTracker.API.Models.Dto
{
	public class RoleChangeRequestDto
	{
		[Required]
		[DataType(DataType.EmailAddress)]
		public string Username { get; set; } = string.Empty;
		public string[]? Roles { get; set; }
		public string? TeacherId { get; set; } = null;
	}
}
