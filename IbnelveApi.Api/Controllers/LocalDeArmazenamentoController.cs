using IbnelveApi.Application.DTOs.LocalDeArmazenamento;
using IbnelveApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IbnelveApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LocalDeArmazenamentoController : ControllerBase
{
    private readonly ILocalDeArmazenamentoService _service;
    private readonly string _tenantId;

    public LocalDeArmazenamentoController(
        ILocalDeArmazenamentoService service,
        ICurrentUserService currentUserService)
    {
        _service = service;
        _tenantId = currentUserService.GetTenantId();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        if (string.IsNullOrEmpty(_tenantId))
            return BadRequest("TenantId não encontrado");

        var result = await _service.GetAllAsync(_tenantId);
        return Ok(result);
    }

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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLocalDeArmazenamentoDto dto)
    {
        if (string.IsNullOrEmpty(_tenantId))
            return BadRequest("TenantId não encontrado");

        var result = await _service.CreateAsync(dto, _tenantId);
        return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateLocalDeArmazenamentoDto dto)
    {
        if (string.IsNullOrEmpty(_tenantId))
            return BadRequest("TenantId não encontrado");

        var result = await _service.UpdateAsync(id, dto, _tenantId);
        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }

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