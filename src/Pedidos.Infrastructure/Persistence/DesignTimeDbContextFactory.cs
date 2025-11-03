using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Pedidos.Infrastructure.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<PedidosDbContext>
{
    public PedidosDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PedidosDbContext>();
        var connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=PedidosStefaniniDb;Trusted_Connection=True;TrustServerCertificate=True";
        optionsBuilder.UseSqlServer(connectionString);
        return new PedidosDbContext(optionsBuilder.Options);
    }
}