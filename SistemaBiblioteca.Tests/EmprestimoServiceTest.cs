using Biblioteca.Data;
using Biblioteca.Models;
using Biblioteca.Service;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace SistemaBiblioteca.Tests;

public class EmprestimoServiceTests
{
    private SqliteConnection CriarConexao()
{
    var connection = new SqliteConnection("Data Source=:memory:");
    connection.Open();
    return connection;
}
    private AppDbContext CriarContexto(SqliteConnection connection)
{
    var options = new DbContextOptionsBuilder<AppDbContext>()
        .UseSqlite(connection)
        .Options;

    var context = new AppDbContext(options);
    context.Database.EnsureCreated();

    return context;
}

    [Fact]
    public void DeveBuscarLivroPorId()
    {
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new EmprestimoService(context);

        var livro = new Livro
        {
            Titulo = "Dom Casmurro",
            Autor = "Machado de Assis",
            Ano = 1899
        };

        context.Livros.Add(livro);
        context.SaveChanges();

        var resultado = service.BuscarLivro(livro.ID);

        Assert.NotNull(resultado);
        Assert.Equal("Dom Casmurro", resultado.Titulo);
    }

    [Fact]
    public void DeveRetornarNullQuandoLivroNaoForEncontrado()
    {
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new EmprestimoService(context);

        var resultado = service.BuscarLivro(999);

        Assert.Null(resultado);
    }

    [Fact]
    public void DeveBuscarUsuarioPorId()
    {
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new EmprestimoService(context);

        var usuario = new Usuario
        {
            Nome = "João Silva",
            Email = "joao@gmail.com",
            CPF = "52998224725"
        };

        context.Usuario.Add(usuario);
        context.SaveChanges();

        var resultado = service.BuscarUsuario(usuario.ID);

        Assert.NotNull(resultado);
        Assert.Equal("João Silva", resultado.Nome);
    }

    [Fact]
    public void DeveRetornarNullQuandoUsuarioNaoForEncontrado()
    {
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new EmprestimoService(context);

        var resultado = service.BuscarUsuario(999);

        Assert.Null(resultado);
    }

    [Fact]
    public void DeveRealizarEmprestimo()
    {
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new EmprestimoService(context);

        var livro = new Livro
        {
            Titulo = "Dom Casmurro",
            Autor = "Machado de Assis",
            Ano = 1899
        };

        var usuario = new Usuario
        {
            Nome = "João Silva",
            Email = "joao@gmail.com",
            CPF = "52998224725"
        };

        context.Livros.Add(livro);
        context.Usuario.Add(usuario);
        context.SaveChanges();

        var emprestimo = new Emprestimo
        {
            LivroID = livro.ID,
            UsuarioID = usuario.ID
        };

        var resultado = service.Emprestimo(emprestimo);

        Assert.True(resultado);
        Assert.Single(context.Emprestimo);
    }

    [Fact]
    public void NaoDeveRealizarEmprestimoComLivroInexistente()
    {
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new EmprestimoService(context);

        var usuario = new Usuario
        {
            Nome = "João Silva",
            Email = "joao@gmail.com",
            CPF = "52998224725"
        };

        context.Usuario.Add(usuario);
        context.SaveChanges();

        var emprestimo = new Emprestimo
        {
            LivroID = 999,
            UsuarioID = usuario.ID
        };

        var resultado = service.Emprestimo(emprestimo);

        Assert.False(resultado);
        Assert.Empty(context.Emprestimo);
    }

    [Fact]
    public void NaoDeveRealizarEmprestimoComUsuarioInexistente()
    {
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new EmprestimoService(context);

        var livro = new Livro
        {
            Titulo = "Dom Casmurro",
            Autor = "Machado de Assis",
            Ano = 1899
        };

        context.Livros.Add(livro);
        context.SaveChanges();

        var emprestimo = new Emprestimo
        {
            LivroID = livro.ID,
            UsuarioID = 999
        };

        var resultado = service.Emprestimo(emprestimo);

        Assert.False(resultado);
        Assert.Empty(context.Emprestimo);
    }

    [Fact]
    public void NaoDeveEmprestarLivroQueJaEstaEmprestado()
    {
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new EmprestimoService(context);

        var livro = new Livro
        {
            Titulo = "Dom Casmurro",
            Autor = "Machado de Assis",
            Ano = 1899
        };

        var usuario1 = new Usuario
        {
            Nome = "João Silva",
            Email = "joao@gmail.com",
            CPF = "52998224725"
        };

        var usuario2 = new Usuario
        {
            Nome = "Pedro Silva",
            Email = "pedro@gmail.com",
            CPF = "12345678909"
        };

        context.Livros.Add(livro);
        context.Usuario.AddRange(usuario1, usuario2);
        context.SaveChanges();

        var primeiroEmprestimo = new Emprestimo
        {
            LivroID = livro.ID,
            UsuarioID = usuario1.ID
        };

        var primeiroResultado = service.Emprestimo(primeiroEmprestimo);

        var segundoEmprestimo = new Emprestimo
        {
            LivroID = livro.ID,
            UsuarioID = usuario2.ID
        };

        var segundoResultado = service.Emprestimo(segundoEmprestimo);

        Assert.True(primeiroResultado);
        Assert.False(segundoResultado);
        Assert.Single(context.Emprestimo);
    }

    [Fact]
    public void DevePermitirNovoEmprestimoDepoisDaDevolucao()
    {
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new EmprestimoService(context);

        var livro = new Livro
        {
            Titulo = "Dom Casmurro",
            Autor = "Machado de Assis",
            Ano = 1899
        };

        var usuario1 = new Usuario
        {
            Nome = "João Silva",
            Email = "joao@gmail.com",
            CPF = "52998224725"
        };

        var usuario2 = new Usuario
        {
            Nome = "Pedro Silva",
            Email = "pedro@gmail.com",
            CPF = "12345678909"
        };

        context.Livros.Add(livro);
        context.Usuario.AddRange(usuario1, usuario2);
        context.SaveChanges();

        var primeiroEmprestimo = new Emprestimo
        {
            LivroID = livro.ID,
            UsuarioID = usuario1.ID
        };

        Assert.True(service.Emprestimo(primeiroEmprestimo));

        service.DevolverEmprestimo(primeiroEmprestimo);

        var segundoEmprestimo = new Emprestimo
        {
            LivroID = livro.ID,
            UsuarioID = usuario2.ID
        };

        var resultado = service.Emprestimo(segundoEmprestimo);

        Assert.True(resultado);
        Assert.Equal(2, context.Emprestimo.Count());
    }

    [Fact]
    public void DeveDefinirDataDoEmprestimoComoHoje()
    {
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new EmprestimoService(context);

        var livro = new Livro
        {
            Titulo = "Dom Casmurro",
            Autor = "Machado de Assis",
            Ano = 1899
        };

        var usuario = new Usuario
        {
            Nome = "João Silva",
            Email = "joao@gmail.com",
            CPF = "52998224725"
        };

        context.Livros.Add(livro);
        context.Usuario.Add(usuario);
        context.SaveChanges();

        var emprestimo = new Emprestimo
        {
            LivroID = livro.ID,
            UsuarioID = usuario.ID
        };

        service.Emprestimo(emprestimo);

        Assert.Equal(DateTime.Today, emprestimo.DataEmprestimo);
    }

    [Fact]
    public void DeveDefinirDevolucaoPrevistaParaSeteDias()
    {
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new EmprestimoService(context);

        var livro = new Livro
        {
            Titulo = "Dom Casmurro",
            Autor = "Machado de Assis",
            Ano = 1899
        };

        var usuario = new Usuario
        {
            Nome = "João Silva",
            Email = "joao@gmail.com",
            CPF = "52998224725"
        };

        context.Livros.Add(livro);
        context.Usuario.Add(usuario);
        context.SaveChanges();

        var emprestimo = new Emprestimo
        {
            LivroID = livro.ID,
            UsuarioID = usuario.ID
        };

        service.Emprestimo(emprestimo);

        Assert.Equal(
            emprestimo.DataEmprestimo.AddDays(7),
            emprestimo.DataDevolucaoPrevista
        );
    }

    [Fact]
    public void DeveDefinirDevolucaoRealComoNullAoRealizarEmprestimo()
    {
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new EmprestimoService(context);

        var livro = new Livro
        {
            Titulo = "Dom Casmurro",
            Autor = "Machado de Assis",
            Ano = 1899
        };

        var usuario = new Usuario
        {
            Nome = "João Silva",
            Email = "joao@gmail.com",
            CPF = "52998224725"
        };

        context.Livros.Add(livro);
        context.Usuario.Add(usuario);
        context.SaveChanges();

        var emprestimo = new Emprestimo
        {
            LivroID = livro.ID,
            UsuarioID = usuario.ID,
            DataDevolucaoReal = DateTime.Now
        };

        service.Emprestimo(emprestimo);

        Assert.Null(emprestimo.DataDevolucaoReal);
    }

    [Fact]
    public void DeveListarEmprestimos()
    {
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new EmprestimoService(context);

        var livro = new Livro
        {
            Titulo = "Dom Casmurro",
            Autor = "Machado de Assis",
            Ano = 1899
        };

        var usuario = new Usuario
        {
            Nome = "João Silva",
            Email = "joao@gmail.com",
            CPF = "52998224725"
        };

        context.Livros.Add(livro);
        context.Usuario.Add(usuario);
        context.SaveChanges();

        context.Emprestimo.Add(new Emprestimo
        {
            LivroID = livro.ID,
            UsuarioID = usuario.ID
        });

        context.SaveChanges();

        var resultado = service.ListarEmprestimo();

        Assert.Single(resultado);
    }

    [Fact]
    public void DeveRetornarListaVaziaQuandoNaoExistiremEmprestimos()
    {
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new EmprestimoService(context);

        var resultado = service.ListarEmprestimo();

        Assert.Empty(resultado);
    }

    [Fact]
    public void DeveDevolverEmprestimo()
    {
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new EmprestimoService(context);

        var livro = new Livro
        {
            Titulo = "Dom Casmurro",
            Autor = "Machado de Assis",
            Ano = 1899
        };

        var usuario = new Usuario
        {
            Nome = "João Silva",
            Email = "joao@gmail.com",
            CPF = "52998224725"
        };

        context.Livros.Add(livro);
        context.Usuario.Add(usuario);
        context.SaveChanges();

        var emprestimo = new Emprestimo
        {
            LivroID = livro.ID,
            UsuarioID = usuario.ID
        };

        service.Emprestimo(emprestimo);

        service.DevolverEmprestimo(emprestimo);

        Assert.NotNull(emprestimo.DataDevolucaoReal);
    }
}