using Microsoft.EntityFrameworkCore;
using Pedidos.Domain;

namespace Pedidos.Infrastructure.Persistence;

public class PedidosDbContext : DbContext
{
    public PedidosDbContext(DbContextOptions<PedidosDbContext> options) : base(options) { }

    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<ItemPedido> ItensPedido => Set<ItemPedido>();
    public DbSet<Produto> Produtos => Set<Produto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Pedido>(cfg =>
        {
            cfg.ToTable("Pedido");
            cfg.HasKey(p => p.Id);
            cfg.Property(p => p.Id).ValueGeneratedOnAdd();

            cfg.Property(p => p.NomeCliente).HasMaxLength(60).IsRequired();
            cfg.Property(p => p.EmailCliente).HasMaxLength(60).IsRequired();
            cfg.Property(p => p.Pago).IsRequired();
            cfg.Property(p => p.DataCriacao).IsRequired();

            cfg.HasMany(p => p.ItensPedido)
               .WithOne(i => i.Pedido!)
               .HasForeignKey(i => i.IdPedido)
               .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Produto>(cfg =>
        {
            cfg.ToTable("Produto");
            cfg.HasKey(p => p.Id);
            cfg.Property(p => p.Id).ValueGeneratedOnAdd();
            cfg.Property(p => p.NomeProduto).HasMaxLength(20).IsRequired();
            cfg.Property(p => p.Valor).HasPrecision(10, 2).IsRequired();

            // Seed inicial para facilitar testes
            cfg.HasData(
                new Produto { Id = 1, NomeProduto = "Notebook", Valor = 4500.00m },
                new Produto { Id = 2, NomeProduto = "Mouse", Valor = 120.00m },
                new Produto { Id = 3, NomeProduto = "Teclado", Valor = 250.00m }
            );
        });

        modelBuilder.Entity<ItemPedido>(cfg =>
        {
            cfg.ToTable("ItensPedido");
            cfg.HasKey(i => i.Id);
            cfg.Property(i => i.Id).ValueGeneratedOnAdd();
            cfg.Property(i => i.Quantidade).IsRequired();

            cfg.HasOne(i => i.Produto)
               .WithMany()
               .HasForeignKey(i => i.IdProduto)
               .OnDelete(DeleteBehavior.Restrict);
        });
    }
}