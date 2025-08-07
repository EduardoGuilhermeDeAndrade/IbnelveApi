using IbnelveApi.Domain.ValueObjects;

namespace IbnelveApi.Domain.Entities;

public class Pessoa : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public Endereco Endereco { get; set; } = null!;

    public Pessoa() { }

    public Pessoa(string nome, string cpf, string telefone, Endereco endereco)
    {
        Nome = nome;
        CPF = cpf;
        Telefone = telefone;
        Endereco = endereco;
    }

    public Pessoa(string nome, string cpf, string telefone, Endereco endereco, string tenantId)
    {
        Nome = nome;
        CPF = cpf;
        Telefone = telefone;
        Endereco = endereco;
        TenantId = tenantId;
    }

    public void AtualizarDados(string nome, string cpf, string telefone, Endereco endereco)
    {
        Nome = nome;
        CPF = cpf;
        Telefone = telefone;
        Endereco = endereco;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ExcluirLogicamente()
    {
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
    }
}

