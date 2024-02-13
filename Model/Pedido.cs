namespace ContosoPizza.Models;
using System.ComponentModel.DataAnnotations;

public class Pedido
{
    [Key]
    public int Id { get; set; }

    [Required]
    public List<Pizza> Pizzas { get; set; } = new List<Pizza>();

    [Required]
    public decimal Precio { get; set; }

    [Required]
    public Usuario? Usuario { get; set; }

    [Required]
    public DateTime FechaPedido { get; set; }

}