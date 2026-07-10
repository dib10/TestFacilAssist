using System.Net;

namespace FacilAssist.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _environment;

        public ExceptionHandlingMiddleware(RequestDelegate next, IWebHostEnvironment environment)
        {
            _next = next;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await TratarExcecaoAsync(context, ex);
            }
        }

        private Task TratarExcecaoAsync(HttpContext context, Exception ex)
        {
            var statusCode = ex switch
            {
                ArgumentException => HttpStatusCode.BadRequest,
                KeyNotFoundException => HttpStatusCode.NotFound,
                _ => HttpStatusCode.InternalServerError
            };

            var resposta = statusCode == HttpStatusCode.InternalServerError
                ? new
                {
                    erro = "Ocorreu um erro interno no servidor.",
                    detalhe = _environment.IsDevelopment() ? ex.Message : null
                }
                : new
                {
                    erro = ex.Message,
                    detalhe = (string?)null
                };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsJsonAsync(resposta);
        }
    }
}
