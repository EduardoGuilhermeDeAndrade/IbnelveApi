using FluentValidation;
using IbnelveApi.Application.Dtos.Utensilio;

namespace IbnelveApi.Application.Validators.Utensilio;

public class UpdateUtensilioDtoValidator : AbstractValidator<UpdateUtensilioDto>
{
    public UpdateUtensilioDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(100);
        RuleFor(x => x.ValorReferencia).GreaterThanOrEqualTo(0).When(x => x.ValorReferencia.HasValue);
        RuleFor(x => x.Situacao).IsInEnum();
    }
}