using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Insania.Api.Swagger;

/// <summary>
/// Класс фильтров операции для сваггера
/// </summary>
public class AuthenticationRequirementsOperationFilter : IOperationFilter
{
    /// <summary>
    /// Метод применение токена
    /// </summary>
    /// <param name="operation">Операция сваггера</param>
    /// <param name="context">Контекст ззапроса</param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Security ??= [];

        OpenApiSecurityScheme scheme = new()
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        };

        operation.Security.Add(new OpenApiSecurityRequirement { [scheme] = [] });
    }
}