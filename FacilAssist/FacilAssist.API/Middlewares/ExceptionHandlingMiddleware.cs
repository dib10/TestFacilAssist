using Microsoft.Data.SqlClient;
using System.Net;

namespace FacilAssist.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            IWebHostEnvironment environment,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _environment = environment;
            _logger = logger;
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
            var (statusCode, mensagem) = MapearExcecao(ex);

            RegistrarExcecao(context, ex, statusCode, mensagem);

            var resposta = statusCode == HttpStatusCode.InternalServerError
                ? new
                {
                    erro = mensagem,
                    detalhe = _environment.IsDevelopment() ? ex.Message : null
                }
                : new
                {
                    erro = mensagem,
                    detalhe = (string?)null
                };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsJsonAsync(resposta);
        }

        private void RegistrarExcecao(HttpContext context, Exception ex, HttpStatusCode statusCode, string mensagem)
        {
            if (statusCode == HttpStatusCode.InternalServerError)
            {
                _logger.LogError(
                    ex,
                    "Erro interno ao processar {Metodo} {Caminho}.",
                    context.Request.Method,
                    context.Request.Path
                );

                return;
            }

            _logger.LogWarning(
                "Requisicao rejeitada com status {StatusCode} em {Metodo} {Caminho}: {Mensagem}",
                (int)statusCode,
                context.Request.Method,
                context.Request.Path,
                mensagem
            );
        }

        private static (HttpStatusCode StatusCode, string Mensagem) MapearExcecao(Exception ex)
        {
            return ex switch
            {
                ArgumentException argumentException => (HttpStatusCode.BadRequest, argumentException.Message),
                KeyNotFoundException keyNotFoundException => (HttpStatusCode.NotFound, keyNotFoundException.Message),
                SqlException sqlException when ContemErroSql(sqlException, 2601, 2627) =>
                    (HttpStatusCode.Conflict, "Já existe um cliente cadastrado com este CPF."),
                SqlException sqlException when ContemErroSql(sqlException, 547) =>
                    (HttpStatusCode.BadRequest, "A situação informada não existe."),
                SqlException sqlException when ContemErroSql(sqlException, 8152, 2628) =>
                    (HttpStatusCode.BadRequest, "Algum campo ultrapassou o tamanho permitido."),
                _ => (HttpStatusCode.InternalServerError, "Ocorreu um erro interno no servidor.")
            };
        }

        private static bool ContemErroSql(SqlException exception, params int[] numeros)
        {
            foreach (SqlError erro in exception.Errors)
            {
                if (numeros.Contains(erro.Number))
                    return true;
            }

            return false;
        }
    }
}
