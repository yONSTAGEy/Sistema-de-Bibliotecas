namespace Biblioteca.Models;

public class Livro
{
    public int ID {get; set;}
    public int Ano {get; set;}
    public string Titulo {get; set;} = string.Empty;
    public ICollection<Autor> Autores {get; set; } = new List<Autor> ();
    
    
}