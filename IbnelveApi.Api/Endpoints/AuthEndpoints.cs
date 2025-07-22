using Microsoft.AspNetCore.Identity;
using IbnelveApi.Infrastructure.Identity;
using System.ComponentModel.DataAnnotations;

namespace IbnelveApi.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/auth")
            .WithTags("Autenticação");

        // POST /api/auth/login
        group.MapPost("/login", async (LoginRequest request, UserManager<IdentityUser> userManager, IJwtService jwtService) =>
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null || !await userManager.CheckPasswordAsync(user, request.Password))
            {
                return Results.BadRequest(new { Success = false, Message = "Credenciais inválidas", Errors = new[] { "Email ou senha incorretos" } });
            }

            var token = await jwtService.GenerateTokenAsync(user);
            return Results.Ok(new { Success = true, Message = "Login realizado com sucesso", Data = new { Token = token, User = new { user.Id, user.Email, user.UserName } } });
        })
        .WithName("Login")
        .WithSummary("Realizar login")
        .WithDescription("Autentica o usuário e retorna um token JWT")
        .Produces<object>(200)
        .Produces<object>(400);

        // POST /api/auth/register
        group.MapPost("/register", async (RegisterRequest request, UserManager<IdentityUser> userManager, IJwtService jwtService) =>
        {
            var existingUser = await userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return Results.BadRequest(new { Success = false, Message = "Usuário já existe", Errors = new[] { "Este email já está em uso" } });
            }

            var user = new IdentityUser
            {
                UserName = request.Email,
                Email = request.Email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToArray();
                return Results.BadRequest(new { Success = false, Message = "Erro ao criar usuário", Errors = errors });
            }

            var token = await jwtService.GenerateTokenAsync(user);
            return Results.Ok(new { Success = true, Message = "Usuário criado com sucesso", Data = new { Token = token, User = new { user.Id, user.Email, user.UserName } } });
        })
        .WithName("Register")
        .WithSummary("Registrar usuário")
        .WithDescription("Cria um novo usuário e retorna um token JWT")
        .Produces<object>(200)
        .Produces<object>(400);
    }
}

public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}

public class RegisterRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;
}

