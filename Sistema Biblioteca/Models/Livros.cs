namespace Biblioteca.Models;

public class Livro
{
    public int ID {get; set;}
    public int Ano {get; set;}
    public string Titulo {get; set;} = string.Empty;
    public string Autor {get; set;} = string.Empty;
    
}