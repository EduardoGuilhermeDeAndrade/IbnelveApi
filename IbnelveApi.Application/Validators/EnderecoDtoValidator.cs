using FluentValidation;
using IbnelveApi.Application.DTOs;

namespace IbnelveApi.Application.Validators;

public class EnderecoDtoValidator : AbstractValidator<EnderecoDto>
{
    public EnderecoDtoValidator()
    {
        RuleFor(x => x.Rua)
            .NotEmpty().WithMessage("Rua é obrigatória")
            .MaximumLength(300).WithMessage("Rua deve ter no máximo 300 caracteres");

        RuleFor(x => x.CEP)
            .NotEmpty().WithMessage("CEP é obrigatório")
            .Matches(@"^\d{8}$").WithMessage("CEP deve conter exatamente 8 dígitos");

        RuleFor(x => x.Bairro)
            .NotEmpty().WithMessage("Bairro é obrigatório")
            .MaximumLength(100).WithMessage("Bairro deve ter no máximo 100 caracteres");

        RuleFor(x => x.Cidade)
            .NotEmpty().WithMessage("Cidade é obrigatória")
            .MaximumLength(100).WithMessage("Cidade deve ter no máximo 100 caracteres");

        RuleFor(x => x.UF)
            .NotEmpty().WithMessage("UF é obrigatória")
            .Length(2).WithMessage("UF deve ter exatamente 2 caracteres")
            .Matches(@"^[A-Z]{2}$").WithMessage("UF deve conter apenas letras maiúsculas");
    }
}

