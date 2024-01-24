using ContosoPizza.Models;
using System.Collections.Generic;
using System.Linq;

namespace ContosoPizza.Data;

public class UsuarioRepository : IUsuarioRepository
{
    private List<Usuario> Usuarios { get; set; }
    private int nextId = 3;

    public UsuarioRepository()
    {
        Usuarios = new List<Usuario>
        {
            new Usuario { Id = 0, Nombre = "Diego Gimenez Sancho", Dirección = "Calle mayor" },
            new Usuario { Id = 1, Nombre = "Gustavo Garcia", Dirección = "Calle menor" },
            new Usuario { Id = 2, Nombre = "Jack Conway", Dirección = "Calle izquierda" },
        };
    }

    public List<Usuario> GetAll() => Usuarios;

    public Usuario? Get(int id) => Usuarios.FirstOrDefault(p => p.Id == id);

    public List<Usuario> GetUsuarios() => GetAll();

    public void Add(Usuario usuario)
    {
        usuario.Id = nextId++;
        Usuarios.Add(usuario);
    }

    public void Delete(int id)
    {
        var usuario = Get(id);
        if (usuario is null)
            return;

        Usuarios.Remove(usuario);
    }

    public void Put(Usuario usuario)
    {
        var index = Usuarios.FindIndex(p => p.Id == usuario.Id);
        if (index == -1)
            return;

        Usuarios[index] = usuario;
    }
}
