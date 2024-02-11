using ContosoPizza.Models;
using System.Data.SqlTypes;
using System.Text.Json;
using System.Data;
using System.Data.SqlClient;

namespace ContosoPizza.Data;

public class UsuarioSqlRepository : IUsuarioRepository
{
    private readonly string _connectionString;

    public UsuarioSqlRepository(string connectionString)
    {
        _connectionString = connectionString;
    }


    public List<Usuario> GetAll()
    {
        var usuarios = new List<Usuario>();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            var sqlString = "SELECT UsuarioId, Nombre, Direccion FROM Usuario"; 
            var command = new SqlCommand(sqlString, connection);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var usuario = new Usuario
                    {
                        Id = Convert.ToInt32(reader["UsuarioId"]),
                        Nombre = reader["Nombre"].ToString(),
                        Dirección = reader["Direccion"].ToString(),
                    };
                    usuarios.Add(usuario);
                }
            }
        }

        return usuarios;
    }

    public Usuario? Get(int id)
    {
        var usuario = new Usuario();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            var sqlString = "SELECT UsuarioId, Nombre, Direccion FROM Usuario WHERE UsuarioId=" + id; 
            var command = new SqlCommand(sqlString, connection);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    usuario = new Usuario
                    {
                        Id = Convert.ToInt32(reader["UsuarioId"]),
                        Nombre = reader["Nombre"].ToString(),
                        Dirección = reader["Direccion"].ToString()
                    };
                }
            }

        }

        return usuario;
    }

    public List<Usuario> GetUsuarios() => GetAll();

    public void Add(Usuario usuario)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            var sqlString = "INSERT INTO Usuario (Nombre, Direccion) VALUES (@Nombre, @Direccion)";
            var command = new SqlCommand(sqlString, connection);

            command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
            command.Parameters.AddWithValue("@Direccion", usuario.Dirección);

            command.ExecuteNonQuery();
        }
    }

    public void Delete(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            var sqlString = "DELETE FROM Usuario WHERE UsuarioId = @UsuarioId";
            var command = new SqlCommand(sqlString, connection);

            command.Parameters.AddWithValue("@UsuarioId", id);

            command.ExecuteNonQuery();
        }
    }


    public void Put(Usuario usuario)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            var sqlString = @"
            UPDATE Usuario 
            SET Nombre = @Nombre, Direccion = @Direccion
            WHERE UsuarioId = @UsuarioId";

            using (var command = new SqlCommand(sqlString, connection))
            {
                command.Parameters.AddWithValue("@UsuarioId", usuario.Id);
                command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                command.Parameters.AddWithValue("@Direccion", usuario.Dirección);

                int rowsAffected = command.ExecuteNonQuery();
            }
        }
    }
}

