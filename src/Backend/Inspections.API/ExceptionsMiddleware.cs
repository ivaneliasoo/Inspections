using System;
using System.Net;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ZadERP.Api.Middleware
{
    /// <summary>
    /// Middleware para el manejo de errores
    /// </summary>
    public class ExceptionsMiddleware
    {
        private const string ContentType = "application/json";
        private readonly ILogger<ExceptionsMiddleware> _logger;
        private readonly RequestDelegate _next;

        /// <summary>
        /// Crea una Nueva Instancia de <see cref="ExceptionsMiddleware"/> class.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="next"></param>
        public ExceptionsMiddleware(ILogger<ExceptionsMiddleware> logger, RequestDelegate next)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _next = next;
        }

        /// <summary>
        /// Serializa Las Excepciones y Deja Registro en el Logger
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
                _logger.LogInformation($"Se está invocando {httpContext.Request.Path.Value} Metodo: {httpContext.Request.Method} - {DateTime.Now}");
                await _next(httpContext).ConfigureAwait(false);
                _logger.LogInformation($"Se ha invocando {httpContext.Request.Path.Value} Metodo: {httpContext.Request.Method} - {DateTime.Now}");
            }
            catch (Exception ex)
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

                _logger.LogError($"Error: Code {statusCode}. Mensaje: {ex.Message}. InnerException: {ex.InnerException}");
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
