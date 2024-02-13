using ContosoPizza.Models;
using System.Collections.Generic;
using System.Linq;

namespace ContosoPizza.Data;

public class PedidoRepository : IPedidoRepository
{
    private List<Pedido> Pedidos { get; set; }
    private int nextId = 1;
    private IUsuarioRepository _usuarioRepository;

    public PedidoRepository(IUsuarioRepository usuarioRepository)
    {
        Pedidos = new List<Pedido>();
        _usuarioRepository = usuarioRepository;
    }

    public List<Pedido> GetAll() => Pedidos;

    public Pedido? Get(int id) => Pedidos.FirstOrDefault(p => p.Id == id);

    public List<Pedido> GetPedidosByUsuarioId(int usuarioId)
    {
        return Pedidos.Where(p => p.Usuario?.Id == usuarioId).ToList();
    }

    public void Add(Pedido pedido)
    {
        pedido.Id = nextId++;
        Pedidos.Add(pedido);
    }

    public void Delete(int id)
    {
        var pedido = Get(id);
        if (pedido is null)
            return;

        Pedidos.Remove(pedido);
    }

    public void Put(Pedido pedido)
    {
        var index = Pedidos.FindIndex(p => p.Id == pedido.Id);
        if (index == -1)
            return;

        Pedidos[index] = pedido;
    }
}