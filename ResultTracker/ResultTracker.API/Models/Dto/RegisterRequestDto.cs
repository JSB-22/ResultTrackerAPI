using System.ComponentModel.DataAnnotations;

namespace ResultTracker.API.Models.Dto
{
	public class RegisterRequestDto
	{
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "password";
        public string FullName { get; set; } = "John Smith";
        //public string[] Roles { get; set; }
        public string? TeacherId { get; set; } = null;
    }
}
