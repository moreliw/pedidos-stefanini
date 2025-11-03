using Microsoft.EntityFrameworkCore;
using Pedidos.Application.Interfaces;
using Pedidos.Domain;
using Pedidos.Infrastructure.Persistence;

namespace Pedidos.Infrastructure.Repositories;

public class ProdutoRepository : IProdutoRepository
{
    private readonly PedidosDbContext _db;
    public ProdutoRepository(PedidosDbContext db) => _db = db;

    public async Task<IReadOnlyList<Produto>> GetAllAsync(CancellationToken ct = default)
        => await _db.Produtos.AsNoTracking().ToListAsync(ct);

    public async Task<Produto?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _db.Produtos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
}