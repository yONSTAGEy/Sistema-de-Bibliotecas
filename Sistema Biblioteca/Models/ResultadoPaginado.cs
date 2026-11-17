
namespace Biblioteca.Models; 

public class ResultadoPaginado
{
    public List<Livro> Livros {get; set;} = new();
    public int TotalRegistros {get; set;}
    public int PaginaAtual {get; set;}
    public int TamanhoPagina {get; set;}
    public int TotalPaginas {get; set;}
}