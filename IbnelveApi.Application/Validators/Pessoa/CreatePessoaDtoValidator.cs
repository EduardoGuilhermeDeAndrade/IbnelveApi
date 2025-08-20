using FluentValidation;
using IbnelveApi.Application.DTOs;
using IbnelveApi.Domain.Interfaces;
using IbnelveApi.Application.Validators.Support;
using IbnelveApi.Application.Extensions;

namespace IbnelveApi.Application.Validators.Pessoa;

public class CreatePessoaDtoValidator : AbstractValidator<CreatePessoaDto>
{
    private readonly IPessoaRepository _repository;
    public CreatePessoaDtoValidator(IPessoaRepository repository)
    {
        _repository = repository;

        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .MaximumLength(200).WithMessage("Nome deve ter no máximo 200 caracteres")
            .MinimumLength(2).WithMessage("Nome deve ter pelo menos 2 caracteres");

        RuleFor(x => x.CPF.RemoveSpecialCharacters())
            .NotEmpty()
                .WithMessage("CPF é obrigatório")
            .Matches(@"^\d{11}$")
                .WithMessage("CPF deve conter exatamente 11 dígitos")
            .MustAsync(async (cpf, cancellation) => !await _repository.CpfExistsAsync(cpf))
                .WithMessage("Já existe um colaborador com este CPF.")
            .Must(ValidateCPF.BeValidCPF)
                .WithMessage("CPF inválido");

        RuleFor(x => x.Telefone.RemoveSpecialCharacters())
            .NotEmpty().WithMessage("Telefone é obrigatório")
            .MaximumLength(20).WithMessage("Telefone deve ter no máximo 20 caracteres")
            .Matches(@"^[\d\s\(\)\-\+]+$").WithMessage("Telefone contém caracteres inválidos");

        RuleFor(x => x.Endereco)
            .NotNull().WithMessage("Endereço é obrigatório")
            .SetValidator(new EnderecoDtoValidator());
    }
}

