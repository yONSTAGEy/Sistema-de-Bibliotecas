using Biblioteca.Models;
using Biblioteca.Data;

using System.Text.RegularExpressions;
namespace Biblioteca.Service;

public class UsuarioService
{
    private readonly AppDbContext _context;

    public UsuarioService(AppDbContext context)
    {
        _context = context;
    }
    
    //CreateSystem.Net.Mail.MailAddress Email;
    public bool AdicionarUsuario(Usuario usuario)
    {
        if (string.IsNullOrWhiteSpace(usuario.Nome) || !ValidarCPF(usuario.CPF) || !ValidarEmail(usuario.Email))
        {
            return false;
        }
        _context.Usuario.Add(usuario);
        _context.SaveChanges();
        return true;
    }

    public bool ValidarEmail(string email)
    {
        string Email = email;
        //Rever isso depois, codigo RegEx, valida se o email é valido, mas não valida se o dominio existe
        if(!Regex.IsMatch(Email, @"^[^@]+@[^@. ]+\.[^@. ]+$"))
        {
            return false;
        }
        return true;
    }

    public bool ValidarCPF(string cpf)
    {
        string CPF = cpf.Replace(".", "").Replace("-", "");
        if(CPF.All(c => c == CPF[0]) || CPF.Length != 11 || !CPF.All(char.IsDigit))
        {
            return false;
        }

        int soma = 0;
        int primeiroDigito;

        for (int i = 0; i < 9; i++)
        {
            //Rever isto depois, o (CPF[i] - '0') é uma forma de converter o caractere em um número inteiro
            soma += (CPF[i] - '0') * (10 - i);
        }

        int resto = soma % 11;

        if (resto < 2)
        {
            primeiroDigito = 0;
            
        }
        else
        {
            primeiroDigito = 11 - resto;
        }
         
        if(primeiroDigito != (CPF[9] - '0'))
        {
            return false;
        }

        int somasegundo = 0;
        int segundoDigito;
        for (int i = 0; i < 10; i++)
        {
            somasegundo += (CPF[i] - '0') * (11 - i);
        }

        int restoSegundo = somasegundo % 11;

        if (restoSegundo < 2)
        {
            segundoDigito = 0;
            
        }
        else
        {
            segundoDigito = 11 - restoSegundo;
        }

        if(segundoDigito != (CPF[10] - '0'))
        {
            return false;
        }

        return true;
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
        if(_context.Emprestimo.Any(e => e.UsuarioID == id))
        {
            return false;
        }
        if(buscaUsuario != null)
        {
        _context.Usuario.Remove(buscaUsuario);
        _context.SaveChanges();
        return true;
        }
        return false;
         
    }
}