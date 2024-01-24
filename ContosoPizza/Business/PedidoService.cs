using ContosoPizza.Models;
using ContosoPizza.Data;

namespace ContosoPizza.Business
{
    public class PedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        private readonly IPizzaRepository _pizzaRepository;
        private readonly IIngredientesRepository _ingredienteRepository;


        public PedidoService(IPedidoRepository pedidoRepository, IUsuarioRepository usuarioRepository, IPizzaRepository pizzaRepository, IIngredientesRepository ingredientesRepository)
        {
            _pedidoRepository = pedidoRepository;
            _usuarioRepository = usuarioRepository;
            _pizzaRepository = pizzaRepository;
            _ingredienteRepository = ingredientesRepository;
        }

        public List<Pedido> GetAll()
        {
            return _pedidoRepository.GetAll();
        }

        public Pedido? GetPedido(int id)
        {
            return _pedidoRepository.Get(id);
        }

        public List<Pedido> GetPedidosByUsuarioId(int usuarioId)
        {
            return _pedidoRepository.GetAll().Where(p => p.Usuario?.Id == usuarioId).ToList();
        }

        public void Add(Pedido pedido)
        {
            var usuario = _usuarioRepository.Get(pedido.Usuario?.Id ?? 0);
            if (usuario == null)
            {
                throw new ArgumentException("Usuario no válido o no encontrado.");
            }
            pedido.Usuario = usuario;

            var pizzasParaAgregar = new List<Pizza>();

            foreach (var pizzaPedido in pedido.Pizzas)
            {
                var pizzaExistente = _pizzaRepository.Get(pizzaPedido.Id);
                if (pizzaExistente != null)
                {
                    var ingredientesCompletos = pizzaExistente.Ingredients.Select(ingrediente =>
                        _ingredienteRepository.Get(ingrediente.Id)).ToList();

                    pizzaExistente.Ingredients = ingredientesCompletos;

                    CalculatePrice(pizzaExistente);

                    pizzasParaAgregar.Add(pizzaExistente);
                }
            }

            pedido.Pizzas = pizzasParaAgregar;

            pedido.Precio = CalculateTotalPrice(pedido.Pizzas);

            _pedidoRepository.Add(pedido);
        }

        public void Delete(int id)
        {
            _pedidoRepository.Delete(id);
        }

        public Pedido? Get(int id)
        {
            return _pedidoRepository.Get(id);
        }

        public void AddPizzas(int pedidoId, List<Pizza> pizzas)
        {
            var pedido = _pedidoRepository.Get(pedidoId);

            foreach (var pizzaPedido in pizzas)
            {
                var pizzaExistente = _pizzaRepository.Get(pizzaPedido.Id);
                if (pizzaExistente != null)
                {
                    var ingredientesCompletos = pizzaExistente.Ingredients.Select(ingrediente =>
                        _ingredienteRepository.Get(ingrediente.Id)).ToList();

                    pizzaExistente.Ingredients = ingredientesCompletos;

                    CalculatePrice(pizzaExistente);

                    pedido.Pizzas.Add(pizzaExistente);
                }
            }

            pedido.Precio = CalculateTotalPrice(pedido.Pizzas);

            _pedidoRepository.Put(pedido);
        }

        private decimal CalculateTotalPrice(List<Pizza> pizzas)
        {
            return pizzas.Sum(p => p.Price);
        }

        private void CalculatePrice(Pizza pizza)
        {
            if (pizza == null || pizza.Ingredients == null || !pizza.Ingredients.Any())
            {
                pizza.Price = 0;
                return;
            }

            decimal precioTotalIngredientes = pizza.Ingredients
                .Select(ing => _ingredienteRepository.Get(ing.Id))
                .Where(ing => ing != null)
                .Sum(ing => ing.Precio);


            decimal iva = 1.21M;
            decimal precioConIva = precioTotalIngredientes * 3 * iva;
            pizza.Price = Math.Round(precioConIva, 2, MidpointRounding.AwayFromZero);
        }
    }
}
