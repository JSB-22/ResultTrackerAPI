
using Microsoft.EntityFrameworkCore;
using ResultTracker.API.Data;
using ResultTracker.API.Repositories;
using ResultTracker.API.Repositories.Interfaces;

namespace ResultTracker.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Services.AddDbContext<ResultTrackerDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ResultTrackerConnectionString")));

			builder.Services.AddAutoMapper(typeof(Program).Assembly);

			builder.Services.AddScoped<ITopicRepository, SQLTopicRepository>();
			builder.Services.AddScoped<ISubjectRepository, SQLSubjectRepository>();

			var app = builder.Build();

			#region Injection of SeedData:
			using (var scope = app.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				SeedData.Initialise(services);
			}
			#endregion

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}