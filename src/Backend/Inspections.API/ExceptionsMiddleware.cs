using System;
using System.Net;
using System.Threading.Tasks;
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
        private const string _contentType = "application/json";
        private readonly ILogger<ExceptionsMiddleware> _logger;
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Crea una Nueva Instancia de <see cref="ExceptionsMiddleware"/> class.
        /// </summary>
        /// <param name="next"></param>
        public ExceptionsMiddleware(ILogger<ExceptionsMiddleware> logger, RequestDelegate next, IServiceProvider serviceProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _next = next;
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        /// <summary>
        /// Serializa Las Excepciones y Deja Registro en el Logger
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public Task Invoke(HttpContext httpContext) => InvokeAsync(httpContext);

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
                httpContext.Response.ContentType = _contentType;

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
            int statusCode;

            switch (ex)
            {
                case var _ when ex is ArgumentNullException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case var _ when ex is NotImplementedException:
                    statusCode = (int)HttpStatusCode.NotFound;
                    break;
                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

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
