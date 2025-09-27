using IbnelveApi.Application.Interfaces;
using IbnelveApi.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace IbnelveApi.Application.Services;

/// <summary>
/// Implementação do serviço para capturar informações do usuário atual
/// VERSÃO SIMPLIFICADA: Usa apenas FindFirst (método nativo do ClaimsPrincipal)
/// </summary>
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        return user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
    }

    public string GetTenantId()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        return user?.FindFirst("TenantId")?.Value ?? string.Empty;
    }

    public string GetUserName()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        return user?.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
    }

    public bool IsAuthenticated()
    {
        return _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }

    public Role GetUserRole()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        var roleStr = user?.FindFirst(ClaimTypes.Role)?.Value ?? "User";
        return Enum.TryParse<Role>(roleStr, out var role) ? role : Role.User;
    }
}

