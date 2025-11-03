using Pedidos.Application.DTOs;
using Pedidos.Domain;

namespace Pedidos.Application.Services;

/// <summary>
/// Adapter/Mapper entre entidades de domínio e DTOs da aplicação.
/// </summary>
public static class PedidoMapper
{
    public static PedidoDto MapToDto(Pedido pedido)
    {
        var itens = pedido.ItensPedido.Select(i => new ItemPedidoDto(
            i.Id,
            i.IdProduto,
            i.Produto?.NomeProduto ?? string.Empty,
            i.Produto?.Valor ?? 0m,
            i.Quantidade
        )).ToList();

        var valorTotal = itens.Sum(x => x.ValorUnitario * x.Quantidade);

        return new PedidoDto(
            pedido.Id,
            pedido.NomeCliente,
            pedido.EmailCliente,
            pedido.Pago,
            valorTotal,
            itens
        );
    }
}