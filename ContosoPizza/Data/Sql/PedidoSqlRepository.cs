using ContosoPizza.Models;
using System.Data.SqlTypes;
using System.Text.Json;
using System.Data;
using System.Data.SqlClient;

namespace ContosoPizza.Data;

public class PedidoSqlRepository : IPedidoRepository
{

    private readonly string _connectionString;

    public PedidoSqlRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<Pedido> GetAll()
    {
        var pedidos = new List<Pedido>();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            var pedidoSqlString = @"
            SELECT p.PedidoId, p.Precio, p.FechaPedido, p.UsuarioId
            FROM Pedidos p";
            using (var command = new SqlCommand(pedidoSqlString, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var pedido = new Pedido
                    {
                        Id = Convert.ToInt32(reader["PedidoId"]),
                        Precio = Convert.ToDecimal(reader["Precio"]),
                        FechaPedido = Convert.ToDateTime(reader["FechaPedido"]),
                        Usuario = new Usuario { Id = Convert.ToInt32(reader["UsuarioId"]) },
                        Pizzas = new List<Pizza>()
                    };
                    pedidos.Add(pedido);
                }
            }
        }

        return pedidos;
    }

    public Pedido? Get(int id)
    {
        Pedido pedido = null;

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            var pedidoSqlString = "SELECT PedidoId, Precio, FechaPedido, UsuarioId FROM Pedidos WHERE PedidoId = @PedidoId";
            using (var pedidoCommand = new SqlCommand(pedidoSqlString, connection))
            {
                pedidoCommand.Parameters.AddWithValue("@PedidoId", id);

                using (var reader = pedidoCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        pedido = new Pedido
                        {
                            Id = Convert.ToInt32(reader["PedidoId"]),
                            Precio = Convert.ToDecimal(reader["Precio"]),
                            FechaPedido = Convert.ToDateTime(reader["FechaPedido"]),
                            Usuario = new Usuario { Id = Convert.ToInt32(reader["UsuarioId"]) },
                            Pizzas = new List<Pizza>()
                        };
                    }
                }
            }

            if (pedido == null) return null;

            var pizzasSqlString = @"
        SELECT p.PizzaId, p.Nombre, p.Precio, p.IsGluten
        FROM Pizza p
        INNER JOIN PedidoPizzas pp ON p.PizzaId = pp.PizzaId
        WHERE pp.PedidoId = @PedidoId";
            using (var pizzasCommand = new SqlCommand(pizzasSqlString, connection))
            {
                pizzasCommand.Parameters.AddWithValue("@PedidoId", id);

                using (var reader = pizzasCommand.ExecuteReader())
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
                        pedido.Pizzas.Add(pizza);
                    }
                }
            }
        }

        return pedido;
    }
    public void Add(Pedido pedido)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                var pedidoSqlString = @"
            INSERT INTO Pedidos (Precio, UsuarioId, FechaPedido)
            VALUES (@Precio, @UsuarioId, @FechaPedido);
            SELECT SCOPE_IDENTITY();";

                using (var pedidoCommand = new SqlCommand(pedidoSqlString, connection, transaction))
                {
                    pedidoCommand.Parameters.AddWithValue("@Precio", pedido.Precio);
                    pedidoCommand.Parameters.AddWithValue("@UsuarioId", pedido.Usuario.Id);
                    pedidoCommand.Parameters.AddWithValue("@FechaPedido", pedido.FechaPedido);

                    var pedidoId = Convert.ToInt32(pedidoCommand.ExecuteScalar());

                    foreach (var pizza in pedido.Pizzas)
                    {
                        var pedidoPizzasSqlString = @"
                    INSERT INTO PedidoPizzas (PedidoId, PizzaId)
                    VALUES (@PedidoId, @PizzaId);";

                        using (var pedidoPizzasCommand = new SqlCommand(pedidoPizzasSqlString, connection, transaction))
                        {
                            pedidoPizzasCommand.Parameters.AddWithValue("@PedidoId", pedidoId);
                            pedidoPizzasCommand.Parameters.AddWithValue("@PizzaId", pizza.Id);

                            pedidoPizzasCommand.ExecuteNonQuery();
                        }
                    }

                }
            }
        }
    }

    public void Delete(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {

                var deletePedidoPizzasSql = "DELETE FROM PedidoPizzas WHERE PedidoId = @PedidoId";
                using (var deletePedidoPizzasCmd = new SqlCommand(deletePedidoPizzasSql, connection, transaction))
                {
                    deletePedidoPizzasCmd.Parameters.AddWithValue("@PedidoId", id);
                    deletePedidoPizzasCmd.ExecuteNonQuery();
                }

                var deletePedidoSql = "DELETE FROM Pedidos WHERE PedidoId = @PedidoId";
                using (var deletePedidoCmd = new SqlCommand(deletePedidoSql, connection, transaction))
                {
                    deletePedidoCmd.Parameters.AddWithValue("@PedidoId", id);
                    deletePedidoCmd.ExecuteNonQuery();
                }
            }
        }
    }

    public List<Pedido> GetPedidosByUsuarioId(int usuarioId)
    {
        var pedidos = new List<Pedido>();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            var pedidoSqlString = "SELECT PedidoId, Precio, FechaPedido, UsuarioId FROM Pedidos WHERE UsuarioId = @UsuarioId";
            using (var command = new SqlCommand(pedidoSqlString, connection))
            {
                command.Parameters.AddWithValue("@UsuarioId", usuarioId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var pedido = new Pedido
                        {
                            Id = Convert.ToInt32(reader["PedidoId"]),
                            Precio = Convert.ToDecimal(reader["Precio"]),
                            FechaPedido = Convert.ToDateTime(reader["FechaPedido"]),
                            Usuario = new Usuario { Id = usuarioId },
                            Pizzas = new List<Pizza>() 
                        };
                        pedidos.Add(pedido);
                    }
                }
            }
        }

        return pedidos;
    }

    public void Put(Pedido pedido)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {

                var updatePedidoSql = @"
                UPDATE Pedidos
                SET Precio = @Precio, UsuarioId = @UsuarioId, FechaPedido = @FechaPedido
                WHERE PedidoId = @PedidoId";

                using (var updatePedidoCmd = new SqlCommand(updatePedidoSql, connection, transaction))
                {
                    updatePedidoCmd.Parameters.AddWithValue("@PedidoId", pedido.Id);
                    updatePedidoCmd.Parameters.AddWithValue("@Precio", pedido.Precio);
                    updatePedidoCmd.Parameters.AddWithValue("@UsuarioId", pedido.Usuario.Id);
                    updatePedidoCmd.Parameters.AddWithValue("@FechaPedido", pedido.FechaPedido);

                    updatePedidoCmd.ExecuteNonQuery();
                }

                var deletePedidoPizzasSql = "DELETE FROM PedidoPizzas WHERE PedidoId = @PedidoId";
                using (var deletePedidoPizzasCmd = new SqlCommand(deletePedidoPizzasSql, connection, transaction))
                {
                    deletePedidoPizzasCmd.Parameters.AddWithValue("@PedidoId", pedido.Id);
                    deletePedidoPizzasCmd.ExecuteNonQuery();
                }

                foreach (var pizza in pedido.Pizzas)
                {
                    var insertPedidoPizzasSql = @"
                    INSERT INTO PedidoPizzas (PedidoId, PizzaId)
                    VALUES (@PedidoId, @PizzaId)";
                    using (var insertPedidoPizzasCmd = new SqlCommand(insertPedidoPizzasSql, connection, transaction))
                    {
                        insertPedidoPizzasCmd.Parameters.AddWithValue("@PedidoId", pedido.Id);
                        insertPedidoPizzasCmd.Parameters.AddWithValue("@PizzaId", pizza.Id);

                        insertPedidoPizzasCmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}