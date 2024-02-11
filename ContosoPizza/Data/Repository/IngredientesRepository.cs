using ContosoPizza.Data;
using ContosoPizza.Models;

public class IngredientesRepository : IIngredientesRepository
{

    private List<Ingrediente> Ingredientes { get; set; }
    private int nextId = 3;

    public IngredientesRepository()
    {
        Ingredientes = new List<Ingrediente>
        {
            new Ingrediente { Id = 0, Nombre = "Mozzarella", Precio = 2.5M, Calorias = 100 },
            new Ingrediente { Id = 1, Nombre = "Pimientos", Precio = 1M, Calorias = 30  },
            new Ingrediente { Id = 2, Nombre = "Champiñones", Precio = 1.2M, Calorias = 20 }
        };
    }

    public List<Ingrediente> GetAll() => Ingredientes;

    public Ingrediente? Get(int id) => Ingredientes.FirstOrDefault(p => p.Id == id);

    public List<Ingrediente> GetIngredientes() => GetAll();


    public void Add(Ingrediente ingrediente)
    {
        ingrediente.Id = nextId++;
        Ingredientes.Add(ingrediente);
    }

    public void Delete(int id)
    {
        var ingrediente = Get(id);
        if (ingrediente is null)
            return;

        Ingredientes.Remove(ingrediente);
    }

    public void Put(Ingrediente ingrediente)
    {
        var index = Ingredientes.FindIndex(p => p.Id == ingrediente.Id);
        if (index == -1)
            return;

        Ingredientes[index] = ingrediente;
    }

    public void Add(Ingrediente ingrediente, int id)
    {
        throw new NotImplementedException();
    }
}