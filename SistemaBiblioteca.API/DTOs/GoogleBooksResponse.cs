
namespace SistemaBiblioteca.API.DTOs;

public class GoogleBooksResponse
{
    public int TotalItems {get; set;}
    public List<GoogleBooksItem> Items {get; set; }= new();
}

