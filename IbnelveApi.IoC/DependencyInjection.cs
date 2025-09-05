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
using IbnelveApi.Domain.Interfaces;
using IbnelveApi.Infrastructure.Data;
using IbnelveApi.Infrastructure.Repositories;
using IbnelveApi.Application.Validators.Membro;

namespace IbnelveApi.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddAllDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);
        services.AddApplication();

        return services;
    }

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // ===== APPLICATION SERVICES =====
        services.AddScoped<IMembroService, MembroService>();
        services.AddScoped<ITarefaService, TarefaService>();

        //  ADICIONADO: Serviço para capturar contexto do usuário atual
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        // JWT Service
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        // ===== VALIDATION =====
        services.AddValidatorsFromAssemblyContaining<CreateMembroDtoValidator>();

        // ===== REPOSITORIES =====

        // Repositório genérico base

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        // Repositórios base por tipo de entidade
        services.AddScoped(typeof(IGlobalRepository<>), typeof(GlobalRepository<>));
        services.AddScoped(typeof(ITenantRepository<>), typeof(TenantRepository<>));
        services.AddScoped(typeof(IUserOwnedRepository<>), typeof(UserOwnedRepository<>));

        // Repositórios específicos existentes
        services.AddScoped<IMembroRepository, MembroRepository>();
        services.AddScoped<ITarefaRepository, TarefaRepository>();

        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // ===== DATABASE =====
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // ===== IDENTITY =====
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

        // ===== JWT AUTHENTICATION =====
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

        //  ADICIONADO: HttpContextAccessor é necessário para o CurrentUserService
        services.AddHttpContextAccessor();

        return services;
    }
}

