using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pedidos.Application.Interfaces;
using Pedidos.Application.Services;
using Pedidos.Infrastructure.Persistence;
using Pedidos.Infrastructure.Repositories;

namespace Pedidos.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? "Server=(localdb)\\MSSQLLocalDB;Database=PedidosStefaniniDb;Trusted_Connection=True;TrustServerCertificate=True";

        services.AddDbContext<PedidosDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IPedidoRepository, PedidoRepository>();
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IPedidoService, PedidoService>();

        return services;
    }
}