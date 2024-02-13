using Microsoft.EntityFrameworkCore;
using ContosoPizza.Models;
using System.Collections.Generic;
using System.Linq;
using ContosoPizza.Data;

namespace ContosoPizza.Data
{
    public class PizzaEFRRepository : IPizzaRepository
    {
        private readonly ContosoPizzaContext _context;

        public PizzaEFRRepository(ContosoPizzaContext context)
        {
            _context = context;
        }

        public List<Pizza> GetAll()
        {
            var pizzas = _context.Pizzas.ToList();

            foreach (var pizza in pizzas)
            {
                var ingredienteIds = _context.PizzaIngredientes
                    .Where(pi => pi.PizzaId == pizza.Id)
                    .Select(pi => pi.IngredienteId)
                    .ToList();

                pizza.Ingredients = _context.Ingredientes
                    .Where(i => ingredienteIds.Contains(i.Id))
                    .ToList();
            }

            return pizzas;
        }
        public Pizza Get(int id)
        {
            var pizza = _context.Pizzas.FirstOrDefault(p => p.Id == id);

            if (pizza != null)
            {
                var ingredienteIds = _context.PizzaIngredientes
                                             .Where(pi => pi.PizzaId == pizza.Id)
                                             .Select(pi => pi.IngredienteId)
                                             .ToList();

                pizza.Ingredients = _context.Ingredientes
                                            .Where(i => ingredienteIds.Contains(i.Id))
                                            .ToList();
            }

            return pizza;
        }

        public void Add(Pizza pizza)
        {
            _context.Pizzas.Add(pizza);
            _context.SaveChanges();

            foreach (var ingrediente in pizza.Ingredients)
            {
                var pizzaIngrediente = new PizzaIngrediente
                {
                    PizzaId = pizza.Id,
                    IngredienteId = ingrediente.Id
                };

                _context.PizzaIngredientes.Add(pizzaIngrediente);
            }
            _context.SaveChanges();
        }


        public void Delete(int id)
        {
            var pizza = _context.Pizzas.FirstOrDefault(pizza => pizza.Id == id);
            if (pizza != null)
            {
                _context.Pizzas.Remove(pizza);
                _context.SaveChanges();
            }
        }

        public void Put(Pizza pizza)
        {
            _context.Pizzas.Update(pizza);
            _context.SaveChanges();
        }
    }
}
