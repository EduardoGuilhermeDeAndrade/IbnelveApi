using FluentValidation;
using IbnelveApi.Application.Common;
using IbnelveApi.Application.DTOs;
using IbnelveApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IbnelveApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MembroController : ControllerBase
{
    private readonly IValidator<CreateMembroDto> _validatorCreate;
    private readonly IValidator<UpdateMembroDto> _validatorUpdate;
    private readonly IMembroService _membroService;
    private readonly ICurrentUserService _currentUserService; 

    public MembroController(
        IValidator<CreateMembroDto> validatorCreate,
        IValidator<UpdateMembroDto> validatorUpdate,
        IMembroService membroService,
        ICurrentUserService currentUserService) 
    {
        _validatorCreate = validatorCreate;
        _validatorUpdate = validatorUpdate;
        _membroService = membroService;
        _currentUserService = currentUserService; 
    }

    /// <summary>
    /// Lista todas as Membros do tenant
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<MembroDto>>>> GetAll(
        [FromQuery] bool includeDeleted = false)
    {
        try
        {
            var tenantId = _currentUserService.GetTenantId(); 

            if (string.IsNullOrEmpty(tenantId))
                return BadRequest(ApiResponse<IEnumerable<MembroDto>>.ErrorResult("TenantId não encontrado"));

            var membros = await _membroService.GetAllAsync(tenantId, includeDeleted); 
            return Ok(membros);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<MembroDto>>.ErrorResult("Erro interno do servidor", ex.Message));
        }
    }

    /// <summary>
    /// Busca membro por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<MembroDto>>> GetById(int id)
    {
        try
        {
            var tenantId = _currentUserService.GetTenantId(); 

            if (string.IsNullOrEmpty(tenantId))
                return BadRequest(ApiResponse<MembroDto>.ErrorResult("TenantId não encontrado"));

            var membro = await _membroService.GetByIdAsync(id, tenantId); 

            if (!membro.Success)
                return NotFound(membro);

            return Ok(membro);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<MembroDto>.ErrorResult("Erro interno do servidor", ex.Message));
        }
    }

    /// <summary>
    /// Busca membro por CPF
    /// </summary>
    [HttpGet("cpf/{cpf}")]
    public async Task<ActionResult<ApiResponse<MembroDto>>> GetByCpf(string cpf)
    {
        try
        {
            var tenantId = _currentUserService.GetTenantId(); 

            if (string.IsNullOrEmpty(tenantId))
                return BadRequest(ApiResponse<MembroDto>.ErrorResult("TenantId não encontrado"));

            var membro = await _membroService.GetByCpfAsync(cpf, tenantId); 

            if (!membro.Success)
                return NotFound(membro);

            return Ok(membro);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<MembroDto>.ErrorResult("Erro interno do servidor", ex.Message));
        }
    }

    /// <summary>
    /// Busca membros por nome
    /// </summary>
    [HttpGet("nome/{nome}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<MembroDto>>>> GetByNome(string nome)
    {
        try
        {
            var tenantId = _currentUserService.GetTenantId(); 

            if (string.IsNullOrEmpty(tenantId))
                return BadRequest(ApiResponse<IEnumerable<MembroDto>>.ErrorResult("TenantId não encontrado"));

            var membros = await _membroService.GetByNomeAsync(nome, tenantId); 
            return Ok(membros);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<MembroDto>>.ErrorResult("Erro interno do servidor", ex.Message));
        }
    }

    /// <summary>
    /// Busca membros por texto (nome, CPF, telefone, cidade)
    /// </summary>
    [HttpGet("search")]
    public async Task<ActionResult<ApiResponse<IEnumerable<MembroDto>>>> Search(
        [FromQuery] string searchTerm,
        [FromQuery] bool includeDeleted = false)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return BadRequest(ApiResponse<IEnumerable<MembroDto>>.ErrorResult("Termo de busca é obrigatório"));

            var tenantId = _currentUserService.GetTenantId(); 

            if (string.IsNullOrEmpty(tenantId))
                return BadRequest(ApiResponse<IEnumerable<MembroDto>>.ErrorResult("TenantId não encontrado"));

            var membros = await _membroService.SearchAsync(searchTerm, tenantId, includeDeleted); 
            return Ok(membros);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<MembroDto>>.ErrorResult("Erro interno do servidor", ex.Message));
        }
    }

    /// <summary>
    /// Lista membros com filtros avançados
    /// </summary>
    [HttpGet("filtros")]
    public async Task<ActionResult<ApiResponse<IEnumerable<MembroDto>>>> GetWithFilters(
        [FromQuery] string? nome = null,
        [FromQuery] string? cpf = null,
        [FromQuery] string? cidade = null,
        [FromQuery] string? uf = null,
        [FromQuery] bool includeDeleted = false,
        [FromQuery] string orderBy = "Nome")
    {
        try
        {
            var tenantId = _currentUserService.GetTenantId(); 

            if (string.IsNullOrEmpty(tenantId))
                return BadRequest(ApiResponse<IEnumerable<MembroDto>>.ErrorResult("TenantId não encontrado"));

            var membros = await _membroService.GetWithFiltersAsync(
                tenantId, nome, cpf, cidade, uf, includeDeleted, orderBy);

            return Ok(membros);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<MembroDto>>.ErrorResult("Erro interno do servidor", ex.Message));
        }
    }

    /// <summary>
    /// Cria nova membro
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<MembroDto>>> Create([FromBody] CreateMembroDto createDto)
    {
        try
        {
            var validationResult = await _validatorCreate.ValidateAsync(createDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(ApiResponse<MembroDto>.ErrorResult("Dados inválidos", errors));
            }

            var tenantId = _currentUserService.GetTenantId(); 

            if (string.IsNullOrEmpty(tenantId))
                return BadRequest(ApiResponse<MembroDto>.ErrorResult("TenantId não encontrado"));

            var result = await _membroService.CreateAsync(createDto, tenantId); 

            if (result.Success)
                return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);

            return BadRequest(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<MembroDto>.ErrorResult("Erro interno do servidor", ex.Message));
        }
    }

    /// <summary>
    /// Atualiza membro
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<MembroDto>>> Update(int id, [FromBody] UpdateMembroDto updateDto)
    {
        try
        {
            var validationResult = await _validatorUpdate.ValidateAsync(updateDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(ApiResponse<MembroDto>.ErrorResult("Dados inválidos", errors));
            }

            var tenantId = _currentUserService.GetTenantId(); 

            if (string.IsNullOrEmpty(tenantId))
                return BadRequest(ApiResponse<MembroDto>.ErrorResult("TenantId não encontrado"));

            var result = await _membroService.UpdateAsync(id, updateDto, tenantId); 

            if (result.Success)
                return Ok(result);

            return NotFound(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<MembroDto>.ErrorResult("Erro interno do servidor", ex.Message));
        }
    }

    /// <summary>
    /// Exclui Membro (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        try
        {
            var tenantId = _currentUserService.GetTenantId(); 

            if (string.IsNullOrEmpty(tenantId))
                return BadRequest(ApiResponse<bool>.ErrorResult("TenantId não encontrado"));

            var result = await _membroService.DeleteAsync(id, tenantId); 

            if (result.Success)
                return Ok(result);

            return NotFound(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<bool>.ErrorResult("Erro interno do servidor", ex.Message));
        }
    }

    /// <summary>
    /// Verifica se CPF já existe no tenant
    /// </summary>
    [HttpGet("cpf-exists/{cpf}")]
    public async Task<ActionResult<ApiResponse<bool>>> CpfExists(string cpf, [FromQuery] int? excludeId = null)
    {
        try
        {
            var tenantId = _currentUserService.GetTenantId(); 

            if (string.IsNullOrEmpty(tenantId))
                return BadRequest(ApiResponse<bool>.ErrorResult("TenantId não encontrado"));

            var result = await _membroService.CpfExistsAsync(cpf, tenantId, excludeId); 
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<bool>.ErrorResult("Erro interno do servidor", ex.Message));
        }
    }
}

