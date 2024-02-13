using ContosoPizza.Models;
using ContosoPizza.Data;
using System.Collections.Generic;

namespace ContosoPizza.Services
{
    public class PizzaService
    {
        private readonly IPizzaRepository _pizzaRepository;
        private readonly IIngredientesRepository _ingredienteRepository;


        public PizzaService(IPizzaRepository pizzaRepository, IIngredientesRepository ingredienteRepository)
        {
            _pizzaRepository = pizzaRepository;
            _ingredienteRepository = ingredienteRepository;
        }

        public List<Pizza> GetAll()
        {
            var pizzas = _pizzaRepository.GetAll();
            foreach (var pizza in pizzas)
            {
                CalculatePrice(pizza);
            }
            return pizzas;
        }

        public Pizza? Get(int id)
        {
            var pizza = _pizzaRepository.Get(id);
            if (pizza != null)
            {
                CalculatePrice(pizza);
            }
            return pizza;
        }

        public void Add(Pizza pizza)
        {
            if (pizza.Ingredients == null)
                pizza.Ingredients = new List<Ingrediente>();

            var ingredientesValidos = new List<Ingrediente>();

            foreach (var ingrediente in pizza.Ingredients)
            {
                var ingredienteEncontrado = _ingredienteRepository.Get(ingrediente.Id);
                if (ingredienteEncontrado != null)
                {
                    ingredientesValidos.Add(ingredienteEncontrado);
                }
            }

            pizza.Ingredients = ingredientesValidos;
            CalculatePrice(pizza);

            _pizzaRepository.Add(pizza);
        }

        public void Delete(int id)
        {
            _pizzaRepository.Delete(id);
        }

        public void Put(Pizza pizza)
        {
            var ingredientesActualizados = new List<Ingrediente>();
            foreach (var ingrediente in pizza.Ingredients)
            {
                var ingredienteDetalle = _ingredienteRepository.Get(ingrediente.Id);
                if (ingredienteDetalle != null)
                {
                    ingredientesActualizados.Add(ingredienteDetalle);
                }
            }
            pizza.Ingredients = ingredientesActualizados;

            CalculatePrice(pizza);
            _pizzaRepository.Put(pizza);
        }

        private void CalculatePrice(Pizza pizza)
        {
            if (pizza == null || pizza.Ingredients == null || !pizza.Ingredients.Any())
                return;

            decimal precioTotalIngredientes = 0;

            foreach (var ingrediente in pizza.Ingredients)
            {
                var ingredienteDetalle = _ingredienteRepository.Get(ingrediente.Id);
                if (ingredienteDetalle != null)
                {
                    precioTotalIngredientes += ingredienteDetalle.Precio;
                }
            }

            pizza.Price = precioTotalIngredientes * 3;
        }
    }
}
