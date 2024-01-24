namespace ContosoPizza.Models;

public class Pizza {

    public int Id{ get; set;}

    public string? Name { get; set; }

    public decimal Price {get; set;}

    public List<Ingrediente> Ingredients {get; set;} = new List<Ingrediente>();

    public bool IsGlutenFree { get; set; }
    
}