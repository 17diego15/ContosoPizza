using ContosoPizza.Models;

namespace ContosoPizza.Data;

    public interface IPedidoRepository
    {
        List<Pedido> GetAll();
        Pedido? Get(int id);
        List<Pedido> GetPedidosByUsuarioId(int usuarioId);
        void Add(Pedido pedido);
        void Delete(int id);
        void Put(Pedido pedido);
    }
