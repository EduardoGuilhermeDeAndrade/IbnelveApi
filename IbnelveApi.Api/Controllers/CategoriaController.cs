using FluentValidation;
using IbnelveApi.Application.Common;
using IbnelveApi.Application.DTOs.Categoria;
using IbnelveApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriaController : ControllerBase
{
    private readonly IValidator<CreateCategoriaUtensilioDto> _validatorCreate;
    private readonly IValidator<UpdateCategoriaUtensilioDto> _validatorUpdate;
    private readonly ICategoriaUtensilioService _categoriaService;
    private readonly ICurrentUserService _currentUserService;

    public CategoriaController(
        IValidator<CreateCategoriaUtensilioDto> validatorCreate,
        IValidator<UpdateCategoriaUtensilioDto> validatorUpdate,
        ICategoriaUtensilioService categoriaService,
        ICurrentUserService currentUserService)
    {
        _validatorCreate = validatorCreate;
        _validatorUpdate = validatorUpdate;
        _categoriaService = categoriaService;
        _currentUserService = currentUserService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<CategoriaUtensilioDto>>>> GetAll([FromQuery] bool includeInactive = false)
    {
        var tenantId = _currentUserService.GetTenantId();
        if (string.IsNullOrEmpty(tenantId))
            return BadRequest(ApiResponse<IEnumerable<CategoriaUtensilioDto>>.ErrorResult("TenantId não encontrado"));

        var categorias = await _categoriaService.GetAllAsync(tenantId, includeInactive);
        return Ok(categorias);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CategoriaUtensilioDto>>> GetById(int id)
    {
        var tenantId = _currentUserService.GetTenantId();
        if (string.IsNullOrEmpty(tenantId))
            return BadRequest(ApiResponse<CategoriaUtensilioDto>.ErrorResult("TenantId não encontrado"));

        var categoria = await _categoriaService.GetByIdAsync(id, tenantId);
        if (!categoria.Success)
            return NotFound(categoria);

        return Ok(categoria);
    }

    [HttpGet("nome/{nome}")]
    public async Task<ActionResult<ApiResponse<CategoriaUtensilioDto>>> GetByNome(string nome)
    {
        var tenantId = _currentUserService.GetTenantId();
        if (string.IsNullOrEmpty(tenantId))
            return BadRequest(ApiResponse<CategoriaUtensilioDto>.ErrorResult("TenantId não encontrado"));

        var categoria = await _categoriaService.GetByNomeAsync(nome, tenantId);
        if (!categoria.Success)
            return NotFound(categoria);

        return Ok(categoria);
    }

    [HttpGet("search")]
    public async Task<ActionResult<ApiResponse<IEnumerable<CategoriaUtensilioDto>>>> Search([FromQuery] string searchTerm, [FromQuery] bool includeInactive = false)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return BadRequest(ApiResponse<IEnumerable<CategoriaUtensilioDto>>.ErrorResult("Termo de busca é obrigatório"));

        var tenantId = _currentUserService.GetTenantId();
        if (string.IsNullOrEmpty(tenantId))
            return BadRequest(ApiResponse<IEnumerable<CategoriaUtensilioDto>>.ErrorResult("TenantId não encontrado"));

        var categorias = await _categoriaService.SearchAsync(searchTerm, tenantId, includeInactive);
        return Ok(categorias);
    }

    [HttpGet("filtros")]
    public async Task<ActionResult<ApiResponse<IEnumerable<CategoriaUtensilioDto>>>> GetWithFilters(
        [FromQuery] string? nome = null,
        [FromQuery] bool? ativa = null,
        [FromQuery] bool includeInactive = false,
        [FromQuery] string orderBy = "Nome")
    {
        var tenantId = _currentUserService.GetTenantId();
        if (string.IsNullOrEmpty(tenantId))
            return BadRequest(ApiResponse<IEnumerable<CategoriaUtensilioDto>>.ErrorResult("TenantId não encontrado"));

        var categorias = await _categoriaService.GetWithFiltersAsync(tenantId, nome, ativa, includeInactive, orderBy);
        return Ok(categorias);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CategoriaUtensilioDto>>> Create([FromBody] CreateCategoriaUtensilioDto createDto)
    {
        var validationResult = await _validatorCreate.ValidateAsync(createDto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(ApiResponse<CategoriaUtensilioDto>.ErrorResult("Dados inválidos", errors));
        }

        var tenantId = _currentUserService.GetTenantId();
        if (string.IsNullOrEmpty(tenantId))
            return BadRequest(ApiResponse<CategoriaUtensilioDto>.ErrorResult("TenantId não encontrado"));

        var result = await _categoriaService.CreateAsync(createDto, tenantId);
        if (result.Success)
            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);

        return BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<CategoriaUtensilioDto>>> Update(int id, [FromBody] UpdateCategoriaUtensilioDto updateDto)
    {
        var validationResult = await _validatorUpdate.ValidateAsync(updateDto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(ApiResponse<CategoriaUtensilioDto>.ErrorResult("Dados inválidos", errors));
        }

        var tenantId = _currentUserService.GetTenantId();
        if (string.IsNullOrEmpty(tenantId))
            return BadRequest(ApiResponse<CategoriaUtensilioDto>.ErrorResult("TenantId não encontrado"));

        var result = await _categoriaService.UpdateAsync(id, updateDto, tenantId);
        if (result.Success)
            return Ok(result);

        return NotFound(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        var tenantId = _currentUserService.GetTenantId();
        if (string.IsNullOrEmpty(tenantId))
            return BadRequest(ApiResponse<bool>.ErrorResult("TenantId não encontrado"));

        var result = await _categoriaService.DeleteAsync(id, tenantId);
        if (result.Success)
            return Ok(result);

        return NotFound(result);
    }
}