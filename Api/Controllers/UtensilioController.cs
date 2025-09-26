using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UtensilioController : ControllerBase
{
    private readonly IUtensilioService _service;

    public UtensilioController(IUtensilioService service)
    {
        _service = service;
    }

    private string GetTenantId() =>
        User.FindFirstValue("TenantId") ?? throw new UnauthorizedAccessException("TenantId não encontrado.");

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tenantId = GetTenantId();
        var result = await _service.GetAllAsync(tenantId);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var tenantId = GetTenantId();
        var result = await _service.GetByIdAsync(id, tenantId);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUtensilioDto dto)
    {
        var tenantId = GetTenantId();
        var result = await _service.CreateAsync(dto, tenantId);
        return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUtensilioDto dto)
    {
        var tenantId = GetTenantId();
        var result = await _service.UpdateAsync(id, dto, tenantId);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var tenantId = GetTenantId();
        var result = await _service.DeleteAsync(id, tenantId);
        return Ok(result);
    }
}