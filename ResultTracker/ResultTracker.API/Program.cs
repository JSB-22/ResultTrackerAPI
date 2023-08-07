
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ResultTracker.API.Data;
using ResultTracker.API.Middlewares;
using ResultTracker.API.Repositories;
using ResultTracker.API.Repositories.Interfaces;
using ResultTracker.API.Users.Domain;
using System.Text;

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
			#region Adding Authentication to Swagger: 
			builder.Services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new OpenApiInfo { Title = "Result Tracker API", Version = "v1" });
				options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme { Name = "Authorization", In = ParameterLocation.Header, Type = SecuritySchemeType.ApiKey, Scheme = JwtBearerDefaults.AuthenticationScheme });
				options.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
					new OpenApiSecurityScheme
					{ Reference = new OpenApiReference
					{ Type = ReferenceType.SecurityScheme, Id = JwtBearerDefaults.AuthenticationScheme
					},
						Scheme = "Oauth2",
						Name = JwtBearerDefaults.AuthenticationScheme,
						In = ParameterLocation.Header
					}, new List<string>() }});
			});
			#endregion

			builder.Services.AddDbContext<ResultTrackerDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ResultTrackerConnectionString")));

			builder.Services.AddAutoMapper(typeof(Program).Assembly);

			builder.Services.AddScoped<ITopicRepository, SQLTopicRepository>();
			builder.Services.AddScoped<ISubjectRepository, SQLSubjectRepository>();
			builder.Services.AddScoped<ITestResultRepository, SQLTestResultRepository>();
			builder.Services.AddScoped<IAccountRepository, SQLAccountRepository>();
			builder.Services.AddScoped<ITokenRepository, TokenRepository>();

			#region Authentication:

			builder.Services.AddIdentityCore<Account>(option => option.SignIn.RequireConfirmedEmail = false) //Confirm this.  WAS <IdentityUser>
				.AddRoles<IdentityRole>()
				.AddTokenProvider<DataProtectorTokenProvider<Account>>("ResultTracker")
				.AddEntityFrameworkStores<ResultTrackerDbContext>()
				.AddDefaultTokenProviders();

			builder.Services.Configure<IdentityOptions>(options =>
			{
				options.Password.RequireDigit = false;
				options.Password.RequireLowercase = false;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;
				options.Password.RequiredLength = 6;
				options.Password.RequiredUniqueChars = 1;
			});

			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			   .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
			   {
				   ValidateIssuer = true,
				   ValidateAudience = true,
				   ValidateLifetime = true,
				   ValidateIssuerSigningKey = true,
				   ValidIssuer = builder.Configuration["Jwt:Issuer"],
				   ValidAudience = builder.Configuration["Jwt:Audience"],
				   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
			   });
			#endregion

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

			app.UseMiddleware<ExceptionHandlerMiddleware>();

			app.UseHttpsRedirection();

			app.UseAuthentication();

			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}