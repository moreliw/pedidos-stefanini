using Microsoft.Extensions.Logging;
using Pedidos.Application.DTOs;
using Pedidos.Application.Interfaces;
using Pedidos.Domain;

namespace Pedidos.Application.Services;

public class PedidoService : IPedidoService
{
    private readonly IPedidoRepository _pedidoRepo;
    private readonly IProdutoRepository _produtoRepo;
    private readonly ILogger<PedidoService> _logger;
    private readonly IUnitOfWork _uow;

    public PedidoService(IPedidoRepository pedidoRepo, IProdutoRepository produtoRepo, ILogger<PedidoService> logger, IUnitOfWork uow)
    {
        _pedidoRepo = pedidoRepo;
        _produtoRepo = produtoRepo;
        _logger = logger;
        _uow = uow;
    }

    public async Task<PedidoDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var pedido = await _pedidoRepo.GetByIdWithItensAsync(id, ct);
        if (pedido is null) return null;
        return PedidoMapper.MapToDto(pedido);
    }

    public async Task<IReadOnlyList<PedidoDto>> GetAllAsync(CancellationToken ct = default)
    {
        var pedidos = await _pedidoRepo.GetAllWithItensAsync(ct);
        return pedidos.Select(PedidoMapper.MapToDto).ToList();
    }

    public async Task<int> CreateAsync(CriarPedidoRequest request, CancellationToken ct = default)
    {
        if (request.Itens is null || request.Itens.Count == 0)
            throw new ArgumentException("Pedido deve conter ao menos um item.");

        var pedido = new Pedido
        {
            NomeCliente = request.NomeCliente.Trim(),
            EmailCliente = request.EmailCliente.Trim(),
            Pago = request.Pago,
            DataCriacao = DateTime.UtcNow
        };

        foreach (var item in request.Itens)
        {
            var produto = await _produtoRepo.GetByIdAsync(item.IdProduto, ct);
            if (produto is null)
                throw new ArgumentException($"Produto {item.IdProduto} não encontrado.");

            pedido.ItensPedido.Add(new ItemPedido
            {
                IdProduto = item.IdProduto,
                Quantidade = item.Quantidade
            });
        }

        await _pedidoRepo.AddAsync(pedido, ct);
        await _uow.SaveChangesAsync(ct);
        _logger.LogInformation("Pedido criado com Id {Id}", pedido.Id);
        return pedido.Id;
    }

    public async Task<bool> UpdateAsync(int id, AtualizarPedidoRequest request, CancellationToken ct = default)
    {
        var exists = await _pedidoRepo.ExistsAsync(id, ct);
        if (!exists) return false;

        var pedido = await _pedidoRepo.GetByIdWithItensAsync(id, ct);
        if (pedido is null) return false;

        pedido.NomeCliente = request.NomeCliente.Trim();
        pedido.EmailCliente = request.EmailCliente.Trim();
        pedido.Pago = request.Pago;

        pedido.ItensPedido.Clear();
        foreach (var item in request.Itens)
        {
            var produto = await _produtoRepo.GetByIdAsync(item.IdProduto, ct);
            if (produto is null)
                throw new ArgumentException($"Produto {item.IdProduto} não encontrado.");

            pedido.ItensPedido.Add(new ItemPedido
            {
                IdProduto = item.IdProduto,
                Quantidade = item.Quantidade
            });
        }

        await _uow.SaveChangesAsync(ct);
        _logger.LogInformation("Pedido {Id} atualizado", id);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var deleted = await _pedidoRepo.DeleteAsync(id, ct);
        if (!deleted) return false;
        await _uow.SaveChangesAsync(ct);
        _logger.LogInformation("Pedido {Id} removido", id);
        return true;
    }

    // Mapeamento movido para PedidoMapper (Adapter)
}