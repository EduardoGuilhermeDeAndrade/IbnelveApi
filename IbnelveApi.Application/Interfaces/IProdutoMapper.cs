using IbnelveApi.Application.DTOs;
using IbnelveApi.Domain.Entities;

namespace IbnelveApi.Application.Mappers.Interfaces;

/// <summary>
/// Interface específica para mapeamento de Produto
/// </summary>
public interface IProdutoMapper :
    IMapper<Produto, ProdutoResponseDto>,
    IMapper<ProdutoCreateDto, Produto>,
    ICollectionMapper<Produto, ProdutoResponseDto>
{
    /// <summary>
    /// Mapeia ProdutoUpdateDto para Produto existente, preservando campos não atualizáveis
    /// </summary>
    /// <param name="updateDto">DTO com dados para atualização</param>
    /// <param name="existingProduto">Produto existente no banco</param>
    /// <returns>Produto com dados atualizados</returns>
    Produto MapUpdate(ProdutoUpdateDto updateDto, Produto existingProduto);

    /// <summary>
    /// Mapeia Produto para ProdutoResponseDto com validações adicionais
    /// </summary>
    /// <param name="produto">Produto de origem</param>
    /// <returns>DTO de resposta</returns>
    ProdutoResponseDto MapToResponse(Produto produto);

    /// <summary>
    /// Mapeia ProdutoCreateDto para Produto com configurações iniciais
    /// </summary>
    /// <param name="createDto">DTO de criação</param>
    /// <param name="tenantId">ID do tenant</param>
    /// <returns>Produto configurado para criação</returns>
    Produto MapToEntity(ProdutoCreateDto createDto, string tenantId);
}

