using IbnelveApi.Application.DTOs;
using IbnelveApi.Domain.Entities;

namespace IbnelveApi.Application.Mappers.Extensions;

/// <summary>
/// Métodos de extensão para facilitar o mapeamento
/// </summary>
public static class MappingExtensions
{
    /// <summary>
    /// Converte Produto para ProdutoResponseDto
    /// </summary>
    /// <param name="produto">Produto de origem</param>
    /// <returns>DTO de resposta</returns>
    public static ProdutoResponseDto ToResponseDto(this Produto produto)
    {
        if (produto == null)
            return null;

        return new ProdutoResponseDto
        {
            Id = produto.Id,
            Nome = produto.Nome ?? string.Empty,
            Preco = produto.Preco,
            TenantId = produto.TenantId ?? string.Empty,
            CreatedAt = produto.CreatedAt,
            UpdatedAt = produto.UpdatedAt
        };
    }

    /// <summary>
    /// Converte coleção de Produto para coleção de ProdutoResponseDto
    /// </summary>
    /// <param name="produtos">Coleção de produtos</param>
    /// <returns>Coleção de DTOs de resposta</returns>
    public static IEnumerable<ProdutoResponseDto> ToResponseDtos(this IEnumerable<Produto> produtos)
    {
        if (produtos == null)
            return Enumerable.Empty<ProdutoResponseDto>();

        return produtos.Select(p => p.ToResponseDto()).Where(dto => dto != null);
    }

    /// <summary>
    /// Converte ProdutoCreateDto para Produto
    /// </summary>
    /// <param name="createDto">DTO de criação</param>
    /// <param name="tenantId">ID do tenant</param>
    /// <returns>Produto para criação</returns>
    public static Produto ToEntity(this ProdutoCreateDto createDto, string tenantId)
    {
        if (createDto == null)
            return null;

        if (string.IsNullOrWhiteSpace(tenantId))
            throw new ArgumentException("TenantId não pode ser null ou vazio", nameof(tenantId));

        return new Produto
        {
            Nome = createDto.Nome?.Trim() ?? string.Empty,
            Preco = createDto.Preco,
            TenantId = tenantId.Trim(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null,
            IsDeleted = false
        };
    }

    /// <summary>
    /// Atualiza Produto existente com dados do ProdutoUpdateDto
    /// </summary>
    /// <param name="produto">Produto existente</param>
    /// <param name="updateDto">DTO com dados para atualização</param>
    /// <returns>Produto atualizado</returns>
    public static Produto UpdateFrom(this Produto produto, ProdutoUpdateDto updateDto)
    {
        if (produto == null || updateDto == null)
            return produto;

        produto.Nome = updateDto.Nome?.Trim() ?? produto.Nome;
        produto.Preco = updateDto.Preco;
        produto.UpdatedAt = DateTime.UtcNow;

        return produto;
    }

    /// <summary>
    /// Verifica se o produto é válido para mapeamento
    /// </summary>
    /// <param name="produto">Produto a ser verificado</param>
    /// <returns>True se válido, false caso contrário</returns>
    public static bool IsValidForMapping(this Produto produto)
    {
        return produto != null &&
               !produto.IsDeleted &&
               !string.IsNullOrWhiteSpace(produto.Nome);
    }

    /// <summary>
    /// Filtra produtos válidos para mapeamento
    /// </summary>
    /// <param name="produtos">Coleção de produtos</param>
    /// <returns>Produtos válidos</returns>
    public static IEnumerable<Produto> ValidForMapping(this IEnumerable<Produto> produtos)
    {
        return produtos?.Where(p => p.IsValidForMapping()) ?? Enumerable.Empty<Produto>();
    }
}

