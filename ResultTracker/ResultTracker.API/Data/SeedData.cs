using ResultTracker.API.Models.Domain;

namespace ResultTracker.API.Data
{
	public class SeedData
	{
		public static void Initialise(IServiceProvider serviceProvider)
		{
			var context = serviceProvider.GetRequiredService<ResultTrackerDbContext>();
			if (context.Results.Any()||context.Topics.Any()||context.Subjects.Any())
			{
				context.Results.RemoveRange(context.Results);
				context.Subjects.RemoveRange(context.Subjects);
				context.Topics.RemoveRange(context.Topics);
				context.SaveChanges();
			}

			var demoTopics = new List<Topic>() 
			{ 
				new Topic() { Id = Guid.Parse("d07121f2-0aa6-4a14-8d21-b086e2edf798"), Name = "Fractions", Year = "8" },
				new Topic() { Id = Guid.Parse("54a23bab-d599-4236-aacb-9e2a4ce2f7fc"), Name = "Grammar", Year = "3" },
				new Topic() { Id = Guid.Parse("a7a306f1-8993-413b-8784-045b2ee9c97b"), Name = "Implicit Differentiation", Year = "12" }
			};
			context.Topics.AddRange(demoTopics);

			var demoSubjects = new List<Subject>()
			{
				new Subject() { Id = Guid.Parse("19550b42-129e-4f84-83dd-4d4aea4bbbe0"), ExamBoard = "AQA", Name = "Foundation Mathematics" },
				new Subject() { Id = Guid.Parse("ef2a8d07-7047-4e15-bd2b-679bb638d789"), ExamBoard = "EDEXCEL", Name = "A-Level Mathematics" },
				new Subject() { Id = Guid.Parse("629d6eb2-9c92-4975-81cd-f4b896521622"), ExamBoard = "Early years", Name = "Engish" },
			};
			context.Subjects.AddRange(demoSubjects);

			var demoTestResults = new List<TestResult>()
			{
				new TestResult() //Foundation maths fraction bad grade.
				{
					Id = Guid.Parse("7e789338-26cc-4cd4-8b78-97e648e8092c"),
					Notes = "Revise more next time",
					PercentageResult = 17,
					TopicId = Guid.Parse("d07121f2-0aa6-4a14-8d21-b086e2edf798"),
					SubjectId = Guid.Parse("19550b42-129e-4f84-83dd-4d4aea4bbbe0")
				},
				new TestResult() //Foundation maths fraction good grade.
				{
					Id = Guid.Parse("9ed5ef67-697f-4d74-a387-dfdb5e781a15"),
					Notes = "Well done!",
					PercentageResult = 88,
					TopicId = Guid.Parse("d07121f2-0aa6-4a14-8d21-b086e2edf798"),
					SubjectId = Guid.Parse("19550b42-129e-4f84-83dd-4d4aea4bbbe0")
				},
				new TestResult() //A-level maths.
				{
					Id = Guid.Parse("ec1df671-6558-4907-b801-e65764146aa6"),
					Notes = "Careful with your derivatives in the final exam.",
					PercentageResult = 60,
					TopicId = Guid.Parse("a7a306f1-8993-413b-8784-045b2ee9c97b"),
					SubjectId = Guid.Parse("ef2a8d07-7047-4e15-bd2b-679bb638d789")
				},
				new TestResult() //Early years english.
				{
					Id = Guid.Parse("055d8083-8b01-4ec3-bbf2-1dfc08b4519c"),
					Notes = "Really good progress, keep it up",
					PercentageResult = 70,
					TopicId = Guid.Parse("54a23bab-d599-4236-aacb-9e2a4ce2f7fc"),
					SubjectId = Guid.Parse("629d6eb2-9c92-4975-81cd-f4b896521622")
				}
			};
			context.Results.AddRange(demoTestResults);

			context.SaveChanges();
		}
	}
}
