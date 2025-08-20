using FluentValidation;
using IbnelveApi.Application.Common;
using IbnelveApi.Application.DTOs;
using IbnelveApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace IbnelveApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PessoaController : ControllerBase
{
    private readonly IValidator<CreatePessoaDto> _validatorCreate;
    private readonly IValidator<UpdatePessoaDto> _validatorUpdate;
    private readonly IPessoaService _pessoaService; // Seu serviço de negócio

    public PessoaController(IValidator<CreatePessoaDto> validatorCreate, IValidator<UpdatePessoaDto> validatorUpdate, IPessoaService pessoaService)
    {
        _validatorCreate = validatorCreate;
        _validatorUpdate = validatorUpdate;
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
        var validationResult = await _validatorCreate.ValidateAsync(createDto);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var novaPessoa = await _pessoaService.CreateAsync(createDto);

        if (!novaPessoa.Success)
            return BadRequest(novaPessoa);

        return CreatedAtAction(nameof(GetById), new { id = novaPessoa.Data?.Id }, novaPessoa);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<PessoaDto>>> Update(int id, [FromBody] UpdatePessoaDto updateDto)
    {
        var validationResult = await _validatorUpdate.ValidateAsync(updateDto);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var novaPessoa = await _pessoaService.UpdateAsync(id, updateDto);

        if (!novaPessoa.Success)
            return BadRequest(novaPessoa);

        return Ok(novaPessoa);

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
