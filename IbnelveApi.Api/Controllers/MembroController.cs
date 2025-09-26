using IbnelveApi.Application.Common;
using IbnelveApi.Application.DTOs.Membro;
using IbnelveApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IbnelveApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MembroController : ControllerBase
{
    private readonly IMembroService _membroService;
    private readonly string _tenantId;

    public MembroController(
        IMembroService membroService,
        ICurrentUserService currentUserService)
    {
        _membroService = membroService;
        _tenantId = currentUserService.GetTenantId();
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
            if (string.IsNullOrEmpty(_tenantId))
                return BadRequest(ApiResponse<IEnumerable<MembroDto>>.ErrorResult("TenantId não encontrado"));

            var membros = await _membroService.GetAllAsync(_tenantId, includeDeleted);
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
            if (string.IsNullOrEmpty(_tenantId))
                return BadRequest(ApiResponse<MembroDto>.ErrorResult("TenantId não encontrado"));

            var membro = await _membroService.GetByIdAsync(id, _tenantId);

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
            if (string.IsNullOrEmpty(_tenantId))
                return BadRequest(ApiResponse<MembroDto>.ErrorResult("TenantId não encontrado"));

            var membro = await _membroService.GetByCpfAsync(cpf, _tenantId);

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
            if (string.IsNullOrEmpty(_tenantId))
                return BadRequest(ApiResponse<IEnumerable<MembroDto>>.ErrorResult("TenantId não encontrado"));

            var membros = await _membroService.GetByNomeAsync(nome, _tenantId);
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

            if (string.IsNullOrEmpty(_tenantId))
                return BadRequest(ApiResponse<IEnumerable<MembroDto>>.ErrorResult("TenantId não encontrado"));

            var membros = await _membroService.SearchAsync(searchTerm, _tenantId, includeDeleted);
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
            if (string.IsNullOrEmpty(_tenantId))
                return BadRequest(ApiResponse<IEnumerable<MembroDto>>.ErrorResult("TenantId não encontrado"));

            var membros = await _membroService.GetWithFiltersAsync(
                _tenantId, nome, cpf, cidade, uf, includeDeleted, orderBy);

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
            if (string.IsNullOrEmpty(_tenantId))
                return BadRequest(ApiResponse<MembroDto>.ErrorResult("TenantId não encontrado"));

            var result = await _membroService.CreateAsync(createDto, _tenantId);

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
            if (string.IsNullOrEmpty(_tenantId))
                return BadRequest(ApiResponse<MembroDto>.ErrorResult("TenantId não encontrado"));

            var result = await _membroService.UpdateAsync(id, updateDto, _tenantId);

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
            if (string.IsNullOrEmpty(_tenantId))
                return BadRequest(ApiResponse<bool>.ErrorResult("TenantId não encontrado"));

            var result = await _membroService.DeleteAsync(id, _tenantId);

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
            if (string.IsNullOrEmpty(_tenantId))
                return BadRequest(ApiResponse<bool>.ErrorResult("TenantId não encontrado"));

            var result = await _membroService.CpfExistsAsync(cpf, _tenantId, excludeId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<bool>.ErrorResult("Erro interno do servidor", ex.Message));
        }
    }
}

