using ContosoPizza.Models;
using System.Data.SqlTypes;
using System.Text.Json;
using System.Data;
using System.Data.SqlClient;


namespace ContosoPizza.Data;

public class IngredientesSqlRepository : IIngredientesRepository
{
    private readonly string _connectionString;

    public IngredientesSqlRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<Ingrediente> GetAll()
    {
        var ingredientes = new List<Ingrediente>();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            var sqlString = "SELECT IngredienteId, Nombre, Precio, Calorias FROM Ingrediente"; 
            var command = new SqlCommand(sqlString, connection);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var ingrediente = new Ingrediente
                    {
                        Id = Convert.ToInt32(reader["IngredienteId"]),
                        Nombre = reader["Nombre"].ToString(),
                        Precio = Convert.ToDecimal(reader["Precio"]),
                        Calorias = Convert.ToDecimal(reader["Calorias"])
                    };
                    ingredientes.Add(ingrediente);
                }
            }
        }

        return ingredientes;
    }

    public Ingrediente? Get(int id)
    {
        var ingrediente = new Ingrediente();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            var sqlString = "SELECT IngredienteId, Nombre, Precio, Calorias FROM Ingrediente WHERE IngredienteId=" + id;
            var command = new SqlCommand(sqlString, connection);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    ingrediente = new Ingrediente
                    {
                        Id = Convert.ToInt32(reader["PizzaId"]),
                        Nombre = reader["Nombre"].ToString(),
                        Precio = Convert.ToDecimal(reader["Precio"]),
                        Calorias = Convert.ToDecimal(reader["Calorias"])
                    };
                    reader.Close();
                }
            }

        }

        return ingrediente;
    }

    public List<Ingrediente> GetIngredientes() => GetAll();

    public void Add(Ingrediente ingrediente)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            var sqlString = "INSERT INTO Ingrediente (Nombre, Precio, Calorias) VALUES (@Nombre, @Precio, @Calorias)"; 
            var command = new SqlCommand(sqlString, connection);

            command.Parameters.AddWithValue("@Nombre", ingrediente.Nombre);
            command.Parameters.AddWithValue("@Precio", ingrediente.Precio);
            command.Parameters.AddWithValue("@Calorias", ingrediente.Calorias);

            command.ExecuteNonQuery();
        }
    }

    public void Delete(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            var sqlString = "DELETE FROM Ingrediente WHERE IngredienteId = @IngredienteId"; 
            var command = new SqlCommand(sqlString, connection);

            command.Parameters.AddWithValue("@IngredienteId", id);

            command.ExecuteNonQuery();
        }
    }


    public void Put(Ingrediente ingrediente)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            var sqlString = @"
            UPDATE Ingrediente 
            SET Nombre = @Nombre, Precio = @Precio, Calorias = @Calorias 
            WHERE IngredienteId = @IngredienteId";

            using (var command = new SqlCommand(sqlString, connection))
            {
                command.Parameters.AddWithValue("@IngredienteId", ingrediente.Id);
                command.Parameters.AddWithValue("@Nombre", ingrediente.Nombre);
                command.Parameters.AddWithValue("@Precio", ingrediente.Precio);
                command.Parameters.AddWithValue("@Calorias", ingrediente.Calorias);

                int rowsAffected = command.ExecuteNonQuery();
            }
        }
    }


}









