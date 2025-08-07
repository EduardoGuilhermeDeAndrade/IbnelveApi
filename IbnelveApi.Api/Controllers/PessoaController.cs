
using IbnelveApi.Application.Common;
using IbnelveApi.Application.DTOs;
using IbnelveApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IbnelveApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PessoaController : ControllerBase
{
    private readonly IPessoaService _pessoaService;

    public PessoaController(IPessoaService pessoaService)
    {
        _pessoaService = pessoaService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<PessoaDto>>>> GetAll()
    {
        // Não precisa mais passar tenantId - é aplicado automaticamente
        var pessoas = await _pessoaService.GetAllAsync();
        return Ok(pessoas);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<PessoaDto>>> GetById(int id)
    {
        // Não precisa mais passar tenantId - é aplicado automaticamente
        var pessoa = await _pessoaService.GetByIdAsync(id);

        if (pessoa == null)
            return NotFound(ApiResponse<PessoaDto>.ErrorResult("Pessoa não encontrada"));

        return Ok((pessoa));
    }

    [HttpGet("cpf/{cpf}")]
    public async Task<ActionResult<ApiResponse<PessoaDto>>> GetByCpf(string cpf)
    {
        // Não precisa mais passar tenantId - é aplicado automaticamente
        var pessoa = await _pessoaService.GetByCpfAsync(cpf);

        if (pessoa == null)
            return NotFound(ApiResponse<PessoaDto>.ErrorResult("Pessoa não encontrada"));

        return Ok(pessoa);
    }

    // Outros métodos seguem o mesmo padrão...
}


