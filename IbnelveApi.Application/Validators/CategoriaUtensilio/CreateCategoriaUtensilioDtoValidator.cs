using FluentValidation;
using IbnelveApi.Application.DTOs.Categoria;

namespace IbnelveApi.Application.Validators.CategoriaUtensilio;

public class CreateCategoriaUtensilioDtoValidator : AbstractValidator<CreateCategoriaUtensilioDto>
{
    public CreateCategoriaUtensilioDtoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Descricao)
            .MaximumLength(500);

        RuleFor(x => x.Cor)
            .MaximumLength(20);
    }
}