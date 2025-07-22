using FluentValidation;
using IbnelveApi.Application.DTOs;

namespace IbnelveApi.Application.Validators;

public class ProdutoUpdateDtoValidator : AbstractValidator<ProdutoUpdateDto>
{
    public ProdutoUpdateDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id deve ser maior que zero");

        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .Length(2, 100).WithMessage("Nome deve ter entre 2 e 100 caracteres");

        RuleFor(x => x.Preco)
            .GreaterThan(0).WithMessage("Preço deve ser maior que zero")
            .LessThanOrEqualTo(999999.99m).WithMessage("Preço deve ser menor ou igual a 999.999,99");
    }
}

