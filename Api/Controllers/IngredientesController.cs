using ContosoPizza.Models;
using ContosoPizza.Business;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ContosoPizza.Services;

namespace ContosoPizza.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IngredientesController : ControllerBase
    {
        private readonly IngredientesService _ingredientesService;

        public IngredientesController(IngredientesService ingredientesService)
        {
            _ingredientesService = ingredientesService;
        }

        [HttpGet]
        public ActionResult<List<Ingrediente>> GetAll()
        {
            return _ingredientesService.GetAll();
        }

        [HttpGet("{id}")]
        public ActionResult<Ingrediente> Get(int id)
        {
            var ingrediente = _ingredientesService.Get(id);

            if (ingrediente == null)
                return NotFound();

            return ingrediente;
        }

        [HttpPost]
        public IActionResult Create(Ingrediente ingrediente)
        {
            _ingredientesService.Add(ingrediente);
            return CreatedAtAction(nameof(Get), new { id = ingrediente.Id }, ingrediente);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Ingrediente ingrediente)
        {
            if (id != ingrediente.Id)
                return BadRequest();

            var existingIngrediente = _ingredientesService.Get(id);
            if (existingIngrediente is null)
                return NotFound();

            _ingredientesService.Put(ingrediente);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var ingrediente = _ingredientesService.Get(id);

            if (ingrediente is null)
                return NotFound();

            _ingredientesService.Delete(id);

            return NoContent();
        }
    }
}