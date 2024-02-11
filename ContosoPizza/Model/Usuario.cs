namespace ContosoPizza.Models;
using System.ComponentModel.DataAnnotations;

public class Usuario {
    [Key]
    public int Id{ get; set;}

    [Required] 
    public string? Nombre { get; set; }
    
    [Required] 
    public string? Dirección {get; set;}
    
}