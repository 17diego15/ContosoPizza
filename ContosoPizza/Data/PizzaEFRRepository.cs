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
          
            return _context.Pizzas.Include(p => p.Ingredients).ToList();
        }

        public Pizza Get(int Id)
        {
            
            return _context.Pizzas.Include(p => p.Ingredients)
                                  .FirstOrDefault(pizza => pizza.Id == Id);
        }

        public void Add(Pizza pizza)
        {
            _context.Pizzas.Add(pizza);
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
