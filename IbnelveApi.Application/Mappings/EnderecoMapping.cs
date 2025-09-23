using IbnelveApi.Application.DTOs;
using IbnelveApi.Domain.ValueObjects;

namespace IbnelveApi.Application.Mappings;

public static class EnderecoMapping
{
    public static EnderecoDto ToDto(Endereco endereco)
    {
        return new EnderecoDto
        {
            Rua = endereco.Rua,
            CEP = endereco.CEP.Replace(".", "").Replace("-", "").Replace(" ", ""),
            Bairro = endereco.Bairro,
            Cidade = endereco.Cidade,
            UF = endereco.UF
        };
    }

    public static Endereco ToValueObject(EnderecoDto enderecoDto)
    {
        return new Endereco(
            enderecoDto.Rua,
            enderecoDto.CEP.Replace(".", "").Replace("-", "").Replace(" ", ""),
            enderecoDto.Bairro,
            enderecoDto.Cidade,
            enderecoDto.UF,
            enderecoDto.Pais
        );
    }
}

