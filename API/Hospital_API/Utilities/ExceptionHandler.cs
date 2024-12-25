using Hospital_API.ValueTypes;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace Hospital_API.Utilities
{
    public sealed class ExceptionHandler : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {

                ProblemDetails problemDeatils = new()
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Title = "Server error",
                    Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
                    Detail = ex.Message
                };

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsJsonAsync(
                    new Result(
                        isSuccess: false,
                        error: new Error(
                            $"Status:{HttpStatusCode.InternalServerError}" +
                            $"Details:{JsonSerializer.Serialize(problemDeatils)}")));
            }
        }
    }
}
