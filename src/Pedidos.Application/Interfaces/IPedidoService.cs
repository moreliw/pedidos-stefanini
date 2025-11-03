using Pedidos.Application.DTOs;

namespace Pedidos.Application.Interfaces;

public interface IPedidoService
{
    Task<PedidoDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<PedidoDto>> GetAllAsync(CancellationToken ct = default);
    Task<int> CreateAsync(CriarPedidoRequest request, CancellationToken ct = default);
    Task<bool> UpdateAsync(int id, AtualizarPedidoRequest request, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}