using IbnelveApi.Domain.Entities;

namespace IbnelveApi.Domain.ValueObjects;

public class Endereco
{
    public string Rua { get; private set; }
    public string CEP { get; private set; }
    public string Bairro { get; private set; }
    public string Cidade { get; private set; }
    public string UF { get; private set; }
    public string Pais { get; private set; }


    public Endereco(string rua, string cep, string bairro, string cidade, string uf, string pais)
    {
        Rua = rua;
        CEP = cep;
        Bairro = bairro;
        Cidade = cidade;
        UF = uf;
        Pais = pais;
    }

    // Construtor sem parâmetros para EF Core
    private Endereco() 
    {
        Rua = string.Empty;
        CEP = string.Empty;
        Bairro = string.Empty;
        Cidade = string.Empty;
        UF = string.Empty;
        Pais = string.Empty;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Endereco other) return false;
        
        return Rua == other.Rua &&
               CEP == other.CEP &&
               Bairro == other.Bairro &&
               Cidade == other.Cidade &&
               UF == other.UF &&
               Pais == other.Pais;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Rua, CEP, Bairro, Cidade, UF, Pais);
    }

    public override string ToString()
    {
        return $"{Rua}, {Bairro}, {Cidade} - {UF} - {Pais}, CEP: {CEP}";
    }
}

