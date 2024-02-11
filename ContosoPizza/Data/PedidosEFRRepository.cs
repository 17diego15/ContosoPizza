using Microsoft.EntityFrameworkCore;
using ContosoPizza.Models;
using System.Collections.Generic;
using System.Linq;
using ContosoPizza.Data;

namespace ContosoPizza.Data
{
    public class PedidosEFRRepository : IPedidoRepository
    {
        private readonly ContosoPizzaContext _context;

        public PedidosEFRRepository(ContosoPizzaContext context)
        {
            _context = context;
        }

        public Pedido Get(int Id)
        {
            return _context.Pedidos.FirstOrDefault(pedido => pedido.Id == Id);
        }

        public void Add(Pedido pedido)
        {
            _context.Pedidos.Add(pedido);
            SaveChanges();
        }

        public void Delete(int id)
        {
            var pedido = _context.Pedidos.FirstOrDefault(pedido => pedido.Id == id);
            if (pedido != null)
            {
                _context.Pedidos.Remove(pedido);
            }
            SaveChanges();
        }

        public List<Pedido> GetAll()
        {
            return _context.Pedidos.ToList();
            SaveChanges();
        }

        public void Put(Pedido pedido)
        {
            _context.Pedidos.Update(pedido);
            SaveChanges();
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

//pedir luego al chat
        public List<Pedido> GetPedidosByUsuarioId(int usuarioId)
        {
            throw new NotImplementedException();
        }
    }
}