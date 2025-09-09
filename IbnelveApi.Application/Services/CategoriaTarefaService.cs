using IbnelveApi.Application.Common;
using IbnelveApi.Application.DTOs;
using IbnelveApi.Application.Interfaces;
using IbnelveApi.Application.Mappings;
using IbnelveApi.Domain.Entities;
using IbnelveApi.Domain.Interfaces;

namespace IbnelveApi.Application.Services;

/// <summary>
/// Implementação do service para CategoriaTarefa
/// </summary>
public class CategoriaTarefaService : ICategoriaTarefaService
{
    private readonly ICategoriaTarefaRepository _repository;

    public CategoriaTarefaService(ICategoriaTarefaRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<IEnumerable<CategoriaTarefaDto>>> GetAllAsync(string tenantId, bool includeDeleted = false)
    {
        try
        {
            var categorias = await _repository.GetAllByTenantAsync(tenantId, includeDeleted);
            var categoriasDto = new List<CategoriaTarefaDto>();

            foreach (var categoria in categorias)
            {
                var dto = categoria.ToDto();
                dto.QuantidadeTarefas = await _repository.ContarTarefasAsync(categoria.Id);
                categoriasDto.Add(dto);
            }

            return ApiResponse<IEnumerable<CategoriaTarefaDto>>.SuccessResult(
                categoriasDto, 
                "Categorias listadas com sucesso"
            );
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<CategoriaTarefaDto>>.ErrorResult(
                "Erro ao listar categorias: " + ex.Message
            );
        }
    }

    public async Task<ApiResponse<IEnumerable<CategoriaTarefaSelectDto>>> GetAtivasForSelectAsync(string tenantId)
    {
        try
        {
            var categorias = await _repository.GetAtivasAsync(tenantId);
            var categoriasDto = categorias.Select(c => c.ToSelectDto()).ToList();

            return ApiResponse<IEnumerable<CategoriaTarefaSelectDto>>.SuccessResult(
                categoriasDto, 
                "Categorias ativas listadas com sucesso"
            );
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<CategoriaTarefaSelectDto>>.ErrorResult(
                "Erro ao listar categorias ativas: " + ex.Message
            );
        }
    }

    public async Task<ApiResponse<CategoriaTarefaDto>> GetByIdAsync(int id, string tenantId)
    {
        try
        {
            var categoria = await _repository.GetByIdAsync(id);

            if (categoria == null || categoria.TenantId != tenantId)
            {
                return ApiResponse<CategoriaTarefaDto>.ErrorResult("Categoria não encontrada");
            }

            var dto = categoria.ToDto();
           // dto.QuantidadeTarefas = await _repository.ContarTarefasAsync(categoria.Id);

            return ApiResponse<CategoriaTarefaDto>.SuccessResult(dto, "Categoria encontrada com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse<CategoriaTarefaDto>.ErrorResult(
                "Erro ao buscar categoria: " + ex.Message
            );
        }
    }

    public async Task<ApiResponse<CategoriaTarefaDto>> CreateAsync(CreateCategoriaTarefaDto dto, string tenantId, string userId)
    {
        try
        {
            // Verificar se nome já existe
            if (await NomeJaExisteAsync(dto.Nome, tenantId))
            {
                return ApiResponse<CategoriaTarefaDto>.ErrorResult("Já existe uma categoria com este nome");
            }

            var categoria = new CategoriaTarefa(dto.Nome, dto.Descricao, dto.Cor, tenantId, userId);
            
            await _repository.AddAsync(categoria);
            //await _repository.SaveChangesAsync();

            var categoriaDto = categoria.ToDto();
            categoriaDto.QuantidadeTarefas = 0;

            return ApiResponse<CategoriaTarefaDto>.SuccessResult(
                categoriaDto, 
                "Categoria criada com sucesso"
            );
        }
        catch (Exception ex)
        {
            return ApiResponse<CategoriaTarefaDto>.ErrorResult(
                "Erro ao criar categoria: " + ex.Message
            );
        }
    }

    public async Task<ApiResponse<CategoriaTarefaDto>> UpdateAsync(int id, UpdateCategoriaTarefaDto dto, string tenantId)
    {
        try
        {
            var categoria = await _repository.GetByIdAsync(id);

            if (categoria == null || categoria.TenantId != tenantId)
            {
                return ApiResponse<CategoriaTarefaDto>.ErrorResult("Categoria não encontrada");
            }

            // Verificar se nome já existe (excluindo a categoria atual)
            if (await NomeJaExisteAsync(dto.Nome, tenantId, id))
            {
                return ApiResponse<CategoriaTarefaDto>.ErrorResult("Já existe uma categoria com este nome");
            }

            categoria.AtualizarDados(dto.Nome, dto.Descricao, dto.Cor);
            
            await _repository.UpdateAsync(categoria);
            //await _repository.SaveChangesAsync();

            var categoriaDto = categoria.ToDto();
           // categoriaDto.QuantidadeTarefas = await _repository.ContarTarefasAsync(categoria.Id);

            return ApiResponse<CategoriaTarefaDto>.SuccessResult(
                categoriaDto, 
                "Categoria atualizada com sucesso"
            );
        }
        catch (Exception ex)
        {
            return ApiResponse<CategoriaTarefaDto>.ErrorResult(
                "Erro ao atualizar categoria: " + ex.Message
            );
        }
    }

    public async Task<ApiResponse<bool>> AtivarAsync(int id, string tenantId)
    {
        try
        {
            var categoria = await _repository.GetByIdAsync(id);

            if (categoria == null || categoria.TenantId != tenantId)
            {
                return ApiResponse<bool>.ErrorResult("Categoria não encontrada");
            }

            categoria.Ativar();
            await _repository.UpdateAsync(categoria);
            //await _repository.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResult(true, "Categoria ativada com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResult("Erro ao ativar categoria: " + ex.Message);
        }
    }

    public async Task<ApiResponse<bool>> DesativarAsync(int id, string tenantId)
    {
        try
        {
            var categoria = await _repository.GetByIdAsync(id);

            if (categoria == null || categoria.TenantId != tenantId)
            {
                return ApiResponse<bool>.ErrorResult("Categoria não encontrada");
            }

            categoria.Desativar();
            await _repository.UpdateAsync(categoria);
            //await _repository.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResult(true, "Categoria desativada com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResult("Erro ao desativar categoria: " + ex.Message);
        }
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int id, string tenantId)
    {
        try
        {
            var categoria = await _repository.GetByIdAsync(id);

            if (categoria == null || categoria.TenantId != tenantId)
            {
                return ApiResponse<bool>.ErrorResult("Categoria não encontrada");
            }

            //// Verificar se está sendo usada por alguma tarefa
            //if (await _repository.EstaSendoUsadaAsync(id))
            //{
            //    return ApiResponse<bool>.ErrorResult(
            //        "Não é possível excluir a categoria pois ela está sendo usada por tarefas"
            //    );
            //}

            await _repository.DeleteAsync(id);
            //await _repository.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResult(true, "Categoria excluída com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResult("Erro ao excluir categoria: " + ex.Message);
        }
    }

    public async Task<bool> NomeJaExisteAsync(string nome, string tenantId, int? excludeId = null)
    {
        return await _repository.ExisteNomeAsync(nome, tenantId, excludeId);
    }
}

