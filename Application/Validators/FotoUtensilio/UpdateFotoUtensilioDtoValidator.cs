using FluentValidation;

public class UpdateFotoUtensilioDtoValidator : AbstractValidator<UpdateFotoUtensilioDto>
{
    public UpdateFotoUtensilioDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ArquivoPath).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Descricao).MaximumLength(255);
        RuleFor(x => x.IsPrincipal).NotNull();
    }
}