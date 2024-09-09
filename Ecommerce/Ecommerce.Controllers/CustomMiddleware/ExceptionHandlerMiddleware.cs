using System.Text.Json;
using System.Text.Json.Serialization;
using Ecommerce.Domain.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Ecommerce.Controllers.CustomMiddleware
{
    public class ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger) : IMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> _logger = logger;

        private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
        {
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (PostgresException ex)
            {
                await HandlePostgresExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            const string contentType = "application/json";
            context.Response.Clear();
            context.Response.ContentType = contentType;

            ProblemDetails problemDetails;
            string json;

            switch (ex)
            {
                case CategoryNotFoundException categoryNotFoundException:
                    context.Response.StatusCode = 404;
                    problemDetails = CreateProblemDetails(context, 404, categoryNotFoundException.Message);
                    json = ToJson(problemDetails);
                    await context.Response.WriteAsync(json);
                    break;
                case CartItemNotFoundExcepiton cartItemNotFoundExcepiton:
                    context.Response.StatusCode = 404;
                    problemDetails = CreateProblemDetails(context, 404, cartItemNotFoundExcepiton.Message);
                    json = ToJson(problemDetails);
                    await context.Response.WriteAsync(json);
                    break;
                case OrderNotFoundException orderNotFoundException:
                    context.Response.StatusCode = 404;
                    problemDetails = CreateProblemDetails(context, 404, orderNotFoundException.Message);
                    json = ToJson(problemDetails);
                    await context.Response.WriteAsync(json);
                    break;
                case ProductNotFoundException productNotFoundException:
                    context.Response.StatusCode = 404;
                    problemDetails = CreateProblemDetails(context, 404, productNotFoundException.Message);
                    json = ToJson(problemDetails);
                    await context.Response.WriteAsync(json);
                    break;
                case UserNotFoundException userNotFoundException:
                    context.Response.StatusCode = 404;
                    problemDetails = CreateProblemDetails(context, 404, userNotFoundException.Message);
                    json = ToJson(problemDetails);
                    await context.Response.WriteAsync(json);
                    break;
                case InsufficientStockException insufficientStockException:
                    context.Response.StatusCode = 400;
                    problemDetails = CreateProblemDetails(context, 400, insufficientStockException.Message);
                    json = ToJson(problemDetails);
                    await context.Response.WriteAsync(json);
                    break;
                case WrongCredentialsException wrongCredentialsException:
                    context.Response.StatusCode = 400;
                    problemDetails = CreateProblemDetails(context, 400, wrongCredentialsException.Message);
                    json = ToJson(problemDetails);
                    await context.Response.WriteAsync(json);
                    break;
                case NotUniqueEmailException notUniqueEmailException:
                    context.Response.StatusCode = 409;
                    problemDetails = CreateProblemDetails(context, 409, notUniqueEmailException.Message);
                    json = ToJson(problemDetails);
                    await context.Response.WriteAsync(json);
                    break;
                case ReviewNotFoundException reviewNotFoundException:
                    context.Response.StatusCode = 404;
                    problemDetails = CreateProblemDetails(context, 404, reviewNotFoundException.Message);
                    json = ToJson(problemDetails);
                    await context.Response.WriteAsync(json);
                    break;
                case UnauthorizedAccessException unauthorizedAccessException:
                    context.Response.StatusCode = 403;
                    problemDetails = CreateProblemDetails(context, 403, unauthorizedAccessException.Message);
                    json = ToJson(problemDetails);
                    await context.Response.WriteAsync(json);
                    break;
                default:
                    problemDetails = CreateProblemDetails(context, 500, ex.Message);
                    json = ToJson(problemDetails);
                    await context.Response.WriteAsync(json);
                    break;
            }
            _logger.LogError(ex, "An exception has occurred: {Message}", ex.Message);
        }

        private async Task HandlePostgresExceptionAsync(HttpContext context, PostgresException ex)
        {

            const string contentType = "application/json;";

            context.Response.Clear();
            context.Response.ContentType = contentType;

            ProblemDetails problemDetails;
            string json;

            switch (ex.SqlState)
            {
                case "23505":
                    problemDetails = CreateProblemDetails(context, 409, $"Duplicate key error: {ex.MessageText}");
                    json = ToJson(problemDetails);
                    await context.Response.WriteAsync(json);
                    break;
                case "P0001":
                    problemDetails = CreateProblemDetails(context, 400, $"Something went wrong: {ex.MessageText}");
                    json = ToJson(problemDetails);
                    await context.Response.WriteAsync(json);
                    break;
                default:
                    problemDetails = CreateProblemDetails(context, 500, ex.Message);
                    json = ToJson(problemDetails);
                    await context.Response.WriteAsync(json);
                    break;
            }

            _logger.LogError(ex, "PostgresException occurred: {Message}", ex.Message);
        }

        private static ProblemDetails CreateProblemDetails(HttpContext context, int statusCode, string detail)
        {
            context.Response.StatusCode = statusCode;
            return new ProblemDetails
            {
                Status = statusCode,
                Detail = detail
            };
        }

        private string ToJson(ProblemDetails problemDetails)
        {
            try
            {
                return JsonSerializer.Serialize(problemDetails, SerializerOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception has occurred while serializing error to JSON.");
            }
            return string.Empty;
        }
    }
}