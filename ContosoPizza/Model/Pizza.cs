using System.ComponentModel.DataAnnotations;
namespace ContosoPizza.Models;

public class Pizza {

    [Key] //clave principal
    public int Id{ get; set;}

    [Required] 
    public string? Name { get; set; }

    [Required] 
    public decimal Price {get; set;}
    [Required] 
    public List<Ingrediente> Ingredients {get; set;} = new List<Ingrediente>();

    [Required] 
    public bool IsGlutenFree { get; set; }
}