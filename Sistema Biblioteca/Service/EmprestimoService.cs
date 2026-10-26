using Biblioteca.Data;
using Biblioteca.Models;

namespace Biblioteca.Service;

public class EmprestimoService
{
    private readonly AppDbContext _context;

    public EmprestimoService(AppDbContext context)
    {
        _context = context;
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
        if(BuscarLivroID == null || BuscarUsuarioID == null)
        {
            return false;
        }
        else if(_context.Emprestimo.Any(e => e.LivroID == emprestimo.LivroID && e.DataDevolucaoReal == null))
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