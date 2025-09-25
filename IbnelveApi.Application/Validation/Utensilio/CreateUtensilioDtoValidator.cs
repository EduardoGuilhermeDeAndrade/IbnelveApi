//using FluentValidation;
//using IbnelveApi.Application.Dtos.Utensilio;
//using IbnelveApi.Domain.Interfaces;

//public class CreateUtensilioDtoValidator : AbstractValidator<CreateUtensilioDto>
//{
//    public CreateUtensilioDtoValidator(ICategoriaRepository categoriaRepository)
//    {
//        RuleFor(x => x.CategoriaId)
//            .MustAsync(async (categoriaId, cancellation) =>
//                await categoriaRepository.ExistsAsync(categoriaId))
//            .WithMessage("CategoriaId informado não existe.");
        
//        // Outras regras...
//    }
//}