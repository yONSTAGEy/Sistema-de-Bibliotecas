using SistemaBiblioteca.API.Validators;

namespace SistemaBiblioteca.Tests.Validators;

public class CpfValidatorTests
{
    [Theory]
    [InlineData("52998224725")]
    [InlineData("11144477735")]
    public void Valido_DeveRetornarTrue_QuandoCpfForValido(string cpf)
    {
        // Act
        var resultado = CpfValidator.Valido(cpf);

        // Assert
        Assert.True(resultado);
    }

    [Theory]
    [InlineData("11111111111")]
    [InlineData("12345678900")]
    [InlineData("52998224724")]
    public void Valido_DeveRetornarFalse_QuandoCpfForInvalido(string cpf)
    {
        // Act
        var resultado = CpfValidator.Valido(cpf);

        // Assert
        Assert.False(resultado);
    }
}