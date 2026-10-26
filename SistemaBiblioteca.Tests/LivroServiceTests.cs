
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

    [Fact]
    public void DeveAdicionarLivroValido()
    {
        // Arrange
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new LivroService(context);

        var livro = new Livro
        {
            Titulo = "Dom Casmurro",
            Autor = "Machado de Assis",
            Ano = 1899
        };

        // Act
        var resultado = service.AdicionarLivro(livro);

        // Assert
        Assert.True(resultado);
        Assert.Single(context.Livros);
    }

    [Fact]
    public void NaoDeveAdicionarLivroSemTitulo()
    {
        // Arrange
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new LivroService(context);

        var livro = new Livro
        {
            Titulo = "",
            Autor = "Machado de Assis",
            Ano = 1899
        };

        // Act
        var resultado = service.AdicionarLivro(livro);

        // Assert
        Assert.False(resultado);
        Assert.Empty(context.Livros);
    }

    [Fact]
    public void NaoDeveAdicionarLivroComTituloApenasEspacos()
    {
        // Arrange
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new LivroService(context);

        var livro = new Livro
        {
            Titulo = "   ",
            Autor = "Machado de Assis",
            Ano = 1899
        };

        // Act
        var resultado = service.AdicionarLivro(livro);

        // Assert
        Assert.False(resultado);
        Assert.Empty(context.Livros);
    }

    [Fact]
    public void NaoDeveAdicionarLivroSemAutor()
    {
        // Arrange
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new LivroService(context);

        var livro = new Livro
        {
            Titulo = "Dom Casmurro",
            Autor = "",
            Ano = 1899
        };

        // Act
        var resultado = service.AdicionarLivro(livro);

        // Assert
        Assert.False(resultado);
        Assert.Empty(context.Livros);
    }

    [Fact]
    public void NaoDeveAdicionarLivroComAutorApenasEspacos()
    {
        // Arrange
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new LivroService(context);

        var livro = new Livro
        {
            Titulo = "Dom Casmurro",
            Autor = "   ",
            Ano = 1899
        };

        // Act
        var resultado = service.AdicionarLivro(livro);

        // Assert
        Assert.False(resultado);
        Assert.Empty(context.Livros);
    }

    [Fact]
    public void NaoDeveAdicionarLivroComAnoMenorQueMil()
    {
        // Arrange
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new LivroService(context);

        var livro = new Livro
        {
            Titulo = "Dom Casmurro",
            Autor = "Machado de Assis",
            Ano = 999
        };

        // Act
        var resultado = service.AdicionarLivro(livro);

        // Assert
        Assert.False(resultado);
        Assert.Empty(context.Livros);
    }

    [Fact]
    public void NaoDeveAdicionarLivroComAnoFuturo()
    {
        // Arrange
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new LivroService(context);

        var livro = new Livro
        {
            Titulo = "Dom Casmurro",
            Autor = "Machado de Assis",
            Ano = DateTime.Now.Year + 1
        };

        // Act
        var resultado = service.AdicionarLivro(livro);

        // Assert
        Assert.False(resultado);
        Assert.Empty(context.Livros);
    }

    [Fact]
    public void DeveListarLivros()
    {
        // Arrange
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new LivroService(context);

        context.Livros.AddRange(
            new Livro
            {
                Titulo = "Dom Casmurro",
                Autor = "Machado de Assis",
                Ano = 1899
            },
            new Livro
            {
                Titulo = "O Cortiço",
                Autor = "Aluísio Azevedo",
                Ano = 1890
            }
        );

        context.SaveChanges();

        // Act
        var livros = service.ListarLivros();

        // Assert
        Assert.Equal(2, livros.Count);
    }

    [Fact]
    public void DeveRetornarListaVaziaQuandoNaoHouverLivros()
    {
        // Arrange
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new LivroService(context);

        // Act
        var livros = service.ListarLivros();

        // Assert
        Assert.Empty(livros);
    }

    [Fact]
    public void DeveBuscarLivroPorTitulo()
    {
        // Arrange
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new LivroService(context);

        context.Livros.Add(new Livro
        {
            Titulo = "Dom Casmurro",
            Autor = "Machado de Assis",
            Ano = 1899
        });

        context.SaveChanges();

        // Act
        var livros = service.BuscarPorTitulo("Dom Casmurro");

        // Assert
        Assert.Single(livros);
        Assert.Equal("Dom Casmurro", livros[0].Titulo);
    }

    [Fact]
    public void DeveBuscarLivroSemDiferenciarMaiusculasEMinusculas()
    {
        // Arrange
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new LivroService(context);

        context.Livros.Add(new Livro
        {
            Titulo = "Dom Casmurro",
            Autor = "Machado de Assis",
            Ano = 1899
        });

        context.SaveChanges();

        // Act
        var livros = service.BuscarPorTitulo("DOM CASMURRO");

        // Assert
        Assert.Single(livros);
    }

    [Fact]
    public void DeveBuscarParteDoTitulo()
    {
        // Arrange
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new LivroService(context);

        context.Livros.Add(new Livro
        {
            Titulo = "Dom Casmurro",
            Autor = "Machado de Assis",
            Ano = 1899
        });

        context.SaveChanges();

        // Act
        var livros = service.BuscarPorTitulo("Casmurro");

        // Assert
        Assert.Single(livros);
        Assert.Equal("Dom Casmurro", livros[0].Titulo);
    }

    [Fact]
    public void DeveRetornarListaVaziaQuandoTituloNaoForEncontrado()
    {
        // Arrange
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new LivroService(context);

        context.Livros.Add(new Livro
        {
            Titulo = "Dom Casmurro",
            Autor = "Machado de Assis",
            Ano = 1899
        });

        context.SaveChanges();

        // Act
        var livros = service.BuscarPorTitulo("Harry Potter");

        // Assert
        Assert.Empty(livros);
    }

    [Fact]
    public void DeveBuscarLivroPorId()
    {
        // Arrange
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new LivroService(context);

        var livro = new Livro
        {
            Titulo = "Dom Casmurro",
            Autor = "Machado de Assis",
            Ano = 1899
        };

        context.Livros.Add(livro);
        context.SaveChanges();

        // Act
        var resultado = service.BuscarPorId(livro.ID);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal("Dom Casmurro", resultado.Titulo);
    }

    [Fact]
    public void DeveRetornarNullQuandoIdNaoForEncontrado()
    {
        // Arrange
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new LivroService(context);

        // Act
        var resultado = service.BuscarPorId(999);

        // Assert
        Assert.Null(resultado);
    }

    [Fact]
    public void DeveAtualizarLivro()
    {
        // Arrange
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new LivroService(context);

        var livro = new Livro
        {
            Titulo = "Dom Casmurro",
            Autor = "Machado de Assis",
            Ano = 1899
        };

        context.Livros.Add(livro);
        context.SaveChanges();

        // Act
        var resultado = service.Update(
            livro.ID,
            "Dom Casmurro Atualizado",
            "Machado de Assis",
            1900
        );

        // Assert
        Assert.True(resultado);

        var livroAtualizado = context.Livros.Find(livro.ID);

        Assert.NotNull(livroAtualizado);
        Assert.Equal("Dom Casmurro Atualizado", livroAtualizado.Titulo);
        Assert.Equal("Machado de Assis", livroAtualizado.Autor);
        Assert.Equal(1900, livroAtualizado.Ano);
    }

    [Fact]
    public void NaoDeveAtualizarLivroInexistente()
    {
        // Arrange
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new LivroService(context);

        // Act
        var resultado = service.Update(
            999,
            "Livro",
            "Autor",
            2020
        );

        // Assert
        Assert.False(resultado);
    }

    [Fact]
    public void DeveRemoverLivro()
    {
        // Arrange
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new LivroService(context);

        var livro = new Livro
        {
            Titulo = "Dom Casmurro",
            Autor = "Machado de Assis",
            Ano = 1899
        };

        context.Livros.Add(livro);
        context.SaveChanges();

        // Act
        var resultado = service.RemoverLivro(livro.ID);

        // Assert
        Assert.True(resultado);
        Assert.Empty(context.Livros);
    }

    [Fact]
    public void NaoDeveRemoverLivroInexistente()
    {
        // Arrange
        using var connection = CriarConexao();
        using var context = CriarContexto(connection);
        var service = new LivroService(context);

        // Act
        var resultado = service.RemoverLivro(999);

        // Assert
        Assert.False(resultado);
    }
}
