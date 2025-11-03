using System.Threading;
using System.Threading.Tasks;

namespace Pedidos.Application.Interfaces;

/// <summary>
/// Padrão Unit of Work para coordenar gravações no banco de dados.
/// </summary>
public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken ct = default);
}