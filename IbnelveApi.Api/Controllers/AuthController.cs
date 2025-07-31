using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using IbnelveApi.Api.Models;
using IbnelveApi.Application.Common;
using IbnelveApi.IoC;

namespace IbnelveApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IConfiguration _configuration;

    public AuthController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IJwtTokenService jwtTokenService,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenService = jwtTokenService;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<LoginResponse>>> Login([FromBody] LoginRequest request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return BadRequest(ApiResponse<LoginResponse>.ErrorResult("Credenciais inválidas"));

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
                return BadRequest(ApiResponse<LoginResponse>.ErrorResult("Credenciais inválidas"));

            // Get TenantId from user claims
            var claims = await _userManager.GetClaimsAsync(user);
            var tenantIdClaim = claims.FirstOrDefault(c => c.Type == "TenantId");
            
            if (tenantIdClaim == null)
                return BadRequest(ApiResponse<LoginResponse>.ErrorResult("Usuário não possui TenantId configurado"));

            var token = _jwtTokenService.GenerateToken(user.Id, user.Email!, tenantIdClaim.Value);
            var expirationHours = int.Parse(_configuration["JwtSettings:ExpirationHours"]!);

            var response = new LoginResponse
            {
                Token = token,
                Email = user.Email!,
                TenantId = tenantIdClaim.Value,
                ExpiresAt = DateTime.UtcNow.AddHours(expirationHours)
            };

            return Ok(ApiResponse<LoginResponse>.SuccessResult(response, "Login realizado com sucesso"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<LoginResponse>.ErrorResult("Erro interno do servidor", ex.Message));
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<string>>> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
                return BadRequest(ApiResponse<string>.ErrorResult("Email já está em uso"));

            var user = new IdentityUser
            {
                UserName = request.Email,
                Email = request.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(ApiResponse<string>.ErrorResult("Erro ao criar usuário", errors));
            }

            // Add TenantId claim
            await _userManager.AddClaimAsync(user, new Claim("TenantId", request.TenantId));

            return Ok(ApiResponse<string>.SuccessResult("Usuário criado com sucesso", "Usuário registrado com sucesso"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<string>.ErrorResult("Erro interno do servidor", ex.Message));
        }
    }
}

