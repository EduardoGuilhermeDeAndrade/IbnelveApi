using IbnelveApi.Application.Common;
using IbnelveApi.Application.DTOs.Categoria;
using IbnelveApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriaUtensilioController : ControllerBase
{
    private readonly ICategoriaUtensilioService _categoriaService;
    private readonly ICurrentUserService _currentUserService;
    private readonly string _tenantId;

    public CategoriaUtensilioController(
        ICategoriaUtensilioService categoriaService,
        ICurrentUserService currentUserService)
    {
        _categoriaService = categoriaService;
        _currentUserService = currentUserService;
        _tenantId = _currentUserService.GetTenantId();
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<CategoriaUtensilioDto>>>> GetAll([FromQuery] bool includeInactive = false)
    {
        if (string.IsNullOrEmpty(_tenantId))
            return BadRequest(ApiResponse<IEnumerable<CategoriaUtensilioDto>>.ErrorResult("TenantId não encontrado"));

        var categorias = await _categoriaService.GetAllAsync(_tenantId, includeInactive);
        return Ok(categorias);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CategoriaUtensilioDto>>> GetById(int id)
    {
        if (string.IsNullOrEmpty(_tenantId))
            return BadRequest(ApiResponse<CategoriaUtensilioDto>.ErrorResult("TenantId não encontrado"));

        var categoria = await _categoriaService.GetByIdAsync(id, _tenantId);
        if (!categoria.Success)
            return NotFound(categoria);

        return Ok(categoria);
    }

    [HttpGet("nome/{nome}")]
    public async Task<ActionResult<ApiResponse<CategoriaUtensilioDto>>> GetByNome(string nome)
    {
        if (string.IsNullOrEmpty(_tenantId))
            return BadRequest(ApiResponse<CategoriaUtensilioDto>.ErrorResult("TenantId não encontrado"));

        var categoria = await _categoriaService.GetByNomeAsync(nome, _tenantId);
        if (!categoria.Success)
            return NotFound(categoria);

        return Ok(categoria);
    }

    [HttpGet("search")]
    public async Task<ActionResult<ApiResponse<IEnumerable<CategoriaUtensilioDto>>>> Search([FromQuery] string searchTerm, [FromQuery] bool includeInactive = false)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return BadRequest(ApiResponse<IEnumerable<CategoriaUtensilioDto>>.ErrorResult("Termo de busca é obrigatório"));

        if (string.IsNullOrEmpty(_tenantId))
            return BadRequest(ApiResponse<IEnumerable<CategoriaUtensilioDto>>.ErrorResult("TenantId não encontrado"));

        var categorias = await _categoriaService.SearchAsync(searchTerm, _tenantId, includeInactive);
        return Ok(categorias);
    }

    [HttpGet("filtros")]
    public async Task<ActionResult<ApiResponse<IEnumerable<CategoriaUtensilioDto>>>> GetWithFilters(
        [FromQuery] string? nome = null,
        [FromQuery] bool? ativa = null,
        [FromQuery] bool includeInactive = false,
        [FromQuery] string orderBy = "Nome")
    {
        if (string.IsNullOrEmpty(_tenantId))
            return BadRequest(ApiResponse<IEnumerable<CategoriaUtensilioDto>>.ErrorResult("TenantId não encontrado"));

        var categorias = await _categoriaService.GetWithFiltersAsync(_tenantId, nome, ativa, includeInactive, orderBy);
        return Ok(categorias);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CategoriaUtensilioDto>>> Create([FromBody] CreateCategoriaUtensilioDto createDto)
    {
        if (string.IsNullOrEmpty(_tenantId))
            return BadRequest(ApiResponse<CategoriaUtensilioDto>.ErrorResult("TenantId não encontrado"));

        var result = await _categoriaService.CreateAsync(createDto, _tenantId);
        if (result.Success)
            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);

        return BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<CategoriaUtensilioDto>>> Update(int id, [FromBody] UpdateCategoriaUtensilioDto updateDto)
    {
        if (string.IsNullOrEmpty(_tenantId))
            return BadRequest(ApiResponse<CategoriaUtensilioDto>.ErrorResult("TenantId não encontrado"));

        var result = await _categoriaService.UpdateAsync(id, updateDto, _tenantId);
        if (result.Success)
            return Ok(result);

        return NotFound(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        if (string.IsNullOrEmpty(_tenantId))
            return BadRequest(ApiResponse<bool>.ErrorResult("TenantId não encontrado"));

        var result = await _categoriaService.DeleteAsync(id, _tenantId);
        if (result.Success)
            return Ok(result);

        return NotFound(result);
    }
}