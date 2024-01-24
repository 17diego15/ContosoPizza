using ContosoPizza.Models;
using ContosoPizza.Data;
using System.Collections.Generic;

namespace ContosoPizza.Services
{
    public class IngredientesService
    {
        private readonly IIngredientesRepository _ingredientesRepository;

        public IngredientesService(IIngredientesRepository ingredientesRepository)
        {
            _ingredientesRepository = ingredientesRepository;
        }


        public List<Ingrediente> GetAll()
        {
            return _ingredientesRepository.GetAll();
        }

        public Ingrediente? Get(int id)
        {
            return _ingredientesRepository.Get(id);
        }

        public void Add(Ingrediente ingrediente)
        {
            _ingredientesRepository.Add(ingrediente);
        }

        public void Delete(int id)
        {
            _ingredientesRepository.Delete(id);
        }

        public void Put(Ingrediente ingrediente)
        {
            _ingredientesRepository.Put(ingrediente);
        }
    }
}
