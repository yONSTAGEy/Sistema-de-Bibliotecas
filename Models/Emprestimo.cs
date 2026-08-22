using System.Security.Cryptography;
namespace Biblioteca.Models;
public class Emprestimo
{
    public int ID {get; set;}
    public int LivroID {get; set;}
    public int UsuarioID{get; set;}
    public Livro Livros{get; set;}
    public Usuario Usuario{get; set;}
    public DateTime DataEmprestimo{get; set;}
    public DateTime DataDevolucaoPrevista  {get; set;}
    public DateTime? DataDevolucaoReal{get; set;}

    public decimal CalcularMulta()
    {
        TimeSpan atraso;
        if(DataDevolucaoReal != null)
        {
            atraso = DataDevolucaoReal.Value - DataDevolucaoPrevista;
        }
        else
        {
            atraso = DateTime.Now - DataDevolucaoPrevista;
        }
        if (atraso.Days <= 0)
        {
            return 0m;
        }
        return (decimal)atraso.Days * 2m;
    }

    public bool EstaAtrasado()
    {
        return DateTime.Now > DataDevolucaoPrevista && DataDevolucaoReal == null;
        
    }
}