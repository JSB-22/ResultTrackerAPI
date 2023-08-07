namespace ResultTracker.UI.Models.Dto
{
    public class TestResultDto
    {
        public Guid Id { get; set; }
        public string? Notes { get; set; }
        public int PercentageResult { get; set; }
        public TopicDto? Topic { get; set; }
        public SubjectDto? Subject { get; set; }

        public AccountDto? Account { get; set; }
    }
}
