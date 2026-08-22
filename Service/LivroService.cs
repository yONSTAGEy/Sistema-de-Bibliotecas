using Biblioteca.Models;
using Biblioteca.Data;

namespace Biblioteca.Service;

public class LivroService 
{
   private readonly AppDbContext _context;

    public LivroService()
    {
        _context = new AppDbContext();
    }
    public void AdicionarLivro(Livro livro)
    {
        _context.Livros.Add(livro);
        _context.SaveChanges();
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
        if(buscarLivro != null)
        {
        _context.Livros.Remove(buscarLivro);
        _context.SaveChanges();
        return true;
        }
        return false;
         
    }
}