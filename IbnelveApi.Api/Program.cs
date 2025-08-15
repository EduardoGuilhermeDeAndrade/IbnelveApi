using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using IbnelveApi.Infrastructure.Data;
using IbnelveApi.IoC;

using IbnelveApi.Api.Extensions;
using IbnelveApi.Application.Interfaces;
using IbnelveApi.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Registrar TenantContext como Scoped
builder.Services.AddScoped<ITenantContext, TenantContext>();

// Add all dependencies (Infrastructure, Application, Repositories)
builder.Services.AddAllDependencies(builder.Configuration);

//Corrige pois não estava abrindo no localhost, somente pelo ip http://127.0.0.1:5000/index.html
//Agora abre normalmente pelo: http://localhost:5082/index.html
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5082); // Porta HTTP
    options.ListenLocalhost(7202, listenOptions =>
    {
        listenOptions.UseHttps();
    }); // Porta HTTPS
});

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
        Description = "API RESTful com Clean Architecture, Multi-tenancy e JWT Authentication"
    });

    // Configure JWT authentication in Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
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
            new string[] {}
        }
    });
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "IbnelveApi v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at app's root
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseTenantMiddleware();
app.UseAuthorization();

app.MapControllers();

// Seed data on startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    
    // Ensure database is created
    await context.Database.EnsureCreatedAsync();
    
    // Seed data
    await DataSeeder.SeedAsync(context, userManager);
}

app.Run("http://0.0.0.0:5000");

