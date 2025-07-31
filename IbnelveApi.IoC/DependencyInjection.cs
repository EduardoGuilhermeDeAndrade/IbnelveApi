using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FluentValidation;
using IbnelveApi.Application.Interfaces;
using IbnelveApi.Application.Services;
using IbnelveApi.Application.Validators;
using IbnelveApi.Domain.Interfaces;
using IbnelveApi.Infrastructure.Data;
using IbnelveApi.Infrastructure.Repositories;

namespace IbnelveApi.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Identity
        services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        // JWT Authentication
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"];

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!)),
                ClockSkew = TimeSpan.Zero
            };
        });

        return services;
    }

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Application Services
        services.AddScoped<IPessoaService, PessoaService>();
        services.AddScoped<ITarefaService, TarefaService>();
        
        // JWT Service
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        // FluentValidation
        services.AddValidatorsFromAssemblyContaining<CreatePessoaDtoValidator>();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        // Repositories
        services.AddScoped<IPessoaRepository, PessoaRepository>();
        services.AddScoped<ITarefaRepository, TarefaRepository>();

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        return services;
    }

    public static IServiceCollection AddAllDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);
        services.AddApplication();
        services.AddRepositories();

        return services;
    }
}

