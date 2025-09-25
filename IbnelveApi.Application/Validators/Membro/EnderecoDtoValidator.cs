using FluentValidation;
using IbnelveApi.Application.DTOs.Membro.Endereco;
using IbnelveApi.Application.Extensions;

namespace IbnelveApi.Application.Validators.Membro;

/// <summary>
/// Validator para EnderecoDto
/// ATUALIZADO: Validações melhoradas e mais robustas
/// </summary>
public class EnderecoDtoValidator : AbstractValidator<EnderecoDto>
{
    public EnderecoDtoValidator()
    {
        RuleFor(x => x.Rua)
            .NotEmpty().WithMessage("Rua é obrigatória")
            .MaximumLength(300).WithMessage("Rua deve ter no máximo 300 caracteres")
            .MinimumLength(5).WithMessage("Rua deve ter pelo menos 5 caracteres")
            .Matches(@"^[a-zA-ZÀ-ÿ0-9\s\.,\-\/]+$").WithMessage("Rua contém caracteres inválidos");

        RuleFor(x => x.CEP)
            .NotEmpty().WithMessage("CEP é obrigatório")
            .Must(cep => !string.IsNullOrEmpty(cep?.RemoveSpecialCharacters()))
                .WithMessage("CEP é obrigatório")
            .Must(cep => cep.RemoveSpecialCharacters().Length == 8)
                .WithMessage("CEP deve conter exatamente 8 dígitos")
            .Matches(@"^[\d\-\s]+$").WithMessage("CEP deve conter apenas números, hífens e espaços");

        RuleFor(x => x.Bairro)
            .NotEmpty().WithMessage("Bairro é obrigatório")
            .MaximumLength(100).WithMessage("Bairro deve ter no máximo 100 caracteres")
            .MinimumLength(2).WithMessage("Bairro deve ter pelo menos 2 caracteres")
            .Matches(@"^[a-zA-ZÀ-ÿ0-9\s\.\-]+$").WithMessage("Bairro contém caracteres inválidos");

        RuleFor(x => x.Cidade)
            .NotEmpty().WithMessage("Cidade é obrigatória")
            .MaximumLength(100).WithMessage("Cidade deve ter no máximo 100 caracteres")
            .MinimumLength(2).WithMessage("Cidade deve ter pelo menos 2 caracteres")
            .Matches(@"^[a-zA-ZÀ-ÿ\s\.\-\']+$").WithMessage("Cidade deve conter apenas letras, espaços, pontos, hífens e apóstrofes");

        RuleFor(x => x.UF)
            .NotEmpty().WithMessage("UF é obrigatória")
            .Length(2).WithMessage("UF deve ter exatamente 2 caracteres")
            .Matches(@"^[A-Z]{2}$").WithMessage("UF deve conter apenas letras maiúsculas")
            .Must(BeValidUF).WithMessage("UF inválida");
    }

    /// <summary>
    /// Valida se a UF é um estado brasileiro válido
    /// </summary>
    private static bool BeValidUF(string uf)
    {
        if (string.IsNullOrEmpty(uf)) return false;

        var validUFs = new[]
        {
            "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO",
            "MA", "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI",
            "RJ", "RN", "RS", "RO", "RR", "SC", "SP", "SE", "TO"
        };

        return validUFs.Contains(uf.ToUpper());
    }
}

