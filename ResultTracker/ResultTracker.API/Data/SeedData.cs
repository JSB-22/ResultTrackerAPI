using ResultTracker.API.Models.Domain;

namespace ResultTracker.API.Data
{
	public class SeedData
	{
		public static void Initialise(IServiceProvider serviceProvider)
		{
			var context = serviceProvider.GetRequiredService<ResultTrackerDbContext>();
			context.Results.RemoveRange(context.Results);
			context.Subjects.RemoveRange(context.Subjects);
			context.Topics.RemoveRange(context.Topics);

			Topic testTopic = new Topic() { Id = Guid.Parse("d07121f2-0aa6-4a14-8d21-b086e2edf798"), Name = "Fractions", Year = "8" };
			context.Topics.Add(testTopic);

			Subject testSubject = new Subject() { Id = Guid.Parse("19550b42-129e-4f84-83dd-4d4aea4bbbe0"), ExamBoard = "AQA", Name = "Foundation Mathematics" };
			context.Subjects.Add(testSubject);

			context.SaveChanges();
		}
	}
}
