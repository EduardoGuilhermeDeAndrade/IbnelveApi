using IbnelveApi.Application.Dtos.Utensilio;
using IbnelveApi.Application.Interfaces;
using IbnelveApi.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IbnelveApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UtensilioController : ControllerBase
{

    private readonly IUtensilioService _utensilioService;
    private readonly ICurrentUserService _currentUserService;

    public UtensilioController(IUtensilioService utensilioService, ICurrentUserService currentUserService)
    {
        _utensilioService = utensilioService;
        _currentUserService = currentUserService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UtensilioDto>>> GetAll()
    {
        var tenantId = _currentUserService.GetTenantId();
        var result = await _utensilioService.GetAllAsync(tenantId);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UtensilioDto>> GetById(int id)
    {
        var tenantId = _currentUserService.GetTenantId();
        var result = await _utensilioService.GetByIdAsync(id, tenantId);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<UtensilioDto>> Create([FromBody] CreateUtensilioDto dto)
    {
        var tenantId = _currentUserService.GetTenantId();
        var result = await _utensilioService.CreateAsync(dto, tenantId);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUtensilioDto dto)
    {
        var tenantId = _currentUserService.GetTenantId();
        if (id != dto.Id) return BadRequest();
        var success = await _utensilioService.UpdateAsync(dto, tenantId);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var tenantId = _currentUserService.GetTenantId();
        var success = await _utensilioService.DeleteAsync(id, tenantId);
        if (!success) return NotFound();
        return NoContent();
    }
}