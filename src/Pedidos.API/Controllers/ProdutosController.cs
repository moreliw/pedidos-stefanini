using Microsoft.AspNetCore.Mvc;
using Pedidos.Application.Interfaces;

namespace Pedidos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutosController(IProdutoRepository repo) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var produtos = await repo.GetAllAsync(ct);
        return Ok(produtos);
    }
}