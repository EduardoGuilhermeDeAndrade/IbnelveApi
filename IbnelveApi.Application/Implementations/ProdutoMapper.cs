using IbnelveApi.Application.DTOs;
using IbnelveApi.Application.Mappers.Interfaces;
using IbnelveApi.Domain.Entities;

namespace IbnelveApi.Application.Mappers.Implementations;

/// <summary>
/// Implementação do mapper para Produto
/// </summary>
public class ProdutoMapper : IProdutoMapper
{
    /// <summary>
    /// Mapeia Produto para ProdutoResponseDto
    /// </summary>
    /// <param name="source">Produto de origem</param>
    /// <returns>DTO de resposta</returns>
    /// <exception cref="ArgumentNullException">Quando source é null</exception>
    public ProdutoResponseDto Map(Produto source)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source), "Produto não pode ser null");

        return MapToResponse(source);
    }

    /// <summary>
    /// Mapeia ProdutoCreateDto para Produto
    /// </summary>
    /// <param name="source">DTO de criação</param>
    /// <returns>Produto para criação</returns>
    /// <exception cref="ArgumentNullException">Quando source é null</exception>
    public Produto Map(ProdutoCreateDto source)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source), "ProdutoCreateDto não pode ser null");

        return new Produto
        {
            Nome = source.Nome?.Trim() ?? string.Empty,
            Preco = source.Preco,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null,
            IsDeleted = false
            // TenantId será definido no service usando MapToEntity
        };
    }

    /// <summary>
    /// Mapeia coleção de Produto para coleção de ProdutoResponseDto
    /// </summary>
    /// <param name="source">Coleção de produtos</param>
    /// <returns>Coleção de DTOs de resposta</returns>
    /// <exception cref="ArgumentNullException">Quando source é null</exception>
    public IEnumerable<ProdutoResponseDto> MapCollection(IEnumerable<Produto> source)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source), "Coleção de produtos não pode ser null");

        return source.Select(MapToResponse).ToList();
    }

    /// <summary>
    /// Mapeia ProdutoUpdateDto para Produto existente
    /// </summary>
    /// <param name="updateDto">DTO com dados para atualização</param>
    /// <param name="existingProduto">Produto existente</param>
    /// <returns>Produto atualizado</returns>
    /// <exception cref="ArgumentNullException">Quando algum parâmetro é null</exception>
    public Produto MapUpdate(ProdutoUpdateDto updateDto, Produto existingProduto)
    {
        if (updateDto == null)
            throw new ArgumentNullException(nameof(updateDto), "ProdutoUpdateDto não pode ser null");

        if (existingProduto == null)
            throw new ArgumentNullException(nameof(existingProduto), "Produto existente não pode ser null");

        // Atualiza apenas os campos modificáveis
        existingProduto.Nome = updateDto.Nome?.Trim() ?? existingProduto.Nome;
        existingProduto.Preco = updateDto.Preco;
        existingProduto.UpdatedAt = DateTime.UtcNow;

        // Preserva campos não modificáveis
        // Id, TenantId, CreatedAt, IsDeleted permanecem inalterados

        return existingProduto;
    }

    /// <summary>
    /// Mapeia Produto para ProdutoResponseDto com validações
    /// </summary>
    /// <param name="produto">Produto de origem</param>
    /// <returns>DTO de resposta</returns>
    public ProdutoResponseDto MapToResponse(Produto produto)
    {
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
    /// Mapeia ProdutoCreateDto para Produto com configurações iniciais
    /// </summary>
    /// <param name="createDto">DTO de criação</param>
    /// <param name="tenantId">ID do tenant</param>
    /// <returns>Produto configurado</returns>
    /// <exception cref="ArgumentNullException">Quando createDto é null</exception>
    /// <exception cref="ArgumentException">Quando tenantId é null ou vazio</exception>
    public Produto MapToEntity(ProdutoCreateDto createDto, string tenantId)
    {
        if (createDto == null)
            throw new ArgumentNullException(nameof(createDto), "ProdutoCreateDto não pode ser null");

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
}

