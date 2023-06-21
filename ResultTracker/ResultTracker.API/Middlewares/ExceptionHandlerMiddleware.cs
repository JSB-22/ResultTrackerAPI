using System.Net;

namespace ResultTracker.API.Middlewares
{
	public class ExceptionHandlerMiddleware
	{
		private readonly RequestDelegate next;

		public ExceptionHandlerMiddleware(RequestDelegate next) // Could inject logger
        {
			this.next = next;
		}

		public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{
				await next(httpContext);
			}
			catch (Exception ex)
			{
				var id = Guid.NewGuid();
				httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				httpContext.Response.ContentType = "application/json";

				var error = new { Id = id, ErrorMessage = "An Error has occured." };

				await  httpContext.Response.WriteAsJsonAsync(error);
				throw;
			}
		}
    }
}
