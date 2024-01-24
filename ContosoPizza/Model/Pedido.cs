namespace ContosoPizza.Models;

public class Pedido
{
    public int Id { get; set; }
    public List<Pizza> Pizzas { get; set; } = new List<Pizza>();
    public decimal Precio { get; set; }
    public Usuario? Usuario { get; set; } 

    public DateTime FechaPedido {get; set;}
    
}