using FluentValidation;
using IbnelveApi.Application.DTOs;

namespace IbnelveApi.Application.Validators;

public class CreateCidadeDtoValidator : AbstractValidator<CreateCidadeDto>
{
    public CreateCidadeDtoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .Length(2, 200).WithMessage("Nome deve ter entre 2 e 200 caracteres.");
        RuleFor(x => x.UF)
            .NotEmpty().WithMessage("UF é obrigatório.")
            .Length(2).WithMessage("UF deve ter 2 letras maiúsculas.")
            .Matches("^[A-Z]{2}$").WithMessage("UF deve conter apenas letras maiúsculas.");
        RuleFor(x => x.CEP)
            .NotEmpty().WithMessage("CEP é obrigatório.")
            .Length(8).WithMessage("CEP deve ter 8 dígitos.")
            .Matches("^[0-9]{8}$").WithMessage("CEP deve conter apenas números.");
    }
}
