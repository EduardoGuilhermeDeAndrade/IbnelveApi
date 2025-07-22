using IbnelveApi.Domain.Common;

namespace IbnelveApi.Domain.Entities;

public class Produto : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
}

