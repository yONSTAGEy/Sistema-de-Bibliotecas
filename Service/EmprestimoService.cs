using Biblioteca.Data;
using Biblioteca.Models;

namespace Biblioteca.Service;

public class EmprestimoService
{
    private readonly AppDbContext _context;

    public EmprestimoService()
    {
        _context = new AppDbContext();
    }
    public Livro? BuscarLivro(int id)
    {
        return _context.Livros.Find(id);
    }

     public Usuario? BuscarUsuario(int id)
    {
        return _context.Usuario.Find(id);
    }

    public bool Emprestimo(Emprestimo emprestimo)
    {
        var BuscarLivroID = BuscarLivro(emprestimo.LivroID);
        var BuscarUsuarioID = BuscarUsuario(emprestimo.UsuarioID);
        Console.WriteLine(BuscarLivroID == null ? "Livro não encontrado" : "Livro encontrado");
        Console.WriteLine(BuscarUsuarioID == null ? "Usuario não encontrado" : "Usuario encontrado");
        if(BuscarLivroID == null || BuscarUsuarioID == null)
        {
            return false;
        }
        else
        {
            emprestimo.DataDevolucaoReal = null;
            emprestimo.DataEmprestimo = DateTime.Today;
            emprestimo.DataDevolucaoPrevista = emprestimo.DataEmprestimo.AddDays(7); 
            _context.Emprestimo.Add(emprestimo);
            _context.SaveChanges();
            return true;
        }
   
    }

    public List<Emprestimo> ListarEmprestimo()
    {
        return _context.Emprestimo.ToList();
    }

    public void DevolverEmprestimo(Emprestimo emprestimo)
    {
        emprestimo.DataDevolucaoReal = DateTime.Now;
        _context.SaveChanges();
    }
   
}