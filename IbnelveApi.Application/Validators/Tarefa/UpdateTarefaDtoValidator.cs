using FluentValidation;
using IbnelveApi.Application.DTOs;
using IbnelveApi.Domain.Enums;

namespace IbnelveApi.Application.Validators.Tarefa;

public class UpdateTarefaDtoValidator : AbstractValidator<UpdateTarefaDto>
{
    public UpdateTarefaDtoValidator()
    {
        RuleFor(x => x.Titulo)
            .NotEmpty().WithMessage("Título é obrigatório")
            .MaximumLength(200).WithMessage("Título deve ter no máximo 200 caracteres")
            .MinimumLength(3).WithMessage("Título deve ter pelo menos 3 caracteres");

        RuleFor(x => x.Descricao)
            .NotEmpty().WithMessage("Descrição é obrigatória")
            .MaximumLength(1000).WithMessage("Descrição deve ter no máximo 1000 caracteres")
            .MinimumLength(5).WithMessage("Descrição deve ter pelo menos 5 caracteres");

        RuleFor(x => x.Prioridade)
            .IsInEnum().WithMessage("Prioridade deve ser um valor válido")
            .NotEqual(PrioridadeTarefa.Critica).When(x => string.IsNullOrEmpty(x.Categoria))
            .WithMessage("Tarefas críticas devem ter uma categoria definida");

        RuleFor(x => x.DataVencimento)
            .GreaterThan(DateTime.UtcNow.Date.AddDays(-1))
            .WithMessage("Data de vencimento não pode ser anterior a hoje")
            .When(x => x.DataVencimento.HasValue);

        RuleFor(x => x.Categoria)
            .MaximumLength(100).WithMessage("Categoria deve ter no máximo 100 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Categoria));
    }
}

