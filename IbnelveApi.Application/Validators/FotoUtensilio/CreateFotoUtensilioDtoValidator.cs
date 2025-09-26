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
            .WithMessage("Descri��o deve ter no m�ximo 500 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Descricao));

        RuleFor(x => x.ArquivoPath)
            .NotEmpty()
            .WithMessage("Url da foto � obrigat�ria")
            .MaximumLength(300)
            .WithMessage("Url deve ter no m�ximo 300 caracteres");
    }
}