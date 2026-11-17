
using Biblioteca.Data;
using Biblioteca.Models;
using Biblioteca.Service;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace SistemaBiblioteca.Tests;

public class LivroServiceTests
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

    private List<Autor> CriarAutores(string nome)
    {
        return new List<Autor>
        {
            new Autor { Nome = nome }
        };
    }

    [Fact]
    public void DeveAdicionarLivroValido()
    {
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new LivroService(context);

        var livro = new Livro
        {
            Titulo = "Dom Casmurro",
            Autores = CriarAutores("Machado de Assis"),
            Ano = 1899
        };

        var resultado = service.AdicionarLivro(livro);

        Assert.True(resultado);
        Assert.Single(context.Livros);
    }

    [Fact]
    public void NaoDeveAdicionarLivroSemTitulo()
    {
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new LivroService(context);

        var livro = new Livro
        {
            Titulo = "",
            Autores = CriarAutores("Machado de Assis"),
            Ano = 1899
        };

        var resultado = service.AdicionarLivro(livro);

        Assert.False(resultado);
        Assert.Empty(context.Livros);
    }

    [Fact]
    public void NaoDeveAdicionarLivroComTituloApenasEspacos()
    {
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new LivroService(context);

        var livro = new Livro
        {
            Titulo = "   ",
            Autores = CriarAutores("Machado de Assis"),
            Ano = 1899
        };

        var resultado = service.AdicionarLivro(livro);

        Assert.False(resultado);
        Assert.Empty(context.Livros);
    }

    [Fact]
    public void NaoDeveAdicionarLivroSemAutor()
    {
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new LivroService(context);

        var livro = new Livro
        {
            Titulo = "Dom Casmurro",
            Autores = new List<Autor>(),
            Ano = 1899
        };

        var resultado = service.AdicionarLivro(livro);

        Assert.False(resultado);
        Assert.Empty(context.Livros);
    }

    [Fact]
    public void NaoDeveAdicionarLivroComAutorApenasEspacos()
    {
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new LivroService(context);

        var livro = new Livro
        {
            Titulo = "Dom Casmurro",
            Autores = CriarAutores("   "),
            Ano = 1899
        };

        var resultado = service.AdicionarLivro(livro);

        Assert.False(resultado);
        Assert.Empty(context.Livros);
    }

    [Fact]
    public void NaoDeveAdicionarLivroComAnoMenorQueMil()
    {
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new LivroService(context);

        var livro = new Livro
        {
            Titulo = "Dom Casmurro",
            Autores = CriarAutores("Machado de Assis"),
            Ano = 999
        };

        var resultado = service.AdicionarLivro(livro);

        Assert.False(resultado);
        Assert.Empty(context.Livros);
    }

    [Fact]
    public void NaoDeveAdicionarLivroComAnoFuturo()
    {
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new LivroService(context);

        var livro = new Livro
        {
            Titulo = "Dom Casmurro",
            Autores = CriarAutores("Machado de Assis"),
            Ano = DateTime.Now.Year + 1
        };

        var resultado = service.AdicionarLivro(livro);

        Assert.False(resultado);
        Assert.Empty(context.Livros);
    }

    [Fact]
    public void DeveListarLivros()
    {
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new LivroService(context);

        context.Livros.AddRange(
            new Livro
            {
                Titulo = "Dom Casmurro",
                Autores = CriarAutores("Machado de Assis"),
                Ano = 1899
            },
            new Livro
            {
                Titulo = "O Cortiço",
                Autores = CriarAutores("Aluísio Azevedo"),
                Ano = 1890
            }
        );

        context.SaveChanges();

        var resultado = service.ListarLivros();

        Assert.Equal(2, resultado.TotalRegistros);
    }

    [Fact]
    public void DeveRetornarListaVaziaQuandoNaoHouverLivros()
    {
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new LivroService(context);

        var resultado = service.ListarLivros();

        Assert.Equal(0, resultado.TotalRegistros);
    }

    [Fact]
    public void DeveBuscarLivroPorTitulo()
    {
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new LivroService(context);

        context.Livros.Add(new Livro
        {
            Titulo = "Dom Casmurro",
            Autores = CriarAutores("Machado de Assis"),
            Ano = 1899
        });

        context.SaveChanges();

        var livros = service.BuscarPorTitulo("Dom Casmurro");

        Assert.Single(livros);
        Assert.Equal("Dom Casmurro", livros[0].Titulo);
    }

    [Fact]
    public void DeveBuscarLivroSemDiferenciarMaiusculasEMinusculas()
    {
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new LivroService(context);

        context.Livros.Add(new Livro
        {
            Titulo = "Dom Casmurro",
            Autores = CriarAutores("Machado de Assis"),
            Ano = 1899
        });

        context.SaveChanges();

        var livros = service.BuscarPorTitulo("DOM CASMURRO");

        Assert.Single(livros);
    }

    [Fact]
    public void DeveBuscarParteDoTitulo()
    {
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new LivroService(context);

        context.Livros.Add(new Livro
        {
            Titulo = "Dom Casmurro",
            Autores = CriarAutores("Machado de Assis"),
            Ano = 1899
        });

        context.SaveChanges();

        var livros = service.BuscarPorTitulo("Casmurro");

        Assert.Single(livros);
        Assert.Equal("Dom Casmurro", livros[0].Titulo);
    }

    [Fact]
    public void DeveRetornarListaVaziaQuandoTituloNaoForEncontrado()
    {
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new LivroService(context);

        context.Livros.Add(new Livro
        {
            Titulo = "Dom Casmurro",
            Autores = CriarAutores("Machado de Assis"),
            Ano = 1899
        });

        context.SaveChanges();

        var livros = service.BuscarPorTitulo("Harry Potter");

        Assert.Empty(livros);
    }

    [Fact]
    public void DeveBuscarLivroPorId()
    {
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new LivroService(context);

        var livro = new Livro
        {
            Titulo = "Dom Casmurro",
            Autores = CriarAutores("Machado de Assis"),
            Ano = 1899
        };

        context.Livros.Add(livro);
        context.SaveChanges();

        var resultado = service.BuscarPorId(livro.ID);

        Assert.NotNull(resultado);
        Assert.Equal("Dom Casmurro", resultado.Titulo);
    }


    [Fact]
    public void DeveRetornarNullQuandoIdNaoForEncontrado()
    {
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new LivroService(context);

        var resultado = service.BuscarPorId(999);

        Assert.Null(resultado);
    }

    [Fact]
    public void DeveAtualizarLivro()
    {
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new LivroService(context);

        var livro = new Livro
        {
            Titulo = "Dom Casmurro",
            Autores = CriarAutores("Machado de Assis"),
            Ano = 1899
        };

        context.Livros.Add(livro);
        context.SaveChanges();

        var resultado = service.Update(
            livro.ID,
            "Dom Casmurro Atualizado",
            CriarAutores("Machado de Assis"),
            1900
        );

        Assert.True(resultado);

        var livroAtualizado = context.Livros
            .Include(l => l.Autores)
            .FirstOrDefault(l => l.ID == livro.ID);

        Assert.NotNull(livroAtualizado);
        Assert.Equal("Dom Casmurro Atualizado", livroAtualizado.Titulo);
        Assert.Contains(
            livroAtualizado.Autores,
            a => a.Nome == "Machado de Assis"
        );
        Assert.Equal(1900, livroAtualizado.Ano);
    }

    [Fact]
    public void NaoDeveAtualizarLivroInexistente()
    {
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new LivroService(context);

        var resultado = service.Update(
            999,
            "Livro",
            CriarAutores("Autor"),
            2020
        );

        Assert.False(resultado);
    }

    [Fact]
    public void DeveRemoverLivro()
    {
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new LivroService(context);

        var livro = new Livro
        {
            Titulo = "Dom Casmurro",
            Autores = CriarAutores("Machado de Assis"),
            Ano = 1899
        };

        context.Livros.Add(livro);
        context.SaveChanges();

        var resultado = service.RemoverLivro(livro.ID);

        Assert.True(resultado);
        Assert.Empty(context.Livros);
    }

    [Fact]
    public void NaoDeveRemoverLivroInexistente()
    {
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new LivroService(context);

        var resultado = service.RemoverLivro(999);

        Assert.False(resultado);
    }
}

   