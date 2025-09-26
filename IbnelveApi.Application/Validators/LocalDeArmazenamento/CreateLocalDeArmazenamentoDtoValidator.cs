using FluentValidation;
using IbnelveApi.Application.DTOs.LocalDeArmazenamento;

namespace IbnelveApi.Application.Validators.LocalDeArmazenamento;

public class CreateLocalDeArmazenamentoDtoValidator : AbstractValidator<CreateLocalDeArmazenamentoDto>
{
    public CreateLocalDeArmazenamentoDtoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Descricao)
            .MaximumLength(500);

        RuleFor(x => x.ContatoResponsavel)
            .MaximumLength(200);
    }
}