public static class UtensilioMapping
{
    public static UtensilioDto ToDto(this Utensilio entity)
    {
        return new UtensilioDto
        {
            Id = entity.Id,
            Nome = entity.Nome,
            Descricao = entity.Descricao,
            Fotos = entity.Fotos?.Where(f => !f.IsDeleted).Select(f => f.ToDto()).ToList() ?? new()
        };
    }

    public static Utensilio ToEntity(this CreateUtensilioDto dto, string tenantId)
    {
        var entity = new Utensilio
        {
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            TenantId = tenantId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Fotos = dto.Fotos?.Select(f => f.ToEntity(tenantId)).ToList() ?? new()
        };
        return entity;
    }
}