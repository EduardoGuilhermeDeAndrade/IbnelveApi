using IbnelveApi.Domain.Enums;
using IbnelveApi.Application.Security;
using IbnelveApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IbnelveApi.Application.DTOs.AcessosPemissoes;

namespace IbnelveApi.Api.Controllers
{
    /// <summary>
    /// Exemplo de controller usando autorização baseada em Permission.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ExemploController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ICurrentUserService _currentUserService;

        public ExemploController(IAuthorizationService authorizationService, ICurrentUserService currentUserService)
        {
            _authorizationService = authorizationService;
            _currentUserService = currentUserService;
        }

        [Authorize(Policy = "Pessoa_Create")]
        [HttpPost("pessoa")]
        public async Task<IActionResult> CreatePessoa([FromBody] CreatePessoaDto dto)
        {
            // ... lógica de criação
            return Ok(new ApiResponse<string>(true, "Pessoa criada"));
        }

        [HttpPut("pessoa/{id}")]
        public async Task<IActionResult> UpdatePessoa(int id, [FromBody] UpdatePessoaDto dto)
        {
            var result = await _authorizationService.AuthorizeAsync(User, null, "Pessoa_Update");
            if (!result.Succeeded)
                return Forbid();
            // ... lógica de atualização
            return Ok(new ApiResponse<string>(true, "Pessoa atualizada"));
        }
    }
}