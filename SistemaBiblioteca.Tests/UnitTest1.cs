using Biblioteca.Models;
using Xunit;

namespace SistemaBiblioteca.Tests;

public class EmprestimoTests
{
    [Fact]
    public void DeveCalcularMulta()
    {
        // Arrange
        var emprestimo = new Emprestimo
        {
            DataDevolucaoPrevista = new DateTime(2026, 9, 1),
            DataDevolucaoReal = new DateTime(2026, 9, 4)
        };

        // Act
        var multa = emprestimo.CalcularMulta();

        // Assert
        Assert.Equal(6m, multa);
    }

    [Fact]
    public void NaoDeveGerarMultaQuandoDevolvidoNoPrazo()
    {
        // Arrange
        var emprestimo = new Emprestimo
        {
            DataDevolucaoPrevista = new DateTime(2026, 9, 5),
            DataDevolucaoReal = new DateTime(2026, 9, 5)
        };

        // Act
        var multa = emprestimo.CalcularMulta();

        // Assert
        Assert.Equal(0m, multa);
    }

    [Fact]
    public void DeveCalcularMultaComDoisDiasDeAtraso()
    {
        // Arrange
        var emprestimo = new Emprestimo
        {
            DataDevolucaoPrevista = new DateTime(2026, 9, 1),
            DataDevolucaoReal = new DateTime(2026, 9, 3)
        };

        // Act
        var multa = emprestimo.CalcularMulta();

        // Assert
        Assert.Equal(4m, multa);
    }

    [Fact]
    public void DeveCalcularMultaComCincoDiasDeAtraso()
    {
        // Arrange
        var emprestimo = new Emprestimo
        {
            DataDevolucaoPrevista = new DateTime(2026, 9, 1),
            DataDevolucaoReal = new DateTime(2026, 9, 6)
        };

        // Act
        var multa = emprestimo.CalcularMulta();

        // Assert
        Assert.Equal(10m, multa);
    }

    [Fact]
    public void DeveEstarAtrasadoQuandoDataPrevistaJaPassou()
    {
        // Arrange
        var emprestimo = new Emprestimo
        {
            DataDevolucaoPrevista = DateTime.Now.AddDays(-2),
            DataDevolucaoReal = null
        };

        // Act
        var atrasado = emprestimo.EstaAtrasado();

        // Assert
        Assert.True(atrasado);
    }

    [Fact]
    public void NaoDeveEstarAtrasadoQuandoAindaEstaNoPrazo()
    {
        // Arrange
        var emprestimo = new Emprestimo
        {
            DataDevolucaoPrevista = DateTime.Now.AddDays(2),
            DataDevolucaoReal = null
        };

        // Act
        var atrasado = emprestimo.EstaAtrasado();

        // Assert
        Assert.False(atrasado);
    }

    [Fact]
    public void NaoDeveEstarAtrasadoQuandoJaFoiDevolvido()
    {
        // Arrange
        var emprestimo = new Emprestimo
        {
            DataDevolucaoPrevista = DateTime.Now.AddDays(-2),
            DataDevolucaoReal = DateTime.Now
        };

        // Act
        var atrasado = emprestimo.EstaAtrasado();

        // Assert
        Assert.False(atrasado);
    }
}