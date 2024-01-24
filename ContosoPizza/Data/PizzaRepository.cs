using ContosoPizza.Models;
using System.Collections.Generic;
using System.Linq;

namespace ContosoPizza.Data;

public class PizzaRepository : IPizzaRepository
{
    private List<Pizza> Pizzas { get; set; }
    private int nextId = 3;

    private IIngredientesRepository _ingredientesRepository;


    public PizzaRepository(IIngredientesRepository ingredientesRepository)
    {
        Pizzas = new List<Pizza>
        {
            new Pizza { Id = 0, Name = "Classic Italian", Ingredients = new List<Ingrediente> { new Ingrediente { Id = 1}, new Ingrediente { Id = 2} }, IsGlutenFree = false },
            new Pizza { Id = 1, Name = "Veggie", Ingredients = new List<Ingrediente> { new Ingrediente { Id = 0}, new Ingrediente { Id = 1}, new Ingrediente { Id = 2} }, IsGlutenFree = true },
            new Pizza { Id = 2, Name = "BBQ",Ingredients = new List<Ingrediente> {new Ingrediente { Id = 0}} , IsGlutenFree = false }
        };
        _ingredientesRepository = ingredientesRepository;

    }

    public List<Pizza> GetAll()
    {
        foreach (var pizza in Pizzas)
        {
            pizza.Ingredients = pizza.Ingredients.Select(ingrediente => _ingredientesRepository.Get(ingrediente.Id)).ToList();
        }

        return Pizzas;
    }

    public Pizza? Get(int id) => Pizzas.FirstOrDefault(p => p.Id == id);

    public void Add(Pizza pizza)
    {
        pizza.Id = nextId++;
        Pizzas.Add(pizza);
    }


    public void Delete(int id)
    {
        var pizza = Get(id);
        if (pizza is null)
            return;

        Pizzas.Remove(pizza);
    }

    public void Put(Pizza pizza)
    {
        var index = Pizzas.FindIndex(p => p.Id == pizza.Id);
        if (index == -1)
            return;

        Pizzas[index] = pizza;
    }
}
