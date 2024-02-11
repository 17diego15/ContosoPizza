using ContosoPizza.Data;
using ContosoPizza.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Data
{
    public class IngredientesEFRRepository : IIngredientesRepository
    {
        private readonly ContosoPizzaContext _context;

        public IngredientesEFRRepository(ContosoPizzaContext context)
        {
            _context = context;
        }

        public List<Ingrediente> GetAll()
        {
            return _context.Ingredientes.ToList();
        }

        public Ingrediente Get(int Id)
        {
            return _context.Ingredientes.FirstOrDefault(ingredientes => ingredientes.Id == Id);
        }


        public void Add(Ingrediente ingredientes)
        {
            _context.Ingredientes.Add(ingredientes);
            SaveChanges();
        }

        public void Delete(int id)
        {
            var ingredientes = _context.Ingredientes.FirstOrDefault(ingredientes => ingredientes.Id == id);
            if (ingredientes != null)
            {
                _context.Ingredientes.Remove(ingredientes);
            }
            SaveChanges();
        }

        public void Put(Ingrediente ingredientes)
        {
            _context.Ingredientes.Update(ingredientes);
            SaveChanges();
        }


        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public List<Ingrediente> GetIngredientes() => GetAll();

    }
}