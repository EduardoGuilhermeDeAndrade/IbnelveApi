using IbnelveApi.Application.Dtos.Utensilio;
using IbnelveApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IbnelveApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UtensilioController : ControllerBase
{
    private readonly IUtensilioService _utensilioService;
    private readonly string _tenantId;

    public UtensilioController(
        IUtensilioService utensilioService,
        ICurrentUserService currentUserService)
    {
        _utensilioService = utensilioService;
        _tenantId = currentUserService.GetTenantId();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UtensilioDto>>> GetAll()
    {
        if (string.IsNullOrEmpty(_tenantId))
            return BadRequest("TenantId não encontrado");

        var result = await _utensilioService.GetAllAsync(_tenantId);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UtensilioDto>> GetById(int id)
    {
        if (string.IsNullOrEmpty(_tenantId))
            return BadRequest("TenantId não encontrado");

        var result = await _utensilioService.GetByIdAsync(id, _tenantId);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<UtensilioDto>> Create([FromBody] CreateUtensilioDto dto)
    {
        if (string.IsNullOrEmpty(_tenantId))
            return BadRequest("TenantId não encontrado");

        var result = await _utensilioService.CreateAsync(dto, _tenantId);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUtensilioDto dto)
    {
        if (string.IsNullOrEmpty(_tenantId))
            return BadRequest("TenantId não encontrado");

        if (id != dto.Id) return BadRequest();
        var success = await _utensilioService.UpdateAsync(dto, _tenantId);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (string.IsNullOrEmpty(_tenantId))
            return BadRequest("TenantId não encontrado");

        var success = await _utensilioService.DeleteAsync(id, _tenantId);
        if (!success) return NotFound();
        return NoContent();
    }
}