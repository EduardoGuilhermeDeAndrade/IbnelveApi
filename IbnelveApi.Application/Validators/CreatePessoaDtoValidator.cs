using FluentValidation;
using IbnelveApi.Application.DTOs;
using IbnelveApi.Domain.Interfaces;

namespace IbnelveApi.Application.Validators;

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

        RuleFor(x => x.CPF)
            .NotEmpty()
                .WithMessage("CPF é obrigatório")
            .Matches(@"^\d{11}$")
                .WithMessage("CPF deve conter exatamente 11 dígitos")
            .MustAsync(async (cpf, cancellation) => !await _repository.CpfExistsAsync(cpf))
                .WithMessage("Já existe um colaborador com este CPF.")
            .Must(BeValidCPF)
                .WithMessage("CPF inválido");

        RuleFor(x => x.Telefone)
            .NotEmpty().WithMessage("Telefone é obrigatório")
            .MaximumLength(20).WithMessage("Telefone deve ter no máximo 20 caracteres")
            .Matches(@"^[\d\s\(\)\-\+]+$").WithMessage("Telefone contém caracteres inválidos");

        RuleFor(x => x.Endereco)
            .NotNull().WithMessage("Endereço é obrigatório")
            .SetValidator(new EnderecoDtoValidator());
    }

    private bool BeValidCPF(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf) || cpf.Length != 11)
            return false;

        // Verifica se todos os dígitos são iguais
        if (cpf.All(c => c == cpf[0]))
            return false;

        // Validação do primeiro dígito verificador
        var sum = 0;
        for (int i = 0; i < 9; i++)
            sum += int.Parse(cpf[i].ToString()) * (10 - i);

        var remainder = sum % 11;
        var digit1 = remainder < 2 ? 0 : 11 - remainder;

        if (int.Parse(cpf[9].ToString()) != digit1)
            return false;

        // Validação do segundo dígito verificador
        sum = 0;
        for (int i = 0; i < 10; i++)
            sum += int.Parse(cpf[i].ToString()) * (11 - i);

        remainder = sum % 11;
        var digit2 = remainder < 2 ? 0 : 11 - remainder;

        return int.Parse(cpf[10].ToString()) == digit2;
    }
}

