using System.Threading;
using System.Threading.Tasks;
using Pedidos.Application.Interfaces;

namespace Pedidos.Infrastructure.Persistence;

/// <summary>
/// Implementação de Unit of Work baseada em PedidosDbContext.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly PedidosDbContext _dbContext;
    public UnitOfWork(PedidosDbContext dbContext) => _dbContext = dbContext;

    public async Task SaveChangesAsync(CancellationToken ct = default)
        => await _dbContext.SaveChangesAsync(ct);
}