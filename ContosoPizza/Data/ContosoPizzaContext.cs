using Microsoft.EntityFrameworkCore;

using ContosoPizza.Models;
using Microsoft.Extensions.Configuration;

namespace ContosoPizza.Data
{
    public class ContosoPizzaContext : DbContext
    {

        public ContosoPizzaContext(DbContextOptions<ContosoPizzaContext> options)
            : base(options)
        {

        }

        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Ingrediente> Ingredientes { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<PizzaIngrediente> PizzaIngredientes { get; set; }
        public DbSet<PizzaPedido> PizzaPedidos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PizzaIngrediente>()
        .HasKey(pi => new { pi.PizzaId, pi.IngredienteId });

            modelBuilder.Entity<PizzaIngrediente>()
                .HasOne<Pizza>()
                .WithMany()
                .HasForeignKey(pi => pi.PizzaId);

            modelBuilder.Entity<PizzaIngrediente>()
                .HasOne<Ingrediente>()
                .WithMany()
                .HasForeignKey(pi => pi.IngredienteId);

            modelBuilder.Entity<PizzaPedido>()
           .HasKey(pi => new { pi.PizzaId, pi.PedidoId });

            modelBuilder.Entity<PizzaPedido>()
                .HasOne<Pizza>()
                .WithMany()
                .HasForeignKey(pi => pi.PizzaId);

            modelBuilder.Entity<PizzaPedido>()
                .HasOne<Pedido>()
                .WithMany()
                .HasForeignKey(pi => pi.PedidoId);
            modelBuilder.Entity<Pizza>().HasData(
                new Pizza { Id = 1, Name = "Classic Italian", Price = 7.02m },
                new Pizza { Id = 2, Name = "Vegetariana", Price = 6.77m }
                );
            modelBuilder.Entity<Ingrediente>().HasData(
                new Ingrediente { Id = 1, Nombre = "Tomate", Precio = 1, Calorias = 21 },
                new Ingrediente { Id = 2, Nombre = "Queso", Precio = 1.2M, Calorias = 43 }
            );
            modelBuilder.Entity<Usuario>().HasData(
               new Usuario { Id = 1, Nombre = "Diego Gimenez Sancho", Dirección = "Poeta Leon Felipe" }
           );
            modelBuilder.Entity<PizzaIngrediente>().HasData(
                new PizzaIngrediente { PizzaId = 1, IngredienteId = 1 },
                new PizzaIngrediente { PizzaId = 1, IngredienteId = 2 },
                new PizzaIngrediente { PizzaId = 2, IngredienteId = 2 }
            );
        }
    }
}