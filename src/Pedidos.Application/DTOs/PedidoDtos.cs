namespace Pedidos.Application.DTOs;

public record ItemPedidoDto(
    int Id,
    int IdProduto,
    string NomeProduto,
    decimal ValorUnitario,
    int Quantidade
);

public record PedidoDto(
    int Id,
    string NomeCliente,
    string EmailCliente,
    bool Pago,
    decimal ValorTotal,
    IReadOnlyList<ItemPedidoDto> ItensPedido
);

public record CriarItemPedidoRequest(int IdProduto, int Quantidade);

public record CriarPedidoRequest(
    string NomeCliente,
    string EmailCliente,
    bool Pago,
    IReadOnlyList<CriarItemPedidoRequest> Itens
);

public record AtualizarPedidoRequest(
    string NomeCliente,
    string EmailCliente,
    bool Pago,
    IReadOnlyList<CriarItemPedidoRequest> Itens
);