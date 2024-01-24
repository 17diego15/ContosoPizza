using ContosoPizza.Models;

namespace ContosoPizza.Data;

    public interface IIngredientesRepository
    {
        List<Ingrediente> GetAll();
        Ingrediente? Get(int id);
        List<Ingrediente> GetIngredientes();
        void Add(Ingrediente ingrediente);
        void Delete(int id);
        void Put(Ingrediente ingrediente);
    }
