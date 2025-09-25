using IbnelveApi.Application.DTOs.Categoria;
using IbnelveApi.Application.DTOs.CategoriaTarefa;
using IbnelveApi.Domain.Entities;

namespace IbnelveApi.Application.Mappings;

/// <summary>
/// Extensões para mapeamento entre Categoria e DTOs
/// </summary>
public static class CategoriaMapping
{
    /// <summary>
    /// Converte Categoria para CategoriaDto
    /// </summary>
    public static CategoriaUtensilioDto ToDto(this CategoriaUtensilio categoria)
    {
        return new CategoriaUtensilioDto
        {
            Id = categoria.Id,
            Nome = categoria.Nome,
            Descricao = categoria.Descricao,
            Ativa = categoria.Ativa,
            TenantId = categoria.TenantId,
            CreatedAt = categoria.CreatedAt,
            UpdatedAt = categoria.UpdatedAt,
        };
    }

    /// <summary>
    /// Converte CategoriaTarefa para CategoriaTarefaSelectDto
    /// </summary>
    public static CategoriaUtensilioSelectDto ToSelectDto(this CategoriaUtensilio categoria)
    {
        return new CategoriaUtensilioSelectDto
        {
            Id = categoria.Id,
            Nome = categoria.Nome,
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

