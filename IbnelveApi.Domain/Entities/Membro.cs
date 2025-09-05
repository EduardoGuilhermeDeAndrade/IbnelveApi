using IbnelveApi.Domain.ValueObjects;

namespace IbnelveApi.Domain.Entities;

public class Membro : TenantEntity
{
    public string Nome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public Endereco Endereco { get; set; } = null!;

    public Membro() { }

    public Membro(string nome, string cpf, string telefone, Endereco endereco, string tenantId)
    {
        Nome = nome;
        CPF = cpf;
        Telefone = telefone;
        Endereco = endereco;
        TenantId = tenantId;  // TenantId obrigatório
    }

    public void AtualizarDados(string nome, string cpf, string telefone, Endereco endereco)
    {
        Nome = nome;
        CPF = cpf;
        Telefone = telefone;
        Endereco = endereco;
        UpdatedAt = DateTime.UtcNow;
    }

}

