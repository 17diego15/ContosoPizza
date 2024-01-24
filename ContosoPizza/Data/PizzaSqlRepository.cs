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
        throw new NotImplementedException();
    }

    public Pizza? Get(int id)
    {
        var pizza = new Pizza();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            var sqlString = "SELECT PizzaId, Nombre, Precio, IsGlutenFree FROM Pizza WHERE PizzaId=" + id;
            var command = new SqlCommand(sqlString, connection);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    pizza = new Pizza
                    {
                        Id = Convert.ToInt32(reader["PizzaId"]),
                        Name = reader["Nombre"].ToString(),
                        Price = Convert.ToDecimal(reader["Precio"]),
                        IsGlutenFree = Convert.ToBoolean(reader["IsGlutenFree"])    
                        //Balance = (decimal)reader[2]
                    };
                }
            }

        }

        return pizza;
    }

    public void Add(Pizza pizza)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public void Put(Pizza pizza)
    {
        throw new NotImplementedException();
    }
}
