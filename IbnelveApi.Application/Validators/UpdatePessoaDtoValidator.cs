using FluentValidation;
using IbnelveApi.Application.DTOs;
using IbnelveApi.Application.Validators.Support;

namespace IbnelveApi.Application.Validators;

public class UpdatePessoaDtoValidator : AbstractValidator<UpdatePessoaDto>
{
    public UpdatePessoaDtoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .MaximumLength(200).WithMessage("Nome deve ter no máximo 200 caracteres")
            .MinimumLength(2).WithMessage("Nome deve ter pelo menos 2 caracteres");

        RuleFor(x => x.CPF)
            .NotEmpty().WithMessage("CPF é obrigatório")
            .Matches(@"^\d{11}$").WithMessage("CPF deve conter exatamente 11 dígitos")
            .Must(ValidateCPF.BeValidCPF).WithMessage("CPF inválido");

        RuleFor(x => x.Telefone)
            .NotEmpty().WithMessage("Telefone é obrigatório")
            .MaximumLength(20).WithMessage("Telefone deve ter no máximo 20 caracteres")
            .Matches(@"^[\d\s\(\)\-\+]+$").WithMessage("Telefone contém caracteres inválidos");

        RuleFor(x => x.Endereco)
            .NotNull().WithMessage("Endereço é obrigatório")
            .SetValidator(new EnderecoDtoValidator());
    }
}

