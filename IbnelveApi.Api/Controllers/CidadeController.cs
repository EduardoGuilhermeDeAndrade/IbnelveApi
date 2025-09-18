using IbnelveApi.Domain.Entities;
using IbnelveApi.Application.Common;
using FluentValidation;
using IbnelveApi.Application.DTOs;
using IbnelveApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IbnelveApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CidadeController : ControllerBase
{
    private readonly IValidator<CreateCidadeDto> _validatorCreate;
    private readonly IValidator<UpdateCidadeDto> _validatorUpdate;
    private readonly ICidadeService _cidadeService;

    public CidadeController(
        IValidator<CreateCidadeDto> validatorCreate,
        IValidator<UpdateCidadeDto> validatorUpdate,
        ICidadeService cidadeService)
    {
        _validatorCreate = validatorCreate;
        _validatorUpdate = validatorUpdate;
        _cidadeService = cidadeService;
    }

    /// <summary>
    /// Lista todas as cidades
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<CidadeDto>>>> GetAll([FromQuery] bool includeDeleted = false)
    {
        var result = await _cidadeService.GetAllAsync(includeDeleted);
        return Ok(result);
    }

    /// <summary>
    /// Busca cidade por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CidadeDto>>> GetById(int id)
    {
        var result = await _cidadeService.GetByIdAsync(id);
        if (!result.Success)
            return NotFound(result);
        return Ok(result);
    }

    /// <summary>
    /// Cria nova cidade
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<CidadeDto>>> Create([FromBody] CreateCidadeDto createDto)
    {
        var validationResult = await _validatorCreate.ValidateAsync(createDto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(ApiResponse<CidadeDto>.ErrorResult("Dados inválidos", errors));
        }
        var result = await _cidadeService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);
    }

    /// <summary>
    /// Atualiza cidade
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<CidadeDto>>> Update(int id, [FromBody] UpdateCidadeDto updateDto)
    {
        var validationResult = await _validatorUpdate.ValidateAsync(updateDto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(ApiResponse<CidadeDto>.ErrorResult("Dados inválidos", errors));
        }
        var result = await _cidadeService.UpdateAsync(id, updateDto);
        if (!result.Success)
            return NotFound(result);
        return Ok(result);
    }

    /// <summary>
    /// Exclui cidade (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        var result = await _cidadeService.DeleteAsync(id);
        if (!result.Success)
            return NotFound(result);
        return Ok(result);
    }
}
