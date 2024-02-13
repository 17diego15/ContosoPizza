using Microsoft.EntityFrameworkCore;
using ContosoPizza.Models;
using System.Collections.Generic;
using System.Linq;
using ContosoPizza.Data;

namespace ContosoPizza.Data
{
    public class UsuariosEFRReposirory : IUsuarioRepository
    {
        private readonly ContosoPizzaContext _context;

        public UsuariosEFRReposirory(ContosoPizzaContext context)
        {
            _context = context;
        }

        public List<Usuario> GetAll()
        {

            return _context.Usuarios.ToList();
        }

        public Usuario Get(int Id)
        {

            return _context.Usuarios.FirstOrDefault(usuario => usuario.Id == Id);

        }

        public void Add(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(usuario => usuario.Id == id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                _context.SaveChanges();
            }
        }

        public void Put(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            _context.SaveChanges();
        }

        public List<Usuario> GetUsuarios() => GetAll();

    }
}
