using Biblioteca.Models;
using Biblioteca.Data;

namespace Biblioteca.Service;

public class UsuarioService
{
    private readonly AppDbContext _context;

    public UsuarioService()
    {
        _context = new AppDbContext();
    }
    //Create
    public void AdicionarUsuario(Usuario usuario)
    {
        _context.Usuario.Add(usuario);
        _context.SaveChanges();
    }
    public Usuario? BuscarPorId(int id)
    {
        return _context.Usuario.Find(id);
    }

    public List<Usuario> BuscarPorNome(string nome)
    {
        return _context.Usuario
        .ToList()
        .Where(u => u.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase))
        .ToList();
    }

    public bool Update(Usuario usuario)

    {
        var UsuarioExistente = BuscarPorId(usuario.ID);
        if (UsuarioExistente != null)
        {
        _context.Usuario.Update(usuario);
        _context.SaveChanges();
        return true;
        }
        return false;
    }

    public bool RemoverUsuario(int id)
    {
        var buscaUsuario = BuscarPorId(id);
        if(buscaUsuario != null)
        {
        _context.Usuario.Remove(buscaUsuario);
        _context.SaveChanges();
        return true;
        }
        return false;
         
    }
}