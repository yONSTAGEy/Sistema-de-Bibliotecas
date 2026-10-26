namespace Biblioteca.Models;

public class Usuario
{
    public int ID {get; set; }
    public string Nome {get; set;} = string.Empty;
    public string Email{get; set;} = string.Empty;
    public string CPF {get; set;} = string.Empty;
}