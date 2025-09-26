using FluentValidation;
using IbnelveApi.Application.DTOs.FotoUtensilio;

namespace IbnelveApi.Application.Validators.FotoUtensilio;

/// <summary>
/// Validador para UpdateFotoUtensilioDto
/// </summary>
public class UpdateFotoUtensilioDtoValidator : AbstractValidator<UpdateFotoUtensilioDto>
{
    public UpdateFotoUtensilioDtoValidator()
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