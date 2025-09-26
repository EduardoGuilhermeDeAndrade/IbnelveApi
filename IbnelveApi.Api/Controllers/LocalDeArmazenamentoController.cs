using IbnelveApi.Application.DTOs.LocalDeArmazenamento;
using IbnelveApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IbnelveApi.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LocalDeArmazenamentoController : ControllerBase
{
    private readonly ILocalDeArmazenamentoService _service;

    public LocalDeArmazenamentoController(ILocalDeArmazenamentoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tenantId = User.FindFirst("TenantId")?.Value;
        if (string.IsNullOrEmpty(tenantId))
            return BadRequest("TenantId não encontrado");

        var result = await _service.GetAllAsync(tenantId);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var tenantId = User.FindFirst("TenantId")?.Value;
        if (string.IsNullOrEmpty(tenantId))
            return BadRequest("TenantId não encontrado");

        var result = await _service.GetByIdAsync(id, tenantId);
        if (result.Success)
            return Ok(result);

        return NotFound(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLocalDeArmazenamentoDto dto)
    {
        var tenantId = User.FindFirst("TenantId")?.Value;
        if (string.IsNullOrEmpty(tenantId))
            return BadRequest("TenantId não encontrado");

        var result = await _service.CreateAsync(dto, tenantId);
        return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateLocalDeArmazenamentoDto dto)
    {
        var tenantId = User.FindFirst("TenantId")?.Value;
        if (string.IsNullOrEmpty(tenantId))
            return BadRequest("TenantId não encontrado");

        var result = await _service.UpdateAsync(id, dto, tenantId);
        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var tenantId = User.FindFirst("TenantId")?.Value;
        if (string.IsNullOrEmpty(tenantId))
            return BadRequest("TenantId não encontrado");

        var result = await _service.DeleteAsync(id, tenantId);
        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }
}