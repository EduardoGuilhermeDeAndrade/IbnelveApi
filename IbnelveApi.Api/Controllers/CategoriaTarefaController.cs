using FluentValidation;
using IbnelveApi.Application.DTOs.CategoriaTarefa;
using IbnelveApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IbnelveApi.Api.Controllers;

/// <summary>
/// Controller para gerenciamento de categorias de tarefa
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriaTarefaController : ControllerBase
{
    private readonly ICategoriaTarefaService _service;
    private readonly IValidator<CreateCategoriaTarefaDto> _createValidator;
    private readonly IValidator<UpdateCategoriaTarefaDto> _updateValidator;

    public CategoriaTarefaController(
        ICategoriaTarefaService service,
        IValidator<CreateCategoriaTarefaDto> createValidator,
        IValidator<UpdateCategoriaTarefaDto> updateValidator)
    {
        _service = service;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    /// <summary>
    /// Lista todas as categorias do tenant
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false)
    {
        var tenantId = User.FindFirst("TenantId")?.Value;
        if (string.IsNullOrEmpty(tenantId))
        {
            return BadRequest("TenantId não encontrado");
        }

        var result = await _service.GetAllAsync(tenantId, includeDeleted);
        
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    /// <summary>
    /// Lista categorias ativas para uso em selects
    /// </summary>
    [HttpGet("ativas")]
    public async Task<IActionResult> GetAtivasForSelect()
    {
        var tenantId = User.FindFirst("TenantId")?.Value;
        if (string.IsNullOrEmpty(tenantId))
        {
            return BadRequest("TenantId não encontrado");
        }

        var result = await _service.GetAtivasForSelectAsync(tenantId);
        
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    /// <summary>
    /// Busca categoria por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var tenantId = User.FindFirst("TenantId")?.Value;
        if (string.IsNullOrEmpty(tenantId))
        {
            return BadRequest("TenantId não encontrado");
        }

        var result = await _service.GetByIdAsync(id, tenantId);
        
        if (result.Success)
        {
            return Ok(result);
        }

        return NotFound(result);
    }

    /// <summary>
    /// Cria nova categoria
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCategoriaTarefaDto dto)
    {
        var validationResult = await _createValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var tenantId = User.FindFirst("TenantId")?.Value;
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(userId))
        {
            return BadRequest("TenantId ou UserId não encontrado");
        }

        var result = await _service.CreateAsync(dto, tenantId, userId);
        
        if (result.Success)
        {
            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);
        }

        return BadRequest(result);
    }

    /// <summary>
    /// Atualiza categoria existente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoriaTarefaDto dto)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var tenantId = User.FindFirst("TenantId")?.Value;
        if (string.IsNullOrEmpty(tenantId))
        {
            return BadRequest("TenantId não encontrado");
        }

        var result = await _service.UpdateAsync(id, dto, tenantId);
        
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    /// <summary>
    /// Ativa categoria
    /// </summary>
    [HttpPatch("{id}/ativar")]
    public async Task<IActionResult> Ativar(int id)
    {
        var tenantId = User.FindFirst("TenantId")?.Value;
        if (string.IsNullOrEmpty(tenantId))
        {
            return BadRequest("TenantId não encontrado");
        }

        var result = await _service.AtivarAsync(id, tenantId);
        
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    /// <summary>
    /// Desativa categoria
    /// </summary>
    [HttpPatch("{id}/desativar")]
    public async Task<IActionResult> Desativar(int id)
    {
        var tenantId = User.FindFirst("TenantId")?.Value;
        if (string.IsNullOrEmpty(tenantId))
        {
            return BadRequest("TenantId não encontrado");
        }

        var result = await _service.DesativarAsync(id, tenantId);
        
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    /// <summary>
    /// Exclui categoria (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var tenantId = User.FindFirst("TenantId")?.Value;
        if (string.IsNullOrEmpty(tenantId))
        {
            return BadRequest("TenantId não encontrado");
        }

        var result = await _service.DeleteAsync(id, tenantId);
        
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
}

