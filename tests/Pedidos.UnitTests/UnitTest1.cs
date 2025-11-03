using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Pedidos.API.Controllers;
using Pedidos.Application.DTOs;
using Pedidos.Application.Services;
using Pedidos.Infrastructure.Persistence;
using Pedidos.Infrastructure.Repositories;

namespace Pedidos.UnitTests;

public class PedidoTests
{
    private static PedidosDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<PedidosDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var ctx = new PedidosDbContext(options);

        ctx.Produtos.AddRange([
            new Pedidos.Domain.Produto { Id = 1, NomeProduto = "Produto A", Valor = 10.00m },
            new Pedidos.Domain.Produto { Id = 2, NomeProduto = "Produto B", Valor = 100.00m }
        ]);
        ctx.SaveChanges();

        var pedido = new Pedidos.Domain.Pedido
        {
            NomeCliente = "John Doe",
            EmailCliente = "john@example.com",
            Pago = true,
            DataCriacao = DateTime.UtcNow
        };
        ctx.Pedidos.Add(pedido);
        ctx.SaveChanges();

        ctx.ItensPedido.AddRange([
            new Pedidos.Domain.ItemPedido { IdPedido = pedido.Id, IdProduto = 1, Quantidade = 2 },
            new Pedidos.Domain.ItemPedido { IdPedido = pedido.Id, IdProduto = 2, Quantidade = 1 }
        ]);
        ctx.SaveChanges();

        return ctx;
    }

    [Fact]
    public async Task Service_GetById_Should_Return_JSON_Model_With_Total()
    {
        using var ctx = CreateInMemoryContext();
        var pedidoRepo = new PedidoRepository(ctx);
        var produtoRepo = new ProdutoRepository(ctx);
        var logger = Mock.Of<ILogger<PedidoService>>();
        var service = new PedidoService(pedidoRepo, produtoRepo, logger);

        var pedidoId = await ctx.Pedidos.Select(p => p.Id).FirstAsync();

        var dto = await service.GetByIdAsync(pedidoId);

        dto.Should().NotBeNull();
        dto!.Id.Should().Be(pedidoId);
        dto!.NomeCliente.Should().Be("John Doe");
        dto!.ItensPedido.Should().HaveCount(2);
        dto!.ValorTotal.Should().Be(120.00m);
        dto!.ItensPedido[0].Should().NotBeNull();
        dto!.ItensPedido[0].NomeProduto.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Controller_GetById_Returns_Ok_With_Dto()
    {
        var serviceMock = new Mock<Pedidos.Application.Interfaces.IPedidoService>();
        var dto = new PedidoDto(1, "Cliente", "cliente@email.com", true, 10m,
            new List<ItemPedidoDto> { new(1, 1, "Produto", 10m, 1) });
        serviceMock.Setup(s => s.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(dto);

        var controller = new PedidosController(serviceMock.Object, Mock.Of<ILogger<PedidosController>>());

        var result = await controller.GetById(1, CancellationToken.None) as Microsoft.AspNetCore.Mvc.OkObjectResult;  

        result.Should().NotBeNull();
        var returned = result!.Value as PedidoDto;
        returned.Should().NotBeNull();
        returned!.Id.Should().Be(1);
        returned!.ItensPedido.Should().HaveCount(1);
    }
}