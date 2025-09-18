using FluentValidation;
using IbnelveApi.Application.DTOs;

namespace IbnelveApi.Application.Validators;

public class CreateCidadeDtoValidator : AbstractValidator<CreateCidadeDto>
{
    public CreateCidadeDtoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome � obrigat�rio.")
            .Length(2, 200).WithMessage("Nome deve ter entre 2 e 200 caracteres.");
        RuleFor(x => x.UF)
            .NotEmpty().WithMessage("UF � obrigat�rio.")
            .Length(2).WithMessage("UF deve ter 2 letras mai�sculas.")
            .Matches("^[A-Z]{2}$").WithMessage("UF deve conter apenas letras mai�sculas.");
        RuleFor(x => x.CEP)
            .NotEmpty().WithMessage("CEP � obrigat�rio.")
            .Length(8).WithMessage("CEP deve ter 8 d�gitos.")
            .Matches("^[0-9]{8}$").WithMessage("CEP deve conter apenas n�meros.");
    }
}
