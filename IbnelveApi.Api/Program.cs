using IbnelveApi.IoC;
using IbnelveApi.Api.Middleware;
using IbnelveApi.Api.Endpoints;
using IbnelveApi.Infrastructure.Data;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container - CONFIGURAÇÃO CENTRALIZADA NO DependencyInjection.cs
// ESTA LINHA JÁ REGISTRA: Identity, Authentication, Authorization, Repositories, Services, etc.
builder.Services.AddAllServices(builder.Configuration);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "IbnelveApi",
        Version = "v1",
        Description = "API com Clean Architecture, Multi-Tenancy e JWT Authentication"
    });

    // Configure JWT authentication in Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando o esquema Bearer. Exemplo: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Add X-Tenant-Id header parameter for all operations
    c.OperationFilter<TenantHeaderOperationFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "IbnelveApi v1");
        c.RoutePrefix = string.Empty; // Swagger na raiz
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

// Use tenant middleware before authentication
app.UseTenantMiddleware();

// Authentication and Authorization middleware
// ESTES MIDDLEWARES FUNCIONAM PORQUE OS SERVIÇOS JÁ FORAM REGISTRADOS NO DependencyInjection.cs
app.UseAuthentication();
app.UseAuthorization();

// Map endpoints
app.MapAuthEndpoints();
app.MapProdutoEndpoints();



// Health check endpoint
app.MapGet("/health", () => Results.Ok(new
{
    Status = "Healthy",
    Timestamp = DateTime.UtcNow,
    Environment = app.Environment.EnvironmentName
}))
.WithName("HealthCheck")
.WithTags("Health");




// Seed data on startup
using (var scope = app.Services.CreateScope())
{
    await DataSeeder.SeedAsync(scope.ServiceProvider);
}

app.Run();

// Operation filter for Swagger to add X-Tenant-Id header to all operations
public class TenantHeaderOperationFilter : Swashbuckle.AspNetCore.SwaggerGen.IOperationFilter
{
    public void Apply(Microsoft.OpenApi.Models.OpenApiOperation operation, Swashbuckle.AspNetCore.SwaggerGen.OperationFilterContext context)
    {
        operation.Parameters ??= new List<Microsoft.OpenApi.Models.OpenApiParameter>();

        operation.Parameters.Add(new Microsoft.OpenApi.Models.OpenApiParameter
        {
            Name = "X-Tenant-Id",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Required = false,
            Description = "ID do tenant para Multi-Tenancy (ex: tenant1, tenant2)",
            Schema = new Microsoft.OpenApi.Models.OpenApiSchema
            {
                Type = "string",
                Default = new Microsoft.OpenApi.Any.OpenApiString("tenant1")
            }
        });
    }
}

