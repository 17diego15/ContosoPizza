using ContosoPizza.Models;
using ContosoPizza.Data;
using System.Collections.Generic;

namespace ContosoPizza.Business
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public List<Usuario> GetAll()
        {
            return _usuarioRepository.GetAll();
        }

        public Usuario? Get(int id)
        {
            return _usuarioRepository.Get(id);
        }

        public void Add(Usuario usuario)
        {
            _usuarioRepository.Add(usuario);
        }

        public void Delete(int id)
        {
            _usuarioRepository.Delete(id);
        }

        public void Put(Usuario usuario)
        {
            _usuarioRepository.Put(usuario);
        }

    }
}
