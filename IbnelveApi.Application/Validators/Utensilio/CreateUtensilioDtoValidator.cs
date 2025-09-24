using FluentValidation;
using IbnelveApi.Application.Dtos.Utensilio;

namespace IbnelveApi.Application.Validators.Utensilio;

public class CreateUtensilioDtoValidator : AbstractValidator<CreateUtensilioDto>
{
    public CreateUtensilioDtoValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(100);
        RuleFor(x => x.ValorReferencia).GreaterThanOrEqualTo(0).When(x => x.ValorReferencia.HasValue);
        RuleFor(x => x.Situacao).IsInEnum();
    }
}