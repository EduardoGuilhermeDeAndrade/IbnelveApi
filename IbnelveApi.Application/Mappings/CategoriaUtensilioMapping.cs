using IbnelveApi.Application.DTOs.CategoriaTarefa;
using IbnelveApi.Domain.Entities;

namespace IbnelveApi.Application.Mappings;

/// <summary>
/// Extensões para mapeamento entre CategoriaTarefa e DTOs
/// </summary>
public static class CategoriaTarefaMapping
{
    /// <summary>
    /// Converte CategoriaTarefa para CategoriaDto
    /// </summary>
    public static CategoriaTarefaDto ToDto(this CategoriaTarefa categoria)
    {
        return new CategoriaTarefaDto
        {
            Id = categoria.Id,
            Nome = categoria.Nome,
            Descricao = categoria.Descricao,
            Cor = categoria.Cor,
            Ativa = categoria.Ativa,
            TenantId = categoria.TenantId,
            CreatedAt = categoria.CreatedAt,
            UpdatedAt = categoria.UpdatedAt,
            QuantidadeTarefas = 0 // Será preenchido pelo service
        };
    }

    /// <summary>
    /// Converte CategoriaTarefa para CategoriaTarefaSelectDto
    /// </summary>
    public static CategoriaTarefaSelectDto ToSelectDto(this CategoriaTarefa categoria)
    {
        return new CategoriaTarefaSelectDto
        {
            Id = categoria.Id,
            Nome = categoria.Nome,
            Cor = categoria.Cor
        };
    }

    /// <summary>
    /// Converte CreateCategoriaTarefaDto para CategoriaTarefa
    /// </summary>
    public static CategoriaTarefa ToEntity(this CreateCategoriaTarefaDto dto, string tenantId, string userId)
    {
        return new CategoriaTarefa(dto.Nome, dto.Descricao, dto.Cor, tenantId, userId);
    }
}

