namespace ContosoPizza.Models;
using System.ComponentModel.DataAnnotations;
public class Ingrediente
{
    [Key]
    public int Id { get; set; }
    [Required]

    public string? Nombre { get; set; }
    [Required]

    public decimal Precio { get; set; }
    [Required]

    public decimal Calorias { get; set; }

}
