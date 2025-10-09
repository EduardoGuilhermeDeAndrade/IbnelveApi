using System.Net;
using FluentValidation;
using FluentValidation.Results;
using IbnelveApi.Application.Common;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Microsoft.Extensions.Hosting; // Adicionado para IHostEnvironment

namespace IbnelveApi.Api.middlewares
{
    public class ValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env; // Adicionado

        public ValidationMiddleware(RequestDelegate next, IHostEnvironment env) // Modificado
        {
            _next = next;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex) // Captura erros do FluentValidation
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";

                var errors = ex.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();

                var response = ApiResponse<object>.ErrorResult("Erro de validação", errors);

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
            catch (Exception ex) // Captura exceções gerais
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var errors = _env.IsDevelopment()
                    ? new List<string> { ex.Message, ex.StackTrace ?? "" }
                    : new List<string>();

                var response = ApiResponse<object>.ErrorResult(
                    "Erro interno do servidor",
                    errors
                );

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}