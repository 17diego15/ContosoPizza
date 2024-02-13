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

            var pizzaSqlString = "SELECT PizzaId, Nombre, Precio, IsGluten FROM Pizza";
            using (var command = new SqlCommand(pizzaSqlString, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var pizza = new Pizza
                    {
                        Id = Convert.ToInt32(reader["PizzaId"]),
                        Name = reader["Nombre"].ToString(),
                        Price = Convert.ToDecimal(reader["Precio"]),
                        IsGlutenFree = Convert.ToBoolean(reader["IsGluten"]),
                        Ingredients = new List<Ingrediente>()
                    };
                    pizzas.Add(pizza);
                }
            }

            var ingredientsSqlString = @"
            SELECT pi.PizzaId, i.IngredienteId, i.Nombre, i.Precio, i.Calorias
            FROM Ingrediente i
            INNER JOIN PizzaIngrediente pi ON i.IngredienteId = pi.IngredienteId
            ORDER BY pi.PizzaId";

            using (var ingredientsCommand = new SqlCommand(ingredientsSqlString, connection))
            using (var ingredientsReader = ingredientsCommand.ExecuteReader())
            {
                while (ingredientsReader.Read())
                {
                    var pizzaId = Convert.ToInt32(ingredientsReader["PizzaId"]);
                    var pizza = pizzas.FirstOrDefault(p => p.Id == pizzaId);
                    if (pizza != null)
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
        return pizzas;
    }

    public Pizza? Get(int id)
    {
        Pizza pizza = null;

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            var pizzaSqlString = "SELECT PizzaId, Nombre, Precio, IsGluten FROM Pizza WHERE PizzaId = @PizzaId";
            using (var pizzaCommand = new SqlCommand(pizzaSqlString, connection))
            {
                pizzaCommand.Parameters.AddWithValue("@PizzaId", id);
                using (var pizzaReader = pizzaCommand.ExecuteReader())
                {
                    if (pizzaReader.Read())
                    {
                        pizza = new Pizza
                        {
                            Id = Convert.ToInt32(pizzaReader["PizzaId"]),
                            Name = pizzaReader["Nombre"].ToString(),
                            Price = Convert.ToDecimal(pizzaReader["Precio"]),
                            IsGlutenFree = Convert.ToBoolean(pizzaReader["IsGluten"]),
                            Ingredients = new List<Ingrediente>()
                        };
                    }
                }
            }

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

            var pizzaSqlString = "INSERT INTO Pizza (Nombre, Precio, IsGluten) VALUES (@Nombre, @Precio, @IsGluten); SELECT SCOPE_IDENTITY();";
            var pizzaCommand = new SqlCommand(pizzaSqlString, connection);

            pizzaCommand.Parameters.AddWithValue("@Nombre", pizza.Name);
            pizzaCommand.Parameters.AddWithValue("@Precio", pizza.Price);
            pizzaCommand.Parameters.AddWithValue("@IsGluten", pizza.IsGlutenFree ? 1 : 0);

            var pizzaId = Convert.ToInt32(pizzaCommand.ExecuteScalar());

            foreach (var ingrediente in pizza.Ingredients)
            {
                var ingredientesSqlString = "INSERT INTO PizzaIngrediente (PizzaId, IngredienteId) VALUES (@PizzaId, @IngredienteId)";
                var ingredientesCommand = new SqlCommand(ingredientesSqlString, connection);

                ingredientesCommand.Parameters.AddWithValue("@PizzaId", pizzaId);
                ingredientesCommand.Parameters.AddWithValue("@IngredienteId", ingrediente.Id);
                ingredientesCommand.ExecuteNonQuery();
            }
        }
    }

    public void Delete(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            var deleteIngredientsSql = "DELETE FROM PizzaIngrediente WHERE PizzaId = @PizzaId";
            using (var deleteIngredientsCmd = new SqlCommand(deleteIngredientsSql, connection))
            {
                deleteIngredientsCmd.Parameters.AddWithValue("@PizzaId", id);
                deleteIngredientsCmd.ExecuteNonQuery();
            }

            var deletePizzaSql = "DELETE FROM Pizza WHERE PizzaId = @PizzaId";
            using (var deletePizzaCmd = new SqlCommand(deletePizzaSql, connection))
            {
                deletePizzaCmd.Parameters.AddWithValue("@PizzaId", id);
                deletePizzaCmd.ExecuteNonQuery();
            }
        }
    }

    public void Put(Pizza pizza)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {

                var updatePizzaSql = @"
                    UPDATE Pizza 
                    SET Nombre = @Nombre, Precio = @Precio, IsGluten = @IsGluten 
                    WHERE PizzaId = @PizzaId";

                using (var updatePizzaCmd = new SqlCommand(updatePizzaSql, connection, transaction))
                {
                    updatePizzaCmd.Parameters.AddWithValue("@PizzaId", pizza.Id);
                    updatePizzaCmd.Parameters.AddWithValue("@Nombre", pizza.Name);
                    updatePizzaCmd.Parameters.AddWithValue("@Precio", pizza.Price);
                    updatePizzaCmd.Parameters.AddWithValue("@IsGluten", pizza.IsGlutenFree ? 1 : 0);
                    updatePizzaCmd.ExecuteNonQuery();
                }

                var deleteIngredientsSql = "DELETE FROM PizzaIngrediente WHERE PizzaId = @PizzaId";
                using (var deleteIngredientsCmd = new SqlCommand(deleteIngredientsSql, connection, transaction))
                {
                    deleteIngredientsCmd.Parameters.AddWithValue("@PizzaId", pizza.Id);
                    deleteIngredientsCmd.ExecuteNonQuery();
                }

                foreach (var ingrediente in pizza.Ingredients)
                {
                    var insertIngredientSql = "INSERT INTO PizzaIngrediente (PizzaId, IngredienteId) VALUES (@PizzaId, @IngredienteId)";
                    using (var insertIngredientCmd = new SqlCommand(insertIngredientSql, connection, transaction))
                    {
                        insertIngredientCmd.Parameters.AddWithValue("@PizzaId", pizza.Id);
                        insertIngredientCmd.Parameters.AddWithValue("@IngredienteId", ingrediente.Id);
                        insertIngredientCmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}

