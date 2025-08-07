// IbnelveApi.Api/Controllers/PessoaController.cs
using IbnelveApi.Application.Common;
using IbnelveApi.Application.DTOs;
using IbnelveApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IbnelveApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PessoaController : ControllerBase
{
    private readonly IPessoaService _pessoaService;

    public PessoaController(IPessoaService pessoaService)
    {
        _pessoaService = pessoaService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<PessoaDto>>>> GetAll()
    {
        // Não precisa mais passar tenantId - é aplicado automaticamente
        var pessoas = await _pessoaService.GetAllAsync();
        return Ok(pessoas);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<PessoaDto>>> GetById(int id)
    {
        // Não precisa mais passar tenantId - é aplicado automaticamente
        var pessoa = await _pessoaService.GetByIdAsync(id);

        if (pessoa == null)
            return NotFound(ApiResponse<PessoaDto>.ErrorResult("Pessoa não encontrada"));

        return Ok((pessoa));
    }

    [HttpGet("cpf/{cpf}")]
    public async Task<ActionResult<ApiResponse<PessoaDto>>> GetByCpf(string cpf)
    {
        // Não precisa mais passar tenantId - é aplicado automaticamente
        var pessoa = await _pessoaService.GetByCpfAsync(cpf);

        if (pessoa == null)
            return NotFound(ApiResponse<PessoaDto>.ErrorResult("Pessoa não encontrada"));

        return Ok(pessoa);
    }

    // Outros métodos seguem o mesmo padrão...
}

//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;
//using IbnelveApi.Application.Common;
//using IbnelveApi.Application.DTOs;
//using IbnelveApi.Application.Interfaces;

//namespace IbnelveApi.Api.Controllers;

//[ApiController]
//[Route("api/[controller]")]
//[Authorize]
//public class PessoaController : ControllerBase
//{
//    private readonly IPessoaService _pessoaService;

//    public PessoaController(IPessoaService pessoaService)
//    {
//        _pessoaService = pessoaService;
//    }

//    private string GetTenantId()
//    {
//        return User.FindFirst("TenantId")?.Value ?? string.Empty;
//    }

//    [HttpGet]
//    public async Task<ActionResult<ApiResponse<IEnumerable<PessoaDto>>>> GetAll(
//        [FromQuery] bool includeDeleted = false,
//        [FromQuery] string? tenantId = null)
//    {
//        try
//        {
//            // Use TenantId from token, but allow override for admin scenarios
//            var currentTenantId = string.IsNullOrEmpty(tenantId) ? GetTenantId() : tenantId;

//            if (string.IsNullOrEmpty(currentTenantId))
//                return BadRequest(ApiResponse<IEnumerable<PessoaDto>>.ErrorResult("TenantId não encontrado"));

//            var result = await _pessoaService.GetAllAsync(currentTenantId, includeDeleted);

//            if (result.Success)
//                return Ok(result);

//            return BadRequest(result);
//        }
//        catch (Exception ex)
//        {
//            return StatusCode(500, ApiResponse<IEnumerable<PessoaDto>>.ErrorResult("Erro interno do servidor", ex.Message));
//        }
//    }

//    [HttpGet("{id}")]
//    public async Task<ActionResult<ApiResponse<PessoaDto>>> GetById(int id)
//    {
//        try
//        {
//            var tenantId = GetTenantId();
//            if (string.IsNullOrEmpty(tenantId))
//                return BadRequest(ApiResponse<PessoaDto>.ErrorResult("TenantId não encontrado"));

//            var result = await _pessoaService.GetByIdAsync(id, tenantId);

//            if (result.Success)
//                return Ok(result);

//            return NotFound(result);
//        }
//        catch (Exception ex)
//        {
//            return StatusCode(500, ApiResponse<PessoaDto>.ErrorResult("Erro interno do servidor", ex.Message));
//        }
//    }

//    [HttpGet("cpf/{cpf}")]
//    public async Task<ActionResult<ApiResponse<PessoaDto>>> GetByCpf(string cpf)
//    {
//        try
//        {
//            var tenantId = GetTenantId();
//            if (string.IsNullOrEmpty(tenantId))
//                return BadRequest(ApiResponse<PessoaDto>.ErrorResult("TenantId não encontrado"));

//            var result = await _pessoaService.GetByCpfAsync(cpf, tenantId);

//            if (result.Success)
//                return Ok(result);

//            return NotFound(result);
//        }
//        catch (Exception ex)
//        {
//            return StatusCode(500, ApiResponse<PessoaDto>.ErrorResult("Erro interno do servidor", ex.Message));
//        }
//    }

//    [HttpGet("search")]
//    public async Task<ActionResult<ApiResponse<IEnumerable<PessoaDto>>>> SearchByNome(
//        [FromQuery] string nome,
//        [FromQuery] bool includeDeleted = false)
//    {
//        try
//        {
//            if (string.IsNullOrWhiteSpace(nome))
//                return BadRequest(ApiResponse<IEnumerable<PessoaDto>>.ErrorResult("Nome é obrigatório para busca"));

//            var tenantId = GetTenantId();
//            if (string.IsNullOrEmpty(tenantId))
//                return BadRequest(ApiResponse<IEnumerable<PessoaDto>>.ErrorResult("TenantId não encontrado"));

//            var result = await _pessoaService.GetByNomeAsync(nome, tenantId, includeDeleted);

//            if (result.Success)
//                return Ok(result);

//            return BadRequest(result);
//        }
//        catch (Exception ex)
//        {
//            return StatusCode(500, ApiResponse<IEnumerable<PessoaDto>>.ErrorResult("Erro interno do servidor", ex.Message));
//        }
//    }

//    [HttpPost]
//    public async Task<ActionResult<ApiResponse<PessoaDto>>> Create([FromBody] CreatePessoaDto createDto)
//    {
//        try
//        {
//            if (!ModelState.IsValid)
//            {
//                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
//                return BadRequest(ApiResponse<PessoaDto>.ErrorResult("Dados inválidos", errors));
//            }

//            var tenantId = GetTenantId();
//            if (string.IsNullOrEmpty(tenantId))
//                return BadRequest(ApiResponse<PessoaDto>.ErrorResult("TenantId não encontrado"));

//            var result = await _pessoaService.CreateAsync(createDto, tenantId);

//            if (result.Success)
//                return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);

//            return BadRequest(result);
//        }
//        catch (Exception ex)
//        {
//            return StatusCode(500, ApiResponse<PessoaDto>.ErrorResult("Erro interno do servidor", ex.Message));
//        }
//    }

//    [HttpPut("{id}")]
//    public async Task<ActionResult<ApiResponse<PessoaDto>>> Update(int id, [FromBody] UpdatePessoaDto updateDto)
//    {
//        try
//        {
//            if (!ModelState.IsValid)
//            {
//                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
//                return BadRequest(ApiResponse<PessoaDto>.ErrorResult("Dados inválidos", errors));
//            }

//            var tenantId = GetTenantId();
//            if (string.IsNullOrEmpty(tenantId))
//                return BadRequest(ApiResponse<PessoaDto>.ErrorResult("TenantId não encontrado"));

//            var result = await _pessoaService.UpdateAsync(id, updateDto, tenantId);

//            if (result.Success)
//                return Ok(result);

//            return NotFound(result);
//        }
//        catch (Exception ex)
//        {
//            return StatusCode(500, ApiResponse<PessoaDto>.ErrorResult("Erro interno do servidor", ex.Message));
//        }
//    }

//    [HttpDelete("{id}")]
//    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
//    {
//        try
//        {
//            var tenantId = GetTenantId();
//            if (string.IsNullOrEmpty(tenantId))
//                return BadRequest(ApiResponse<bool>.ErrorResult("TenantId não encontrado"));

//            var result = await _pessoaService.DeleteAsync(id, tenantId);

//            if (result.Success)
//                return Ok(result);

//            return NotFound(result);
//        }
//        catch (Exception ex)
//        {
//            return StatusCode(500, ApiResponse<bool>.ErrorResult("Erro interno do servidor", ex.Message));
//        }
//    }
//}

