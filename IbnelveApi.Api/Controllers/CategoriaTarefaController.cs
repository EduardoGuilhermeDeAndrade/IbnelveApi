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
    private readonly string _tenantId;

    public CategoriaTarefaController(
        ICategoriaTarefaService service,
        ICurrentUserService currentUserService)
    {
        _service = service;
        _tenantId = currentUserService.GetTenantId();
    }

    /// <summary>
    /// Lista todas as categorias do tenant
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false)
    {
        if (string.IsNullOrEmpty(_tenantId))
            return BadRequest("TenantId não encontrado");

        var result = await _service.GetAllAsync(_tenantId, includeDeleted);

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }

    /// <summary>
    /// Lista categorias ativas para uso em selects
    /// </summary>
    [HttpGet("ativas")]
    public async Task<IActionResult> GetAtivasForSelect()
    {
        if (string.IsNullOrEmpty(_tenantId))
            return BadRequest("TenantId não encontrado");

        var result = await _service.GetAtivasForSelectAsync(_tenantId);

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }

    /// <summary>
    /// Busca categoria por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (string.IsNullOrEmpty(_tenantId))
            return BadRequest("TenantId não encontrado");

        var result = await _service.GetByIdAsync(id, _tenantId);

        if (result.Success)
            return Ok(result);

        return NotFound(result);
    }

    /// <summary>
    /// Cria nova categoria
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCategoriaTarefaDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(_tenantId) || string.IsNullOrEmpty(userId))
            return BadRequest("TenantId ou UserId não encontrado");

        var result = await _service.CreateAsync(dto, _tenantId, userId);

        if (result.Success)
            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);

        return BadRequest(result);
    }

    /// <summary>
    /// Atualiza categoria existente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoriaTarefaDto dto)
    {
        if (string.IsNullOrEmpty(_tenantId))
            return BadRequest("TenantId não encontrado");

        var result = await _service.UpdateAsync(id, dto, _tenantId);

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }

    /// <summary>
    /// Ativa categoria
    /// </summary>
    [HttpPatch("{id}/ativar")]
    public async Task<IActionResult> Ativar(int id)
    {
        if (string.IsNullOrEmpty(_tenantId))
            return BadRequest("TenantId não encontrado");

        var result = await _service.AtivarAsync(id, _tenantId);

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }

    /// <summary>
    /// Desativa categoria
    /// </summary>
    [HttpPatch("{id}/desativar")]
    public async Task<IActionResult> Desativar(int id)
    {
        if (string.IsNullOrEmpty(_tenantId))
            return BadRequest("TenantId não encontrado");

        var result = await _service.DesativarAsync(id, _tenantId);

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }

    /// <summary>
    /// Exclui categoria (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (string.IsNullOrEmpty(_tenantId))
            return BadRequest("TenantId não encontrado");

        var result = await _service.DeleteAsync(id, _tenantId);

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }
}

