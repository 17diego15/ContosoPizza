using ContosoPizza.Models;
using ContosoPizza.Business;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ContosoPizza.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly PedidoService _pedidoService;

        public PedidoController(PedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpGet]
        public ActionResult<List<Pedido>> GetAll()
        {
            return _pedidoService.GetAll();
        }

        [HttpGet("{id}")]
        public ActionResult<Pedido> Get(int id)
        {
            var pedido = _pedidoService.Get(id);

            if (pedido == null)
                return NotFound();

            return pedido;
        }

        [HttpPost]
        public IActionResult Create(Pedido pedido)
        {
            if (pedido.Pizzas != null)
            {
                pedido.Precio = pedido.Pizzas.Sum(p => p.Price);
            }

            _pedidoService.Add(pedido);

            if (pedido.Usuario != null)
            {
                return CreatedAtAction(nameof(Get), new { id = pedido.Id }, pedido);
            }
            else
            {
                return BadRequest("Usuario no válido o no encontrado.");
            }
        }
        [HttpPut("{id}/add-pizzas")]
        public IActionResult AddPizzas(int id, List<Pizza> pizzas)
        {
            _pedidoService.AddPizzas(id, pizzas);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var pedido = _pedidoService.Get(id);

            if (pedido is null)
                return NotFound();

            _pedidoService.Delete(id);

            return NoContent();
        }

        [HttpGet("pedidosPorUsuario/{usuarioId}")]
        public ActionResult<List<Pedido>> GetPedidosPorUsuario(int usuarioId)
        {
            var pedidos = _pedidoService.GetPedidosByUsuarioId(usuarioId);

            if (pedidos == null || pedidos.Count == 0)
                return NotFound();

            return pedidos;
        }
    }
}