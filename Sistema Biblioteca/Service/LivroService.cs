using Biblioteca.Models;
using Biblioteca.Data;

namespace Biblioteca.Service;

public class LivroService
{
    private readonly AppDbContext _context;

    public LivroService(AppDbContext context)
    {
        _context = context;
    }

    public bool AdicionarLivro(Livro livro)
    {
        if (string.IsNullOrWhiteSpace(livro.Titulo) || string.IsNullOrWhiteSpace(livro.Autor) || livro.Ano < 1000 || livro.Ano > DateTime.Now.Year)
        {
            return false;
        }
        else
        {
            _context.Livros.Add(livro);
            _context.SaveChanges();
            return true;
        }

    }
    public List<Livro> ListarLivros()
    {
        return _context.Livros.ToList();
    }
    public List<Livro> BuscarPorTitulo(string titulo)
    {
        return _context.Livros.Where(l => l.Titulo.ToLower().Contains(titulo.ToLower())).ToList();
    }
    public Livro? BuscarPorId(int id)
    {
        return _context.Livros.Find(id);
    }
    public bool Update(int id, string titulo, string autor, int ano)
    {
        if (string.IsNullOrWhiteSpace(titulo) ||
        string.IsNullOrWhiteSpace(autor) ||
        ano < 1000 ||
        ano > DateTime.Now.Year)
        {
            return false;
        }
        var LivroExistente = BuscarPorId(id);
        if (LivroExistente != null)
        {
            LivroExistente.Titulo = titulo;
            LivroExistente.Autor = autor;
            LivroExistente.Ano = ano;

            _context.SaveChanges();
            return true;
        }
        return false;
    }
    public bool RemoverLivro(int id)
    {
        var buscarLivro = BuscarPorId(id);
        if (buscarLivro == null)
        {
            return false;
        }

        if (_context.Emprestimo.Any(e => e.LivroID == id))
        {
            return false;
        }

        _context.Livros.Remove(buscarLivro);
        _context.SaveChanges();

        return true;
    }
}