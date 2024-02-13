﻿// <auto-generated />
using System;
using ContosoPizza.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ContosoPizza.Data.Migrations
{
    [DbContext(typeof(ContosoPizzaContext))]
    partial class ContosoPizzaContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ContosoPizza.Models.Ingrediente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Calorias")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PizzaId")
                        .HasColumnType("int");

                    b.Property<decimal>("Precio")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("PizzaId");

                    b.ToTable("Ingredientes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Calorias = 21m,
                            Nombre = "Tomate",
                            Precio = 1m
                        },
                        new
                        {
                            Id = 2,
                            Calorias = 43m,
                            Nombre = "Queso",
                            Precio = 1.2m
                        });
                });

            modelBuilder.Entity("ContosoPizza.Models.Pedido", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("FechaPedido")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Precio")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Pedidos");
                });

            modelBuilder.Entity("ContosoPizza.Models.Pizza", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsGlutenFree")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PedidoId")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("PedidoId");

                    b.ToTable("Pizzas");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IsGlutenFree = false,
                            Name = "Classic Italian",
                            Price = 7.02m
                        },
                        new
                        {
                            Id = 2,
                            IsGlutenFree = false,
                            Name = "Vegetariana",
                            Price = 6.77m
                        });
                });

            modelBuilder.Entity("ContosoPizza.Models.PizzaIngrediente", b =>
                {
                    b.Property<int>("PizzaId")
                        .HasColumnType("int");

                    b.Property<int>("IngredienteId")
                        .HasColumnType("int");

                    b.HasKey("PizzaId", "IngredienteId");

                    b.HasIndex("IngredienteId");

                    b.ToTable("PizzaIngredientes");

                    b.HasData(
                        new
                        {
                            PizzaId = 1,
                            IngredienteId = 1
                        },
                        new
                        {
                            PizzaId = 1,
                            IngredienteId = 2
                        },
                        new
                        {
                            PizzaId = 2,
                            IngredienteId = 2
                        });
                });

            modelBuilder.Entity("ContosoPizza.Models.PizzaPedido", b =>
                {
                    b.Property<int>("PizzaId")
                        .HasColumnType("int");

                    b.Property<int>("PedidoId")
                        .HasColumnType("int");

                    b.HasKey("PizzaId", "PedidoId");

                    b.HasIndex("PedidoId");

                    b.ToTable("PizzaPedidos");
                });

            modelBuilder.Entity("ContosoPizza.Models.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Dirección")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Usuarios");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Dirección = "Poeta Leon Felipe",
                            Nombre = "Diego Gimenez Sancho"
                        });
                });

            modelBuilder.Entity("ContosoPizza.Models.Ingrediente", b =>
                {
                    b.HasOne("ContosoPizza.Models.Pizza", null)
                        .WithMany("Ingredients")
                        .HasForeignKey("PizzaId");
                });

            modelBuilder.Entity("ContosoPizza.Models.Pedido", b =>
                {
                    b.HasOne("ContosoPizza.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("ContosoPizza.Models.Pizza", b =>
                {
                    b.HasOne("ContosoPizza.Models.Pedido", null)
                        .WithMany("Pizzas")
                        .HasForeignKey("PedidoId");
                });

            modelBuilder.Entity("ContosoPizza.Models.PizzaIngrediente", b =>
                {
                    b.HasOne("ContosoPizza.Models.Ingrediente", null)
                        .WithMany()
                        .HasForeignKey("IngredienteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ContosoPizza.Models.Pizza", null)
                        .WithMany()
                        .HasForeignKey("PizzaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ContosoPizza.Models.PizzaPedido", b =>
                {
                    b.HasOne("ContosoPizza.Models.Pedido", null)
                        .WithMany()
                        .HasForeignKey("PedidoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ContosoPizza.Models.Pizza", null)
                        .WithMany()
                        .HasForeignKey("PizzaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ContosoPizza.Models.Pedido", b =>
                {
                    b.Navigation("Pizzas");
                });

            modelBuilder.Entity("ContosoPizza.Models.Pizza", b =>
                {
                    b.Navigation("Ingredients");
                });
#pragma warning restore 612, 618
        }
    }
}