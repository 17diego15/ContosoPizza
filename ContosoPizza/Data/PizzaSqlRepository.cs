using ContosoPizza.Models;
using System.Data.SqlTypes;
using System.Text.Json;
using System.Data;
using System.Data.SqlClient;


namespace ContosoPizza.Data;

public class PizzaSqlRepository : IPizzaRepository
{
    private readonly string _connectionString;

    public PizzaSqlRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<Pizza> GetAll()
    {
        var pizzas = new List<Pizza>();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            var sqlString = "SELECT PizzaId, Nombre, Precio, IsGluten FROM Pizza";
            var command = new SqlCommand(sqlString, connection);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var pizza = new Pizza
                    {
                        Id = Convert.ToInt32(reader["PizzaId"]),
                        Name = reader["Nombre"].ToString(),
                        Price = Convert.ToDecimal(reader["Precio"]),
                        IsGlutenFree = Convert.ToBoolean(reader["IsGluten"])
                    };
                    pizzas.Add(pizza);
                }
            }
        }

        return pizzas;
    }

    public Pizza? Get(int id)
    {
        Pizza pizza = null;

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            // Obtener los detalles de la pizza
            var pizzaSqlString = "SELECT PizzaId, Nombre, Precio, IsGluten FROM Pizza WHERE PizzaId = @PizzaId";
            using (var pizzaCommand = new SqlCommand(pizzaSqlString, connection))
            {
                pizzaCommand.Parameters.AddWithValue("@PizzaId", id);
                using (var pizzaReader = pizzaCommand.ExecuteReader())
                {
                    if (pizzaReader.Read()) // Si hay una pizza, solo debería haber una debido al PRIMARY KEY
                    {
                        pizza = new Pizza
                        {
                            Id = Convert.ToInt32(pizzaReader["PizzaId"]),
                            Name = pizzaReader["Nombre"].ToString(),
                            Price = Convert.ToDecimal(pizzaReader["Precio"]),
                            IsGlutenFree = Convert.ToBoolean(pizzaReader["IsGluten"]),
                            Ingredients = new List<Ingrediente>() // Inicializar la lista de ingredientes
                        };
                    }
                }
            }

            // Si encontramos una pizza, ahora obtenemos sus ingredientes
            if (pizza != null)
            {
                var ingredientsSqlString = @"
                SELECT i.IngredienteId, i.Nombre, i.Precio, i.Calorias
                FROM Ingrediente i
                INNER JOIN PizzaIngrediente pi ON i.IngredienteId = pi.IngredienteId
                WHERE pi.PizzaId = @PizzaId";

                using (var ingredientsCommand = new SqlCommand(ingredientsSqlString, connection))
                {
                    ingredientsCommand.Parameters.AddWithValue("@PizzaId", id);
                    using (var ingredientsReader = ingredientsCommand.ExecuteReader())
                    {
                        while (ingredientsReader.Read())
                        {
                            var ingrediente = new Ingrediente
                            {
                                Id = Convert.ToInt32(ingredientsReader["IngredienteId"]),
                                Nombre = ingredientsReader["Nombre"].ToString(),
                                Precio = Convert.ToDecimal(ingredientsReader["Precio"]),
                                Calorias = Convert.ToInt32(ingredientsReader["Calorias"])
                            };
                            pizza.Ingredients.Add(ingrediente);
                        }
                    }
                }
            }
        }

        return pizza;
    }

    public void Add(Pizza pizza)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            var sqlString = "INSERT INTO Pizza (Nombre, Precio, IsGluten) VALUES (@Nombre, @Precio, @IsGluten)";
            var command = new SqlCommand(sqlString, connection);

            command.Parameters.AddWithValue("@Nombre", pizza.Name);
            command.Parameters.AddWithValue("@Precio", pizza.Price);
            command.Parameters.AddWithValue("@IsGluten", pizza.IsGlutenFree ? 1 : 0);

            command.ExecuteNonQuery();
        }
    }

    public void Delete(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            var sqlString = "DELETE FROM Pizza WHERE PizzaId = @PizzaId";
            var command = new SqlCommand(sqlString, connection);

            command.Parameters.AddWithValue("@PizzaId", id);

            command.ExecuteNonQuery();
        }
    }


    public void Put(Pizza pizza)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            var sqlString = @"
            UPDATE Pizza 
            SET Nombre = @Nombre, Precio = @Precio, IsGluten = @IsGluten 
            WHERE PizzaId = @PizzaId";

            using (var command = new SqlCommand(sqlString, connection))
            {
                command.Parameters.AddWithValue("@PizzaId", pizza.Id);
                command.Parameters.AddWithValue("@Nombre", pizza.Name);
                command.Parameters.AddWithValue("@Precio", pizza.Price);
                command.Parameters.AddWithValue("@IsGluten", pizza.IsGlutenFree ? 1 : 0);

                int rowsAffected = command.ExecuteNonQuery();
            }
        }
    }
}
