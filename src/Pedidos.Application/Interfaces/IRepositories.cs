using Pedidos.Domain;

namespace Pedidos.Application.Interfaces;

public interface IPedidoRepository
{
    Task<Pedido?> GetByIdWithItensAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<Pedido>> GetAllWithItensAsync(CancellationToken ct = default);
    Task AddAsync(Pedido pedido, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}

public interface IProdutoRepository
{
    Task<Produto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<Produto>> GetAllAsync(CancellationToken ct = default);
}