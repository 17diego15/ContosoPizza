using Microsoft.EntityFrameworkCore;
using ContosoPizza.Models;
using System.Collections.Generic;
using System.Linq;
using ContosoPizza.Data;

namespace ContosoPizza.Data
{
    public class PedidosEFRRepository : IPedidoRepository
    {
        private readonly ContosoPizzaContext _context;

        public PedidosEFRRepository(ContosoPizzaContext context)
        {
            _context = context;
        }

        public List<Pedido> GetAll()
        {
            var pedidos = _context.Pedidos
                .Include(pedido => pedido.Usuario)
                .ToList();

            foreach (var pedido in pedidos)
            {
                var pizzaIds = _context.PizzaPedidos
                    .Where(pp => pp.PedidoId == pedido.Id)
                    .Select(pp => pp.PizzaId)
                    .ToList();

                pedido.Pizzas = _context.Pizzas
                    .Where(pizza => pizzaIds.Contains(pizza.Id))
                    .ToList();

                foreach (var pizza in pedido.Pizzas)
                {
                    var ingredienteIds = _context.PizzaIngredientes
                        .Where(pi => pi.PizzaId == pizza.Id)
                        .Select(pi => pi.IngredienteId)
                        .ToList();

                    pizza.Ingredients = _context.Ingredientes
                        .Where(ingrediente => ingredienteIds.Contains(ingrediente.Id))
                        .ToList();
                }
            }

            return pedidos;
        }

        public Pedido Get(int id)
        {
            var pedido = _context.Pedidos
                .Include(p => p.Usuario)
                .FirstOrDefault(pedido => pedido.Id == id);

            if (pedido != null)
            {
                var pizzas = _context.PizzaPedidos
                    .Where(pp => pp.PedidoId == pedido.Id)
                    .Select(pp => pp.PizzaId)
                    .ToList();

                pedido.Pizzas = _context.Pizzas
                    .Where(p => pizzas.Contains(p.Id))
                    .ToList();

                foreach (var pizza in pedido.Pizzas)
                {
                    var ingredientes = _context.PizzaIngredientes
                        .Where(pi => pi.PizzaId == pizza.Id)
                        .Select(pi => pi.IngredienteId)
                        .ToList();

                    pizza.Ingredients = _context.Ingredientes
                        .Where(i => ingredientes.Contains(i.Id))
                        .ToList();
                }
            }

            return pedido;
        }

        public void Add(Pedido pedido)
        {
            _context.Pedidos.Add(pedido);
            _context.SaveChanges();

            foreach (var pizza in pedido.Pizzas)
            {
                var pizzaPedido = new PizzaPedido
                {
                    PedidoId = pedido.Id,
                    PizzaId = pizza.Id
                };

                _context.PizzaPedidos.Add(pizzaPedido);
            }

            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var pedido = _context.Pedidos.FirstOrDefault(pedido => pedido.Id == id);
            if (pedido != null)
            {
                _context.Pedidos.Remove(pedido);
            }
            SaveChanges();
        }

        public void Put(Pedido pedido)
        {
            _context.Pedidos.Update(pedido);
            SaveChanges();
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public List<Pedido> GetPedidosByUsuarioId(int usuarioId)
        {
            var pedidosDelUsuario = _context.Pedidos
                .Where(pedido => pedido.Id == usuarioId)
                .Include(pedido => pedido.Usuario)
                .ToList();

            foreach (var pedido in pedidosDelUsuario)
            {
                var pizzaIds = _context.PizzaPedidos
                    .Where(pp => pp.PedidoId == pedido.Id)
                    .Select(pp => pp.PizzaId)
                    .ToList();

                pedido.Pizzas = _context.Pizzas
                    .Where(pizza => pizzaIds.Contains(pizza.Id))
                    .ToList();

                foreach (var pizza in pedido.Pizzas)
                {
                    var ingredienteIds = _context.PizzaIngredientes
                        .Where(pi => pi.PizzaId == pizza.Id)
                        .Select(pi => pi.IngredienteId)
                        .ToList();

                    pizza.Ingredients = _context.Ingredientes
                        .Where(ingrediente => ingredienteIds.Contains(ingrediente.Id))
                        .ToList();
                }
            }
            return pedidosDelUsuario;
        }
    }
}