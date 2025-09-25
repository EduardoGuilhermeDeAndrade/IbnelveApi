using IbnelveApi.Domain.Entities;
using IbnelveApi.Application.Interfaces;
using IbnelveApi.Application.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IbnelveApi.Application.DTOs.Cidade;

namespace IbnelveApi.Application.Services;

public class CidadeService : ICidadeService
{
    private readonly ICidadeRepository _cidadeRepository;

    public CidadeService(ICidadeRepository cidadeRepository)
    {
        _cidadeRepository = cidadeRepository;
    }

    public async Task<ApiResponse<IEnumerable<CidadeDto>>> GetAllAsync(bool includeDeleted = false)
    {
        var cidades = await _cidadeRepository.GetAllAsync(includeDeleted);
        var dtos = cidades.Select(c => new CidadeDto
        {
            Id = c.Id,
            Nome = c.Nome,
            UF = c.UF,
            CEP = c.CEP,
            Ativo = c.Ativo,
            CodigoIBGE = c.CodigoIBGE,
            Capital = c.Capital,
            EstadoId = c.EstadoId,
            EstadoNome = c.Estado?.Nome
        });
        return ApiResponse<IEnumerable<CidadeDto>>.SuccessResult(dtos);
    }

    public async Task<ApiResponse<CidadeDto>> GetByIdAsync(int id)
    {
        var cidade = await _cidadeRepository.GetByIdAsync(id);
        if (cidade == null)
            return ApiResponse<CidadeDto>.ErrorResult("Cidade não encontrada");
        var dto = new CidadeDto
        {
            Id = cidade.Id,
            Nome = cidade.Nome,
            UF = cidade.UF,
            CEP = cidade.CEP,
            Ativo = cidade.Ativo,
            CodigoIBGE = cidade.CodigoIBGE,
            Capital = cidade.Capital,
            EstadoId = cidade.EstadoId,
            EstadoNome = cidade.Estado?.Nome
        };
        return ApiResponse<CidadeDto>.SuccessResult(dto);
    }

    public async Task<ApiResponse<CidadeDto>> CreateAsync(CreateCidadeDto dto)
    {
        var cidade = new Cidade
        {
            Nome = dto.Nome,
            UF = dto.UF,
            CEP = dto.CEP,
            Ativo = dto.Ativo,
            CodigoIBGE = dto.CodigoIBGE,
            Capital = dto.Capital,
            EstadoId = dto.EstadoId
        };
        await _cidadeRepository.AddAsync(cidade);
        var resultDto = new CidadeDto
        {
            Id = cidade.Id,
            Nome = cidade.Nome,
            UF = cidade.UF,
            CEP = cidade.CEP,
            Ativo = cidade.Ativo,
            CodigoIBGE = cidade.CodigoIBGE,
            Capital = cidade.Capital,
            EstadoId = cidade.EstadoId
        };
        return ApiResponse<CidadeDto>.SuccessResult(resultDto);
    }

    public async Task<ApiResponse<CidadeDto>> UpdateAsync(int id, UpdateCidadeDto dto)
    {
        var cidade = await _cidadeRepository.GetByIdAsync(id);
        if (cidade == null)
            return ApiResponse<CidadeDto>.ErrorResult("Cidade não encontrada");
        cidade.Nome = dto.Nome;
        cidade.UF = dto.UF;
        cidade.CEP = dto.CEP;
        cidade.Ativo = dto.Ativo;
        cidade.CodigoIBGE = dto.CodigoIBGE;
        cidade.Capital = dto.Capital;
        cidade.EstadoId = dto.EstadoId;
        await _cidadeRepository.UpdateAsync(cidade);
        var resultDto = new CidadeDto
        {
            Id = cidade.Id,
            Nome = cidade.Nome,
            UF = cidade.UF,
            CEP = cidade.CEP,
            Ativo = cidade.Ativo,
            CodigoIBGE = cidade.CodigoIBGE,
            Capital = cidade.Capital,
            EstadoId = cidade.EstadoId
        };
        return ApiResponse<CidadeDto>.SuccessResult(resultDto);
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int id)
    {
        var cidade = await _cidadeRepository.GetByIdAsync(id);
        if (cidade == null)
            return ApiResponse<bool>.ErrorResult("Cidade não encontrada");
        cidade.ExcluirLogicamente();
        await _cidadeRepository.UpdateAsync(cidade);
        return ApiResponse<bool>.SuccessResult(true);
    }
}
