using FluentValidation;

public class CreateFotoUtensilioDtoValidator : AbstractValidator<CreateFotoUtensilioDto>
{
    public CreateFotoUtensilioDtoValidator()
    {
        RuleFor(x => x.ArquivoPath).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Descricao).MaximumLength(255);
        RuleFor(x => x.IsPrincipal).NotNull();
    }
}