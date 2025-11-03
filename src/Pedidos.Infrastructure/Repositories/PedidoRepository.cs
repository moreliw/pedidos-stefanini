using Microsoft.EntityFrameworkCore;
using Pedidos.Application.Interfaces;
using Pedidos.Domain;
using Pedidos.Infrastructure.Persistence;

namespace Pedidos.Infrastructure.Repositories;

public class PedidoRepository : IPedidoRepository
{
    private readonly PedidosDbContext _db;
    public PedidoRepository(PedidosDbContext db) => _db = db;

    public async Task AddAsync(Pedido pedido, CancellationToken ct = default)
    {
        await _db.Pedidos.AddAsync(pedido, ct);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var pedido = await _db.Pedidos.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (pedido is null) return false;
        _db.Pedidos.Remove(pedido);
        return true;
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
    {
        return await _db.Pedidos.AnyAsync(x => x.Id == id, ct);
    }

    public async Task<IReadOnlyList<Pedido>> GetAllWithItensAsync(CancellationToken ct = default)
    {
        return await _db.Pedidos
            .Include(p => p.ItensPedido)
            .ThenInclude(i => i.Produto)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<Pedido?> GetByIdWithItensAsync(int id, CancellationToken ct = default)
    {
        return await _db.Pedidos
            .Include(p => p.ItensPedido)
            .ThenInclude(i => i.Produto)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await _db.SaveChangesAsync(ct);
    }
}