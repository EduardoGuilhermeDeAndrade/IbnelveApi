using FluentValidation;

public class CreateUtensilioDtoValidator : AbstractValidator<CreateUtensilioDto>
{
    public CreateUtensilioDtoValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Descricao).MaximumLength(255);
        RuleForEach(x => x.Fotos).SetValidator(new CreateFotoUtensilioDtoValidator());
    }
}