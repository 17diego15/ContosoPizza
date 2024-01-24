using ContosoPizza.Models;

namespace ContosoPizza.Data;

    public interface IPizzaRepository
    {
        List<Pizza> GetAll();
        Pizza? Get(int id);
        void Add(Pizza pizza);
        void Delete(int id);
        void Put(Pizza pizza);
    }
