using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using System.Text.Json;

namespace BiiGBackend.Models.SharedModels
{
    public class ResponseModal : ActionResult
    {
        public object Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)StatusCode;
            context.HttpContext.Response.ContentType = "application/json";
            JsonSerializerOptions options = new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            await context.HttpContext.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(Data, options)));
        }

        public static ResponseModal Send(object? Data = null, HttpStatusCode? StatusCode = HttpStatusCode.OK)
        {
            var response = new ResponseModal();
            response.Data = Data ?? string.Empty;
            response.StatusCode = (HttpStatusCode)StatusCode;
            return response;
        }
    }
}
