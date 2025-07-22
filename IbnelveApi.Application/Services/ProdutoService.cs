using AutoMapper;
using FluentValidation;
using IbnelveApi.Application.Common;
using IbnelveApi.Application.DTOs;
using IbnelveApi.Domain.Entities;
using IbnelveApi.Domain.Interfaces;

namespace IbnelveApi.Application.Services;

public class ProdutoService : IProdutoService
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<ProdutoCreateDto> _createValidator;
    private readonly IValidator<ProdutoUpdateDto> _updateValidator;
    private readonly ITenantContext _tenantContext;

    public ProdutoService(
        IProdutoRepository produtoRepository,
        IMapper mapper,
        IValidator<ProdutoCreateDto> createValidator,
        IValidator<ProdutoUpdateDto> updateValidator,
        ITenantContext tenantContext)
    {
        _produtoRepository = produtoRepository;
        _mapper = mapper;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _tenantContext = tenantContext;
    }

    public async Task<ApiResponse<IEnumerable<ProdutoResponseDto>>> GetAllAsync()
    {
        try
        {
            var produtos = await _produtoRepository.GetAllAsync();
            var produtosDto = _mapper.Map<IEnumerable<ProdutoResponseDto>>(produtos);
            return ApiResponse<IEnumerable<ProdutoResponseDto>>.SuccessResult(produtosDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<ProdutoResponseDto>>.ErrorResult(
                "Erro ao buscar produtos", ex.Message);
        }
    }

    public async Task<ApiResponse<ProdutoResponseDto>> GetByIdAsync(int id)
    {
        try
        {
            var produto = await _produtoRepository.GetByIdAsync(id);
            if (produto == null)
            {
                return ApiResponse<ProdutoResponseDto>.ErrorResult(
                    "Produto não encontrado", "O produto especificado não existe");
            }

            var produtoDto = _mapper.Map<ProdutoResponseDto>(produto);
            return ApiResponse<ProdutoResponseDto>.SuccessResult(produtoDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<ProdutoResponseDto>.ErrorResult(
                "Erro ao buscar produto", ex.Message);
        }
    }

    public async Task<ApiResponse<ProdutoResponseDto>> CreateAsync(ProdutoCreateDto produtoDto)
    {
        try
        {
            var validationResult = await _createValidator.ValidateAsync(produtoDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return ApiResponse<ProdutoResponseDto>.ErrorResult(
                    "Dados inválidos", errors);
            }

            var produto = _mapper.Map<Produto>(produtoDto);
            produto.TenantId = _tenantContext.TenantId;

            var produtoCriado = await _produtoRepository.AddAsync(produto);
            var produtoResponseDto = _mapper.Map<ProdutoResponseDto>(produtoCriado);

            return ApiResponse<ProdutoResponseDto>.SuccessResult(
                produtoResponseDto, "Produto criado com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse<ProdutoResponseDto>.ErrorResult(
                "Erro ao criar produto", ex.Message);
        }
    }

    public async Task<ApiResponse<ProdutoResponseDto>> UpdateAsync(ProdutoUpdateDto produtoDto)
    {
        try
        {
            var validationResult = await _updateValidator.ValidateAsync(produtoDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return ApiResponse<ProdutoResponseDto>.ErrorResult(
                    "Dados inválidos", errors);
            }

            var produtoExistente = await _produtoRepository.GetByIdAsync(produtoDto.Id);
            if (produtoExistente == null)
            {
                return ApiResponse<ProdutoResponseDto>.ErrorResult(
                    "Produto não encontrado", "O produto especificado não existe");
            }

            produtoExistente.Nome = produtoDto.Nome;
            produtoExistente.Preco = produtoDto.Preco;
            produtoExistente.UpdatedAt = DateTime.UtcNow;

            var produtoAtualizado = await _produtoRepository.UpdateAsync(produtoExistente);
            var produtoResponseDto = _mapper.Map<ProdutoResponseDto>(produtoAtualizado);

            return ApiResponse<ProdutoResponseDto>.SuccessResult(
                produtoResponseDto, "Produto atualizado com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse<ProdutoResponseDto>.ErrorResult(
                "Erro ao atualizar produto", ex.Message);
        }
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int id)
    {
        try
        {
            var produto = await _produtoRepository.GetByIdAsync(id);
            if (produto == null)
            {
                return ApiResponse<bool>.ErrorResult(
                    "Produto não encontrado", "O produto especificado não existe");
            }

            await _produtoRepository.DeleteAsync(id);
            return ApiResponse<bool>.SuccessResult(true, "Produto excluído com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResult(
                "Erro ao excluir produto", ex.Message);
        }
    }
}

