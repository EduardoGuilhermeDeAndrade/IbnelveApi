using IbnelveApi.Application.Dtos.Utensilio;
using IbnelveApi.Domain.Entities;

namespace IbnelveApi.Application.Mappings;

public static class UtensilioMapping
{
    public static UtensilioDto ToDto(this Utensilio entity)
        => new()
        {
            Id = entity.Id,
            Nome = entity.Nome,
            Descricao = entity.Descricao,
            Observacoes = entity.Observacoes,
            ValorReferencia = entity.ValorReferencia,
            DataCompra = entity.DataCompra,
            NumeroSerie = entity.NumeroSerie,
            NomeFornecedor = entity.NomeFornecedor,
            Situacao = (int)entity.Situacao,
            TenantId = entity.TenantId
        };

    public static Utensilio ToEntity(this CreateUtensilioDto dto, string tenantId)
        => new(
            dto.Nome,
            dto.Descricao,
            dto.Observacoes,
            dto.ValorReferencia,
            dto.DataCompra,
            dto.NumeroSerie,
            dto.NomeFornecedor,
            (Domain.Enums.StatusItem)dto.Situacao,
            tenantId
        );
}