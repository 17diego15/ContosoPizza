using ContosoPizza.Models;
using ContosoPizza.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ContosoPizza.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PizzaController : ControllerBase
    {
        private readonly PizzaService _pizzaService;

        public PizzaController(PizzaService pizzaService)
        {
            _pizzaService = pizzaService;
        }

        [HttpGet]
        public ActionResult<List<Pizza>> GetAll()
        {
            return _pizzaService.GetAll();
        }

        [HttpGet("{id}")]
        public ActionResult<Pizza> Get(int id)
        {
            var pizza = _pizzaService.Get(id);

            if (pizza == null)
                return NotFound();

            return pizza;
        }

        [HttpPost]
        public IActionResult Create(Pizza pizza)
        {
            _pizzaService.Add(pizza);

            return CreatedAtAction(nameof(Get), new { id = pizza.Id }, pizza);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Pizza pizza)
        {
            if (id != pizza.Id)
                return BadRequest();

            var existingPizza = _pizzaService.Get(id);
            if (existingPizza is null)
                return NotFound();

            _pizzaService.Put(pizza);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var pizza = _pizzaService.Get(id);

            if (pizza is null)
                return NotFound();

            _pizzaService.Delete(id);

            return NoContent();
        }
    }
}
