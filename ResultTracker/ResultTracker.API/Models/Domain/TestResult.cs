using System.Security.Cryptography.X509Certificates;

namespace ResultTracker.API.Models.Domain
{
	public class TestResult
	{
        public Guid Id { get; set; }
        public string? Notes { get; set; }
		public int PercentageResult { get; set; }
		public Guid SubjectId { get; set; }
		public Guid TopicID { get; set; }
		public Topic Topic { get; set; }
		public Subject Subject { get; set; }

    }
}
