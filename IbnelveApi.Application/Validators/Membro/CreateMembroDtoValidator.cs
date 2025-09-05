using FluentValidation;
using IbnelveApi.Application.DTOs;
using IbnelveApi.Application.Interfaces;
using IbnelveApi.Application.Validators.Support;
using IbnelveApi.Application.Extensions;

namespace IbnelveApi.Application.Validators.Membro;

/// <summary>
/// Validator para CreateMembroDto
/// ATUALIZADO: Agora considera tenantId para validação de CPF duplicado
/// </summary>
public class CreateMembroDtoValidator : AbstractValidator<CreateMembroDto>
{
    private readonly IMembroService _membroService; 
    private readonly ICurrentUserService _currentUserService; 

    public CreateMembroDtoValidator(IMembroService membroService, ICurrentUserService currentUserService)
    {
        _membroService = membroService;
        _currentUserService = currentUserService;

        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .MaximumLength(200).WithMessage("Nome deve ter no máximo 200 caracteres")
            .MinimumLength(2).WithMessage("Nome deve ter pelo menos 2 caracteres")
            .Matches(@"^[a-zA-ZÀ-ÿ\s]+$").WithMessage("Nome deve conter apenas letras e espaços");

        RuleFor(x => x.CPF)
            .NotEmpty().WithMessage("CPF é obrigatório")
            .Must(cpf => !string.IsNullOrEmpty(cpf?.RemoveSpecialCharacters()))
                .WithMessage("CPF é obrigatório")
            .Must(cpf => cpf.RemoveSpecialCharacters().Length == 11)
                .WithMessage("CPF deve conter exatamente 11 dígitos")
            .Must(cpf => ValidateCPF.BeValidCPF(cpf.RemoveSpecialCharacters()))
                .WithMessage("CPF inválido")
            .MustAsync(async (cpf, cancellation) =>
            {
                var tenantId = _currentUserService.GetTenantId();
                if (string.IsNullOrEmpty(tenantId)) return false; 

                var result = await _membroService.CpfExistsAsync(cpf.RemoveSpecialCharacters(), tenantId);
                return !result.Data; 
            })
                .WithMessage("Já existe uma membro com este CPF neste tenant");

        RuleFor(x => x.Telefone)
            .NotEmpty().WithMessage("Telefone é obrigatório")
            .Must(telefone => !string.IsNullOrEmpty(telefone?.RemoveSpecialCharacters()))
                .WithMessage("Telefone é obrigatório")
            .Must(telefone => telefone.RemoveSpecialCharacters().Length >= 10)
                .WithMessage("Telefone deve ter pelo menos 10 dígitos")
            .Must(telefone => telefone.RemoveSpecialCharacters().Length <= 15)
                .WithMessage("Telefone deve ter no máximo 15 dígitos")
            .Matches(@"^[\d\s\(\)\-\+]+$").WithMessage("Telefone contém caracteres inválidos");

        RuleFor(x => x.Endereco)
            .NotNull().WithMessage("Endereço é obrigatório")
            .SetValidator(new EnderecoDtoValidator()); 
    }
}

