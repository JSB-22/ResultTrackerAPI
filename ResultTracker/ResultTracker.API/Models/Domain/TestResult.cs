using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ResultTracker.API.Users.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;

namespace ResultTracker.API.Models.Domain
{
	public class TestResult
	{
        public Guid Id { get; set; }
        public string? Notes { get; set; }
		public int PercentageResult { get; set; }
		public Guid SubjectId { get; set; }
		public Guid TopicId { get; set; }
		public Topic Topic { get; set; }
		public Subject Subject { get; set; }
		[ValidateNever]
		[ForeignKey("Student")]
		public string StudentId { get; set; } = "";
		public Account Student { get; set; }

    }
}
