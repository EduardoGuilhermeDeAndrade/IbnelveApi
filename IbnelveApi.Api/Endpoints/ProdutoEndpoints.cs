using IbnelveApi.Application.DTOs;
using IbnelveApi.Application.Services;
using Microsoft.AspNetCore.Authorization;

namespace IbnelveApi.Api.Endpoints;

public static class ProdutoEndpoints
{
    public static void MapProdutoEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/produtos")
            .WithTags("Produtos")
            .RequireAuthorization();

        // GET /api/produtos
        group.MapGet("/", async (IProdutoService produtoService) =>
        {
            var result = await produtoService.GetAllAsync();
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        })
        .WithName("GetAllProdutos")
        .WithSummary("Obter todos os produtos")
        .WithDescription("Retorna uma lista de todos os produtos do tenant atual")
        .Produces<object>(200)
        .Produces<object>(400);

        // GET /api/produtos/{id}
        group.MapGet("/{id:int}", async (int id, IProdutoService produtoService) =>
        {
            var result = await produtoService.GetByIdAsync(id);
            return result.Success ? Results.Ok(result) : Results.NotFound(result);
        })
        .WithName("GetProdutoById")
        .WithSummary("Obter produto por ID")
        .WithDescription("Retorna um produto específico pelo ID")
        .Produces<object>(200)
        .Produces<object>(404);

        // POST /api/produtos
        group.MapPost("/", async (ProdutoCreateDto produtoDto, IProdutoService produtoService) =>
        {
            var result = await produtoService.CreateAsync(produtoDto);
            return result.Success 
                ? Results.Created($"/api/produtos/{result.Data?.Id}", result)
                : Results.BadRequest(result);
        })
        .WithName("CreateProduto")
        .WithSummary("Criar novo produto")
        .WithDescription("Cria um novo produto")
        .Produces<object>(201)
        .Produces<object>(400);

        // PUT /api/produtos/{id}
        group.MapPut("/{id:int}", async (int id, ProdutoUpdateDto produtoDto, IProdutoService produtoService) =>
        {
            if (id != produtoDto.Id)
            {
                return Results.BadRequest(new { Success = false, Message = "ID do produto não confere", Errors = new[] { "O ID da URL deve ser igual ao ID do produto" } });
            }

            var result = await produtoService.UpdateAsync(produtoDto);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        })
        .WithName("UpdateProduto")
        .WithSummary("Atualizar produto")
        .WithDescription("Atualiza um produto existente")
        .Produces<object>(200)
        .Produces<object>(400)
        .Produces<object>(404);

        // DELETE /api/produtos/{id}
        group.MapDelete("/{id:int}", async (int id, IProdutoService produtoService) =>
        {
            var result = await produtoService.DeleteAsync(id);
            return result.Success ? Results.Ok(result) : Results.NotFound(result);
        })
        .WithName("DeleteProduto")
        .WithSummary("Excluir produto")
        .WithDescription("Exclui um produto (exclusão lógica)")
        .Produces<object>(200)
        .Produces<object>(404);
    }
}

