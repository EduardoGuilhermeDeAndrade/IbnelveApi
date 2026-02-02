using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using IbnelveApi.Api.Models;
using IbnelveApi.Application.Common;
using IbnelveApi.IoC;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

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

            var userClaims = await _userManager.GetClaimsAsync(user);

            // Obtenha valores específicos das claims
            var roleClaim = userClaims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value ?? "User";
            var tenantIdClaim = userClaims.FirstOrDefault(c => c.Type == "TenantId")?.Value ?? "algum-guid-aqui";
            var emailClaim = userClaims.FirstOrDefault(c => c.Type == "email")?.Value ?? user.Email;
            var permissionClaims = userClaims.Where(c => c.Type == "permission").Select(c => c.Value).ToList();

            // Claims obrigatórias do JWT
            var claims = new List<Claim>
            {
                new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", roleClaim),
                new Claim("TenantId", tenantIdClaim),
                new Claim("email", emailClaim),
                new Claim(JwtRegisteredClaimNames.Iss, _configuration["JwtSettings:Issuer"]),
                new Claim(JwtRegisteredClaimNames.Aud, _configuration["JwtSettings:Audience"])
            };

            // Adiciona permissões como claims múltiplas
            foreach (var permission in permissionClaims)
            {
                claims.Add(new Claim("permission", permission));
            }

            // ...código para gerar o token JWT, incluindo exp (expiration) na configuração do token...
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1), // exp
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var expirationHours = int.Parse(_configuration["JwtSettings:ExpirationHours"]!);

            var response = new LoginResponse
            {
                Token = tokenHandler.WriteToken(token),
                Email = user.Email!,
                TenantId = tenantIdClaim,
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

