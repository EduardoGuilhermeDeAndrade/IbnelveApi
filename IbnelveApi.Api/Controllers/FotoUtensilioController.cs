using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IbnelveApi.Application.DTOs.FotoUtensilio;
using IbnelveApi.Application.Interfaces;


[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FotoUtensilioController : ControllerBase
{
    private readonly IFotoUtensilioService _service;
    private readonly IFotoStorageService _fotoStorageService;
    private readonly string _tenantId;

    public FotoUtensilioController(
        IFotoUtensilioService service,
        IFotoStorageService fotoStorageService,
        ICurrentUserService currentUserService)
    {
        _service = service;
        _fotoStorageService = fotoStorageService;
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
        return Ok(result);
    }

    [HttpPost("{utensilioId}")]
    public async Task<IActionResult> Create(int utensilioId, [FromBody] CreateFotoUtensilioDto dto)
    {
        if (string.IsNullOrEmpty(_tenantId))
            return BadRequest("TenantId não encontrado");

        var result = await _service.CreateAsync(dto, utensilioId, _tenantId);
        return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateFotoUtensilioDto dto)
    {
        if (string.IsNullOrEmpty(_tenantId))
            return BadRequest("TenantId não encontrado");

        var result = await _service.UpdateAsync(id, dto, _tenantId);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (string.IsNullOrEmpty(_tenantId))
            return BadRequest("TenantId não encontrado");

        var result = await _service.DeleteAsync(id, _tenantId);
        return Ok(result);
    }
}