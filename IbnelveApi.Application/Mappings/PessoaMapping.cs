using IbnelveApi.Application.DTOs;
using IbnelveApi.Domain.Entities;
using IbnelveApi.Domain.ValueObjects;

namespace IbnelveApi.Application.Mappings;

public static class PessoaMapping
{
    public static PessoaDto ToDto(Pessoa pessoa)
    {
        return new PessoaDto
        {
            Id = pessoa.Id,
            Nome = pessoa.Nome,
            CPF = pessoa.CPF,
            Telefone = pessoa.Telefone,
            Endereco = EnderecoMapping.ToDto(pessoa.Endereco),
            //TenantId = pessoa.TenantId,
            CreatedAt = pessoa.CreatedAt,
            UpdatedAt = pessoa.UpdatedAt
        };
    }

    public static Pessoa ToEntity(CreatePessoaDto createDto)
    {
        return new Pessoa(
            createDto.Nome,
            createDto.CPF.Replace(".", "").Replace("-", "").Replace(" ", ""),
            createDto.Telefone,
            EnderecoMapping.ToValueObject(createDto.Endereco)
            // TenantId será definido automaticamente no DbContext.SaveChangesAsync
        );
    }
    //public static Pessoa ToEntity(CreatePessoaDto createDto, string tenantId)
    //{
    //    return new Pessoa(
    //        createDto.Nome,
    //        createDto.CPF.Replace(".", "").Replace("-", "").Replace(" ", ""),
    //        createDto.Telefone,
    //        EnderecoMapping.ToValueObject(createDto.Endereco),
    //        tenantId
    //    );
    //}

    public static void UpdateEntity(Pessoa pessoa, UpdatePessoaDto updateDto)
    {
        pessoa.AtualizarDados(
            updateDto.Nome,
            updateDto.CPF,
            updateDto.Telefone,
            EnderecoMapping.ToValueObject(updateDto.Endereco)
        );
    }

    public static IEnumerable<PessoaDto> ToDtoList(IEnumerable<Pessoa> pessoas)
    {
        return pessoas.Select(ToDto);
    }
}

