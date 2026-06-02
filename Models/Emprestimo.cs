using System.Security.Cryptography;

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

    public double CalcularMulta()
    {
        
    }

    public bool EstaAtrasado()
    {
        
        return true;
    }
}