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
        var pessoas = await _pessoaService.GetAllAsync();
        return Ok(pessoas);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<PessoaDto>>> GetById(int id)
    {
        var pessoa = await _pessoaService.GetByIdAsync(id);

        if (!pessoa.Success)
            return NotFound(pessoa);

        return Ok(pessoa);
    }

    [HttpGet("cpf/{cpf}")]
    public async Task<ActionResult<ApiResponse<PessoaDto>>> GetByCpf(string cpf)
    {
        var pessoa = await _pessoaService.GetByCpfAsync(cpf);

        if (!pessoa.Success)
            return NotFound(pessoa);

        return Ok(pessoa);
    }

    [HttpGet("nome/{nome}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<PessoaDto>>>> GetByNome(string nome)
    {
        var pessoas = await _pessoaService.GetByNomeAsync(nome);
        return Ok(pessoas);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<PessoaDto>>> Create([FromBody] CreatePessoaDto createDto)
    {
        var result = await _pessoaService.CreateAsync(createDto);

        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<PessoaDto>>> Update(int id, [FromBody] UpdatePessoaDto updateDto)
    {
        var result = await _pessoaService.UpdateAsync(id, updateDto);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        var result = await _pessoaService.DeleteAsync(id);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }
}
