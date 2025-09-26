using IbnelveApi.Application.DTOs.FotoUtensilio;

public static class FotoUtensilioMapping
{
    public static FotoUtensilioDto ToDto(this FotoUtensilio entity)
    {
        return new FotoUtensilioDto
        {
            Id = entity.Id,
            ArquivoPath = entity.ArquivoPath,
            Descricao = entity.Descricao,
            IsPrincipal = entity.IsPrincipal
        };
    }

    public static FotoUtensilio ToEntity(this CreateFotoUtensilioDto dto, string tenantId)
    {
        return new FotoUtensilio
        {
            ArquivoPath = dto.ArquivoPath,
            Descricao = dto.Descricao,
            IsPrincipal = dto.IsPrincipal,
            TenantId = tenantId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public static FotoUtensilio ToEntity(this CreateFotoUtensilioDto dto, int utensilioId, string tenantId)
    {
        var entity = dto.ToEntity(tenantId);
        entity.UtensilioId = utensilioId;
        return entity;
    }
}