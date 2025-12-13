using AiAgentEconomy.Application.Exceptions;
using System.Net;

namespace AiAgentEconomy.API.Middleware
{
    public sealed class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");

                var (statusCode, title) = ex switch
                {
                    NotFoundException => ((int)HttpStatusCode.NotFound, "Not Found"),
                    ConflictException => ((int)HttpStatusCode.Conflict, "Conflict"),
                    ValidationException => ((int)HttpStatusCode.BadRequest, "Bad Request"),
                    ArgumentException => ((int)HttpStatusCode.BadRequest, "Bad Request"),
                    _ => ((int)HttpStatusCode.InternalServerError, "Internal Server Error")
                };

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/problem+json";

                var traceId = context.TraceIdentifier;

                var payload = new
                {
                    type = $"https://httpstatuses.com/{statusCode}",
                    title,
                    status = statusCode,
                    detail = ex.Message,
                    traceId
                };

                await context.Response.WriteAsJsonAsync(payload);
            }
        }
    }
}
