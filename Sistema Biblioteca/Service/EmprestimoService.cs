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

    public string Emprestimo(Emprestimo emprestimo)
    {
        var BuscarLivroID = BuscarLivro(emprestimo.LivroID);
        var BuscarUsuarioID = BuscarUsuario(emprestimo.UsuarioID);
        if (BuscarLivroID == null)
        {
            return "Livro não encontrado";
        }
        else if (BuscarUsuarioID == null)
        {
            return "Usuário não encontrado";
        }
        else if (_context.Emprestimo.Any(e => e.LivroID == emprestimo.LivroID && e.DataDevolucaoReal == null))
        {
            return "Livro indisponível";
        }
        else
        {
            emprestimo.DataDevolucaoReal = null;
            emprestimo.DataEmprestimo = DateTime.Today;
            emprestimo.DataDevolucaoPrevista = emprestimo.DataEmprestimo.AddDays(7);
            _context.Emprestimo.Add(emprestimo);
            _context.SaveChanges();
            return "Empréstimo feito";
        }

    }

    public List<Emprestimo> ListarEmprestimo()
    {
        return _context.Emprestimo.ToList();
    }

    public string DevolverEmprestimo(int emprestimoID, int usuarioID)
    {
        var emprestimo = _context.Emprestimo.Find(emprestimoID);
        if (emprestimo == null)
        {
            return "Empréstimo não encontrado";
        }
        if (emprestimo.UsuarioID != usuarioID)
        {
            return "Empréstimo não pertence ao usuário";
        }
        if (emprestimo.DataDevolucaoReal != null)
        {
            return "Livro já devolvido";
        }

        emprestimo.DataDevolucaoReal = DateTime.Now;
        _context.SaveChanges();
        return "Devolução realizada";
    }

    public List<Emprestimo> ListarEmprestimosUsuario(int usuarioID)
    {
        return _context.Emprestimo.Where(e => e.UsuarioID == usuarioID).ToList();
    }

    public List<Emprestimo> ListarEmprestimosAtrasadoAdmin()
    {
        var Emprestimos = _context.Emprestimo.ToList();

        return Emprestimos.Where(e => e.EstaAtrasado()).ToList();
    }


}