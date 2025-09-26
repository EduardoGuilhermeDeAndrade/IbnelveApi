using IbnelveApi.Application.DTOs.Categoria;
using IbnelveApi.Domain.Entities;

namespace IbnelveApi.Application.Mappings;

/// <summary>
/// Extensões para mapeamento entre CategoriaUtensilio e DTOs
/// </summary>
public static class CategoriaUtensilioMapping
{
    /// <summary>
    /// Converte CategoriaUtensilio para CategoriaUtensilioDto
    /// </summary>
    public static CategoriaUtensilioDto ToDto(this CategoriaUtensilio categoria)
    {
        return new CategoriaUtensilioDto
        {
            Id = categoria.Id,
            Nome = categoria.Nome,
            Descricao = categoria.Descricao,
            Cor = null, // CategoriaUtensilio doesn't have Cor property
            Ativa = categoria.Ativa,
            TenantId = categoria.TenantId,
            CreatedAt = categoria.CreatedAt,
            UpdatedAt = categoria.UpdatedAt
        };
    }

    /// <summary>
    /// Converte CreateCategoriaUtensilioDto para CategoriaUtensilio
    /// </summary>
    public static CategoriaUtensilio ToEntity(this CreateCategoriaUtensilioDto dto, string tenantId, string userId)
    {
        return new CategoriaUtensilio(dto.Nome, dto.Descricao, tenantId, userId);
    }
}