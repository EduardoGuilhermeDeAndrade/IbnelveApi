using FluentValidation;
using IbnelveApi.Application.DTOs;

namespace IbnelveApi.Application.Validators.CategoriaTarefa;

/// <summary>
/// Validador para UpdateCategoriaTarefaDto
/// </summary>
public class UpdateCategoriaTarefaDtoValidator : AbstractValidator<UpdateCategoriaTarefaDto>
{
    public UpdateCategoriaTarefaDtoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("Nome é obrigatório")
            .Length(2, 100)
            .WithMessage("Nome deve ter entre 2 e 100 caracteres")
            .Matches(@"^[a-zA-ZÀ-ÿ0-9\s\-_]+$")
            .WithMessage("Nome deve conter apenas letras, números, espaços, hífens e underscores");

        RuleFor(x => x.Descricao)
            .MaximumLength(500)
            .WithMessage("Descrição deve ter no máximo 500 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Descricao));

        RuleFor(x => x.Cor)
            .Matches(@"^#[0-9A-Fa-f]{6}$")
            .WithMessage("Cor deve estar no formato hexadecimal (#RRGGBB)")
            .When(x => !string.IsNullOrEmpty(x.Cor));
    }
}