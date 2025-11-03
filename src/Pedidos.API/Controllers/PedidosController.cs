using Microsoft.AspNetCore.Mvc;
using Pedidos.Application.DTOs;
using Pedidos.Application.Interfaces;

namespace Pedidos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidosController(IPedidoService service, ILogger<PedidosController> logger) : ControllerBase
{
    private readonly IPedidoService _service = service;
    private readonly ILogger<PedidosController> _logger = logger;

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<PedidoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var pedidos = await _service.GetAllAsync(ct);
        return Ok(pedidos);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PedidoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var pedido = await _service.GetByIdAsync(id, ct);
        if (pedido is null) return NotFound();
        return Ok(pedido);
    }

    [HttpPost]
    [ProducesResponseType(typeof(PedidoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CriarPedidoRequest request, CancellationToken ct)
    {
        var id = await _service.CreateAsync(request, ct);
        var created = await _service.GetByIdAsync(id, ct);
        return CreatedAtAction(nameof(GetById), new { id }, created);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] AtualizarPedidoRequest request, CancellationToken ct)
    {
        var ok = await _service.UpdateAsync(id, request, ct);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var ok = await _service.DeleteAsync(id, ct);
        return ok ? NoContent() : NotFound();
    }
}