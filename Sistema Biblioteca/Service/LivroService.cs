using Biblioteca.Models;
using Biblioteca.Data;
using Microsoft.EntityFrameworkCore;

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
        
        if (string.IsNullOrWhiteSpace(livro.Titulo) || livro.Autores.Count == 0 || livro.Autores.Any(a => string.IsNullOrWhiteSpace(a.Nome)) || livro.Ano < 1000 || livro.Ano > DateTime.Now.Year)
        {
            return false;
        }
        else
        {
            List<Autor> AutoresAtualizados = new List<Autor>();
            foreach (var autor in livro.Autores)
            {
                var autorExistente = AutoresAtualizados.FirstOrDefault(
                a => a.Nome.ToLower() == autor.Nome.ToLower()
                );

                if (autorExistente == null)
                {
                    autorExistente = _context.Autores.FirstOrDefault(
                    a => a.Nome.ToLower() == autor.Nome.ToLower()
                );
                }

                if (autorExistente != null)
                {
                    AutoresAtualizados.Add(autorExistente);
                }
                else
                {
                    AutoresAtualizados.Add(autor);
                }   
            }

            livro.Autores = AutoresAtualizados;
            _context.Livros.Add(livro);
            _context.SaveChanges();
            return true;
        }

    }
    public ResultadoPaginado ListarLivros(string? titulo = null, string? autor = null, bool? Disponivel = null, int page = 1, int pageSize = 8)
    {
        var consulta = _context.Livros
        .Include(l => l.Autores)
        .AsQueryable();

        if (!string.IsNullOrWhiteSpace(titulo))
        {
            consulta = consulta.Where(e => e.Titulo.ToLower().Contains(titulo.ToLower()));
        }
        if (!string.IsNullOrWhiteSpace(autor))
        {
            consulta = consulta.Where(e => e.Autores.Any(a => a.Nome.ToLower().Contains(autor.ToLower())));
        }
        if (Disponivel.HasValue)
        {
            consulta = consulta.Where(e => !_context.Emprestimo.Any(l => l.LivroID == e.ID && l.DataDevolucaoReal == null) == Disponivel.Value);
        }
        if (page < 1)
        {
            page = 1;
        }
        if (pageSize < 1)
        {
            pageSize = 8;
        }
        if (pageSize > 100)
        {
            pageSize = 100;
        }

        consulta = consulta.OrderBy(e => e.ID);

        var TotalLivros = consulta.Count();

        var TotalPaginas = Convert.ToInt32(Math.Ceiling((double)TotalLivros / pageSize));

        consulta = consulta.Skip((page - 1) * pageSize).Take(pageSize);

        return new ResultadoPaginado { Livros = consulta.ToList(), PaginaAtual = page, TamanhoPagina = pageSize, TotalPaginas = TotalPaginas, TotalRegistros = TotalLivros };
    }
    public List<Livro> BuscarPorTitulo(string titulo)
    {
        return _context.Livros.Where(l => l.Titulo.ToLower().Contains(titulo.ToLower())).ToList();
    }
    public Livro? BuscarPorId(int id)
    {
        return _context.Livros
        .Include(l => l.Autores)
        .FirstOrDefault(l => l.ID == id);
    }

    public bool Update(int id, string titulo, List<Autor> autores, int ano)
    {
        if (string.IsNullOrWhiteSpace(titulo) || autores.Count == 0 ||
        autores.Any(a => string.IsNullOrWhiteSpace(a.Nome))||
        ano < 1000 ||
        ano > DateTime.Now.Year)
        {
            return false;
        }
        var LivroExistente = BuscarPorId(id);
        if (LivroExistente != null)
        {
            List<Autor> AutoresAtualizados = new List<Autor>();
            foreach(var autor in autores)
            {

                var autorExistente = AutoresAtualizados.FirstOrDefault(
                a => a.Nome.ToLower() == autor.Nome.ToLower()
                );

                if(autorExistente == null)
                {
                    autorExistente = _context.Autores.FirstOrDefault(
                    a => a.Nome.ToLower() == autor.Nome.ToLower()
                    );

                }

                if (autorExistente != null)
                {
                    AutoresAtualizados.Add(autorExistente);
                }
                else
                {
                    AutoresAtualizados.Add(autor);
                }
            }
            LivroExistente.Titulo = titulo;
            LivroExistente.Autores = AutoresAtualizados;
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