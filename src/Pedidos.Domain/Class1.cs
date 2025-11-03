namespace Pedidos.Domain;

public class Pedido
{
    public int Id { get; set; }
    public string NomeCliente { get; set; } = string.Empty;
    public string EmailCliente { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public bool Pago { get; set; }

    public ICollection<ItemPedido> ItensPedido { get; set; } = new List<ItemPedido>();
}

public class ItemPedido
{
    public int Id { get; set; }
    public int IdPedido { get; set; }
    public Pedido? Pedido { get; set; }

    public int IdProduto { get; set; }
    public Produto? Produto { get; set; }

    public int Quantidade { get; set; }
}

public class Produto
{
    public int Id { get; set; }
    public string NomeProduto { get; set; } = string.Empty;
    public decimal Valor { get; set; }
}
