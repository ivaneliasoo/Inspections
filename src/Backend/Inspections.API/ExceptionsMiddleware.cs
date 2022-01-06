using System;
using System.Net;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Inspections.API
{
    /// <summary>
    /// Errors handling Middleware
    /// </summary>
    public class ExceptionsMiddleware
    {
        private const string ContentType = "application/json";
        private readonly ILogger<ExceptionsMiddleware> _logger;
        private readonly RequestDelegate _next;

        /// <summary>
        /// creates a new instance of <see cref="ExceptionsMiddleware"/> class.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="next"></param>
        public ExceptionsMiddleware(ILogger<ExceptionsMiddleware> logger, RequestDelegate next)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _next = next;
        }

        /// <summary>
        /// Srialize exceptions and log the event
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public Task Invoke(HttpContext httpContext)
        {
            Guard.Against.Null(httpContext, nameof(httpContext));
            return InvokeAsync(httpContext);
        }

        async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                _logger.LogInformation($"Requesting {httpContext.Request.Path.Value} Method: {httpContext.Request.Method} - {DateTime.Now}");
                await _next(httpContext).ConfigureAwait(false);
                _logger.LogInformation($"Requested {httpContext.Request.Path.Value} Method: {httpContext.Request.Method} - {DateTime.Now}");
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                var statusCode = ConfigureExceptionTypes(ex);

                httpContext.Response.StatusCode = statusCode;
                httpContext.Response.ContentType = ContentType;

                var error = JsonConvert.SerializeObject(new
                {
                    message = ex.Message,
                    innerexception = ex.InnerException?.StackTrace,
                });

                await httpContext.Response.WriteAsync(error).ConfigureAwait(false);

                _logger.LogError($"Error: Code {statusCode}. Message: {ex.Message}. InnerException: {ex.InnerException}");
            }
        }

        private static int ConfigureExceptionTypes(Exception ex)
        {
            var statusCode = ex switch
            {
                var _ when ex is ArgumentNullException => (int)HttpStatusCode.BadRequest,
                var _ when ex is NotImplementedException => (int)HttpStatusCode.NotFound,
                _ => (int)HttpStatusCode.InternalServerError,
            };
            return statusCode;
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionsMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionsMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionsMiddleware>();
        }
    }
}
