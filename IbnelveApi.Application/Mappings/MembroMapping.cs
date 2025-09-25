using IbnelveApi.Application.DTOs.Membro;
using IbnelveApi.Application.DTOs.Membro.Endereco;
using IbnelveApi.Domain.Entities;
using IbnelveApi.Domain.ValueObjects;

namespace IbnelveApi.Application.Mappings;

/// <summary>
/// Mapeamento entre Membro e DTOs
/// ATUALIZADO: Adicionados métodos auxiliares para Endereco
/// </summary>
public static class MembroMapping
{
    public static MembroDto ToDto(Membro membro)
    {
        return new MembroDto
        {
            Id = membro.Id,
            Nome = membro.Nome,
            CPF = membro.CPF,
            Telefone = membro.Telefone,
            Endereco = EnderecoMapping.ToDto(membro.Endereco),
            CreatedAt = membro.CreatedAt,
            UpdatedAt = membro.UpdatedAt
        };
    }

    /// <summary>
    /// Cria entidade Membro com tenantId 
    /// </summary>
    public static Membro ToEntity(CreateMembroDto createDto, string tenantId)
    {
        return new Membro(
            createDto.Nome,
            createDto.CPF.Replace(".", "").Replace("-", "").Replace(" ", ""),
            createDto.Telefone,
            EnderecoMapping.ToValueObject(createDto.Endereco),
            tenantId
        );
    }

    public static void UpdateEntity(Membro membro, UpdateMembroDto updateDto)
    {
        membro.AtualizarDados(
            updateDto.Nome,
            updateDto.CPF,
            updateDto.Telefone,
            EnderecoMapping.ToValueObject(updateDto.Endereco)
        );
    }

    public static IEnumerable<MembroDto> ToDtoList(IEnumerable<Membro> membros)
    {
        return membros.Select(ToDto);
    }

    /// <summary>
    /// Converte EnderecoDto para Endereco ValueObject (método auxiliar)
    /// </summary>
    public static Endereco ToEnderecoEntity(EnderecoDto enderecoDto)
    {
        return new Endereco(
            enderecoDto.Rua,
            enderecoDto.CEP.Replace("-", "").Replace(" ", ""),
            enderecoDto.Bairro,
            enderecoDto.Cidade,
            enderecoDto.UF,
            enderecoDto.Pais
        );
    }
}

