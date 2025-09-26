using FluentValidation;
using IbnelveApi.Application.DTOs.FotoUtensilio;

namespace IbnelveApi.Application.Validators.FotoUtensilio;

/// <summary>
/// Validador para CreateFotoUtensilioDto
/// </summary>
public class CreateFotoUtensilioDtoValidator : AbstractValidator<CreateFotoUtensilioDto>
{
    public CreateFotoUtensilioDtoValidator()
    {
               RuleFor(x => x.Descricao)
            .MaximumLength(500)
            .WithMessage("Descrição deve ter no máximo 500 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Descricao));

        RuleFor(x => x.ArquivoPath)
            .NotEmpty()
            .WithMessage("Url da foto é obrigatória")
            .MaximumLength(300)
            .WithMessage("Url deve ter no máximo 300 caracteres");
    }
}