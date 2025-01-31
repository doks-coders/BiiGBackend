using System.Text.Json;
using TutorApplication.SharedModels.Models;

namespace BiiGBackend.Web.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;

        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);

            }
            catch (Exception ex)
            {
                var error = new ErrorModel(ex.Message, ex.Message, ex.StackTrace ?? "Internal Server Error");


                context.Response.ContentType = "application/json";
                context.Response.StatusCode = int.Parse(error.StatusCode);
                await context.Response.WriteAsync(JsonSerializer.Serialize(error));
            }


        }
    }

}
