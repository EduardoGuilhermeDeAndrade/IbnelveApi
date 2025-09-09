using IbnelveApi.Application.Common;
using IbnelveApi.Application.DTOs;
using IbnelveApi.Application.Interfaces;
using IbnelveApi.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IbnelveApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TarefaController : ControllerBase
{
    private readonly ITarefaService _tarefaService;
    private readonly ICurrentUserService _currentUserService; 

    public TarefaController(ITarefaService tarefaService, ICurrentUserService currentUserService) 
    {
        _tarefaService = tarefaService;
        _currentUserService = currentUserService; 
    }

    /// <summary>
    /// Lista todas as tarefas do usuário
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<TarefaDto>>>> GetAll(
        [FromQuery] bool includeDeleted = false)
    {
        try
        {
            var userId = _currentUserService.GetUserId();    
            var tenantId = _currentUserService.GetTenantId(); 

            if (string.IsNullOrEmpty(tenantId))
                return BadRequest(ApiResponse<IEnumerable<TarefaDto>>.ErrorResult("TenantId não encontrado"));

            if (string.IsNullOrEmpty(userId))
                return BadRequest(ApiResponse<IEnumerable<TarefaDto>>.ErrorResult("UserId não encontrado"));

            var result = await _tarefaService.GetAllAsync(userId, tenantId, includeDeleted); 

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<TarefaDto>>.ErrorResult("Erro interno do servidor", ex.Message));
        }
    }

    /// <summary>
    /// Lista tarefas com filtros avançados
    /// </summary>
    [HttpGet("filtros")]
    public async Task<ActionResult<ApiResponse<IEnumerable<TarefaDto>>>> GetWithFilters(
        [FromQuery] StatusTarefa? status = null,
        [FromQuery] PrioridadeTarefa? prioridade = null,
        [FromQuery] int? categoriaId = 0,
        [FromQuery] DateTime? dataVencimentoInicio = null,
        [FromQuery] DateTime? dataVencimentoFim = null,
        [FromQuery] bool includeDeleted = false,
        [FromQuery] string orderBy = "CreatedAt",
        [FromQuery] string? searchTerm = null)
    {
        try
        {
            var userId = _currentUserService.GetUserId();    
            var tenantId = _currentUserService.GetTenantId(); 

            if (string.IsNullOrEmpty(tenantId))
                return BadRequest(ApiResponse<IEnumerable<TarefaDto>>.ErrorResult("TenantId não encontrado"));

            if (string.IsNullOrEmpty(userId))
                return BadRequest(ApiResponse<IEnumerable<TarefaDto>>.ErrorResult("UserId não encontrado"));

            var filtro = new TarefaFiltroDto
            {
                Status = status,
                Prioridade = prioridade, 
                CategoriaId = categoriaId,
                DataVencimentoInicio = dataVencimentoInicio,
                DataVencimentoFim = dataVencimentoFim,
                IncludeDeleted = includeDeleted,
                OrderBy = orderBy,
                SearchTerm = searchTerm
            };

            var result = await _tarefaService.GetWithFiltersAsync(filtro, userId, tenantId); 

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<TarefaDto>>.ErrorResult("Erro interno do servidor", ex.Message));
        }
    }

    /// <summary>
    /// Busca tarefa por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<TarefaDto>>> GetById(int id)
    {
        try
        {
            var userId = _currentUserService.GetUserId();    
            var tenantId = _currentUserService.GetTenantId(); 

            if (string.IsNullOrEmpty(tenantId))
                return BadRequest(ApiResponse<TarefaDto>.ErrorResult("TenantId não encontrado"));

            if (string.IsNullOrEmpty(userId))
                return BadRequest(ApiResponse<TarefaDto>.ErrorResult("UserId não encontrado"));

            var result = await _tarefaService.GetByIdAsync(id, userId, tenantId); 

            if (result.Success)
                return Ok(result);

            return NotFound(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<TarefaDto>.ErrorResult("Erro interno do servidor", ex.Message));
        }
    }

    /// <summary>
    /// Busca tarefas por texto (título ou descrição)
    /// </summary>
    [HttpGet("search")]
    public async Task<ActionResult<ApiResponse<IEnumerable<TarefaDto>>>> Search(
        [FromQuery] string searchTerm,
        [FromQuery] bool includeDeleted = false)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return BadRequest(ApiResponse<IEnumerable<TarefaDto>>.ErrorResult("Termo de busca é obrigatório"));

            var userId = _currentUserService.GetUserId();   
            var tenantId = _currentUserService.GetTenantId(); 

            if (string.IsNullOrEmpty(tenantId))
                return BadRequest(ApiResponse<IEnumerable<TarefaDto>>.ErrorResult("TenantId não encontrado"));

            if (string.IsNullOrEmpty(userId))
                return BadRequest(ApiResponse<IEnumerable<TarefaDto>>.ErrorResult("UserId não encontrado"));

            var result = await _tarefaService.SearchAsync(searchTerm, userId, tenantId, includeDeleted); 

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<TarefaDto>>.ErrorResult("Erro interno do servidor", ex.Message));
        }
    }

    /// <summary>
    /// Lista tarefas por status
    /// </summary>
    [HttpGet("status/{status}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<TarefaDto>>>> GetByStatus(
        StatusTarefa status,
        [FromQuery] bool includeDeleted = false)
    {
        try
        {
            var userId = _currentUserService.GetUserId();    
            var tenantId = _currentUserService.GetTenantId(); 

            if (string.IsNullOrEmpty(tenantId))
                return BadRequest(ApiResponse<IEnumerable<TarefaDto>>.ErrorResult("TenantId não encontrado"));

            if (string.IsNullOrEmpty(userId))
                return BadRequest(ApiResponse<IEnumerable<TarefaDto>>.ErrorResult("UserId não encontrado"));

            var result = await _tarefaService.GetByStatusAsync(status, userId, tenantId, includeDeleted); 

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<TarefaDto>>.ErrorResult("Erro interno do servidor", ex.Message));
        }
    }

    /// <summary>
    /// Lista tarefas vencidas
    /// </summary>
    [HttpGet("vencidas")]
    public async Task<ActionResult<ApiResponse<IEnumerable<TarefaDto>>>> GetVencidas(
        [FromQuery] bool includeDeleted = false)
    {
        try
        {
            var userId = _currentUserService.GetUserId();    
            var tenantId = _currentUserService.GetTenantId(); 

            if (string.IsNullOrEmpty(tenantId))
                return BadRequest(ApiResponse<IEnumerable<TarefaDto>>.ErrorResult("TenantId não encontrado"));

            if (string.IsNullOrEmpty(userId))
                return BadRequest(ApiResponse<IEnumerable<TarefaDto>>.ErrorResult("UserId não encontrado"));

            var result = await _tarefaService.GetVencidasAsync(userId, tenantId, includeDeleted); 

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<TarefaDto>>.ErrorResult("Erro interno do servidor", ex.Message));
        }
    }

    /// <summary>
    /// Lista tarefas concluídas
    /// </summary>
    [HttpGet("concluidas")]
    public async Task<ActionResult<ApiResponse<IEnumerable<TarefaDto>>>> GetConcluidas(
        [FromQuery] bool includeDeleted = false)
    {
        try
        {
            var userId = _currentUserService.GetUserId();    
            var tenantId = _currentUserService.GetTenantId(); 

            if (string.IsNullOrEmpty(tenantId))
                return BadRequest(ApiResponse<IEnumerable<TarefaDto>>.ErrorResult("TenantId não encontrado"));

            if (string.IsNullOrEmpty(userId))
                return BadRequest(ApiResponse<IEnumerable<TarefaDto>>.ErrorResult("UserId não encontrado"));

            var result = await _tarefaService.GetConcluidasAsync(userId, tenantId, includeDeleted); 

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<TarefaDto>>.ErrorResult("Erro interno do servidor", ex.Message));
        }
    }

    /// <summary>
    /// Cria nova tarefa
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<TarefaDto>>> Create([FromBody] CreateTarefaDto createDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(ApiResponse<TarefaDto>.ErrorResult("Dados inválidos", errors));
            }

            var userId = _currentUserService.GetUserId();    
            var tenantId = _currentUserService.GetTenantId(); 

            if (string.IsNullOrEmpty(tenantId))
                return BadRequest(ApiResponse<TarefaDto>.ErrorResult("TenantId não encontrado"));

            if (string.IsNullOrEmpty(userId))
                return BadRequest(ApiResponse<TarefaDto>.ErrorResult("UserId não encontrado"));

            var result = await _tarefaService.CreateAsync(createDto, userId, tenantId); 

            if (result.Success)
                return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);

            return BadRequest(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<TarefaDto>.ErrorResult("Erro interno do servidor", ex.Message));
        }
    }

    /// <summary>
    /// Atualiza tarefa
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<TarefaDto>>> Update(int id, [FromBody] UpdateTarefaDto updateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(ApiResponse<TarefaDto>.ErrorResult("Dados inválidos", errors));
            }

            var userId = _currentUserService.GetUserId();    
            var tenantId = _currentUserService.GetTenantId(); 

            if (string.IsNullOrEmpty(tenantId))
                return BadRequest(ApiResponse<TarefaDto>.ErrorResult("TenantId não encontrado"));

            if (string.IsNullOrEmpty(userId))
                return BadRequest(ApiResponse<TarefaDto>.ErrorResult("UserId não encontrado"));

            var result = await _tarefaService.UpdateAsync(id, updateDto, userId, tenantId); 

            if (result.Success)
                return Ok(result);

            return NotFound(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<TarefaDto>.ErrorResult("Erro interno do servidor", ex.Message));
        }
    }

    /// <summary>
    /// Altera status da tarefa
    /// </summary>
    [HttpPatch("{id}/status")]
    public async Task<ActionResult<ApiResponse<TarefaDto>>> UpdateStatus(int id, [FromBody] UpdateStatusTarefaDto statusDto)
    {
        try
        {
            var userId = _currentUserService.GetUserId();    
            var tenantId = _currentUserService.GetTenantId(); 

            if (string.IsNullOrEmpty(tenantId))
                return BadRequest(ApiResponse<TarefaDto>.ErrorResult("TenantId não encontrado"));

            if (string.IsNullOrEmpty(userId))
                return BadRequest(ApiResponse<TarefaDto>.ErrorResult("UserId não encontrado"));

            var result = await _tarefaService.UpdateStatusAsync(id, statusDto.Status, userId, tenantId);

            if (result.Success)
                return Ok(result);

            return NotFound(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<TarefaDto>.ErrorResult("Erro interno do servidor", ex.Message));
        }
    }

    /// <summary>
    /// Marca tarefa como concluída
    /// </summary>
    [HttpPatch("{id}/concluir")]
    public async Task<ActionResult<ApiResponse<TarefaDto>>> MarcarComoConcluida(int id)
    {
        try
        {
            var userId = _currentUserService.GetUserId();    
            var tenantId = _currentUserService.GetTenantId(); 

            if (string.IsNullOrEmpty(tenantId))
                return BadRequest(ApiResponse<TarefaDto>.ErrorResult("TenantId não encontrado"));

            if (string.IsNullOrEmpty(userId))
                return BadRequest(ApiResponse<TarefaDto>.ErrorResult("UserId não encontrado"));

            var result = await _tarefaService.MarcarComoConcluidaAsync(id, userId, tenantId); 

            if (result.Success)
                return Ok(result);

            return NotFound(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<TarefaDto>.ErrorResult("Erro interno do servidor", ex.Message));
        }
    }

    /// <summary>
    /// Marca tarefa como pendente
    /// </summary>
    [HttpPatch("{id}/pendente")]
    public async Task<ActionResult<ApiResponse<TarefaDto>>> MarcarComoPendente(int id)
    {
        try
        {
            var userId = _currentUserService.GetUserId();    
            var tenantId = _currentUserService.GetTenantId(); 

            if (string.IsNullOrEmpty(tenantId))
                return BadRequest(ApiResponse<TarefaDto>.ErrorResult("TenantId não encontrado"));

            if (string.IsNullOrEmpty(userId))
                return BadRequest(ApiResponse<TarefaDto>.ErrorResult("UserId não encontrado"));

            var result = await _tarefaService.MarcarComoPendenteAsync(id, userId, tenantId); 

            if (result.Success)
                return Ok(result);

            return NotFound(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<TarefaDto>.ErrorResult("Erro interno do servidor", ex.Message));
        }
    }

    /// <summary>
    /// Exclui tarefa (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        try
        {
            var userId = _currentUserService.GetUserId();    
            var tenantId = _currentUserService.GetTenantId(); 

            if (string.IsNullOrEmpty(tenantId))
                return BadRequest(ApiResponse<bool>.ErrorResult("TenantId não encontrado"));

            if (string.IsNullOrEmpty(userId))
                return BadRequest(ApiResponse<bool>.ErrorResult("UserId não encontrado"));

            var result = await _tarefaService.DeleteAsync(id, userId, tenantId); 

            if (result.Success)
                return Ok(result);

            return NotFound(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<bool>.ErrorResult("Erro interno do servidor", ex.Message));
        }
    }
}

