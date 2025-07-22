using AutoMapper;
using IbnelveApi.Application.DTOs;
using IbnelveApi.Domain.Entities;

namespace IbnelveApi.Application.Mappings;

public class ProdutoMappingProfile : Profile
{
    public ProdutoMappingProfile()
    {
        CreateMap<Produto, ProdutoResponseDto>();
        CreateMap<ProdutoCreateDto, Produto>();
        CreateMap<ProdutoUpdateDto, Produto>();
    }
}

