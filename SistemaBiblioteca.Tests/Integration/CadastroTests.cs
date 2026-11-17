using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using SistemaBiblioteca.Tests.Infrastructure;
using Xunit;

namespace SistemaBiblioteca.Tests.Integration;

public class CadastroTests : IDisposable
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public CadastroTests()
    {
        _factory = new CustomWebApplicationFactory();
        _client = _factory.CreateClient();
    }

    public void Dispose()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [Fact]
    public async Task Cadastro_ComDadosValidos_DeveRetornar201()
    {
        var cadastro = new
        {
            nome = "Usuario Teste",
            cpf = "52998224725",
            email = "teste@biblioteca.com",
            senha = "Senha123"
        };

        var resposta = await _client.PostAsJsonAsync(
            "/api/autenticacao/register",
            cadastro);

        Assert.Equal(HttpStatusCode.Created, resposta.StatusCode);
    }

    [Fact]
    public async Task Cadastro_ComCpfInvalido_DeveRetornar400()
    {
        var cadastro = new
        {
            nome = "Usuario Teste",
            cpf = "11111111111",
            email = "teste2@biblioteca.com",
            senha = "Senha123"
        };

        var resposta = await _client.PostAsJsonAsync(
            "/api/autenticacao/register",
            cadastro);

        Assert.Equal(HttpStatusCode.BadRequest, resposta.StatusCode);
    }

    [Fact]
    public async Task Cadastro_ComEmailExistente_DeveRetornar409()
    {
        var cadastro = new
        {
            nome = "Usuario Teste",
            cpf = "52998224725",
            email = "duplicado@biblioteca.com",
            senha = "Senha123"
        };

        // Primeiro cadastro
        var primeiraResposta = await _client.PostAsJsonAsync(
            "/api/autenticacao/register",
            cadastro);

        Assert.Equal(HttpStatusCode.Created, primeiraResposta.StatusCode);

        // Segundo cadastro com o mesmo e-mail
        var segundaResposta = await _client.PostAsJsonAsync(
            "/api/autenticacao/register",
            new
            {
                nome = "Outro Usuario",
                cpf = "11144477735",
                email = "duplicado@biblioteca.com",
                senha = "Senha123"
            });

        Assert.Equal(HttpStatusCode.Conflict, segundaResposta.StatusCode);
    }

    [Fact]
    public async Task Login_ComDadosValidos_DeveRetornar200ComToken()
    {
        // Arrange: cadastrar o usuário
        var cadastro = new
        {
            nome = "Usuario Login",
            cpf = "52998224725",
            email = "login@biblioteca.com",
            senha = "Senha123"
        };

        await _client.PostAsJsonAsync(
            "/api/autenticacao/register",
            cadastro);

        // Act: realizar login
        var login = new
        {
            email = "login@biblioteca.com",
            senha = "Senha123"
        };

        var resposta = await _client.PostAsJsonAsync(
            "/api/autenticacao/login",
            login);

        // Assert
        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

        var conteudo = await resposta.Content.ReadFromJsonAsync<JsonElement>();

        Assert.True(conteudo.TryGetProperty("token", out var token));
        Assert.False(string.IsNullOrWhiteSpace(token.GetString()));
    }

    [Fact]
    public async Task Login_ComSenhaIncorreta_DeveRetornar401()
    {
        // Arrange: cadastrar o usuário
        var cadastro = new
        {
            nome = "Usuario Login",
            cpf = "52998224725",
            email = "login@biblioteca.com",
            senha = "Senha123"
        };

        await _client.PostAsJsonAsync(
            "/api/autenticacao/register",
            cadastro);

        // Act: tentar login com senha incorreta
        var login = new
        {
            email = "login@biblioteca.com",
            senha = "SenhaErrada123"
        };

        var resposta = await _client.PostAsJsonAsync(
            "/api/autenticacao/login",
            login);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, resposta.StatusCode);
    }

    [Fact]
    public async Task Login_ComUsuarioInexistente_DeveRetornar401()
    {
        // Arrange: preparar credenciais de usuário inexistente
        var login = new
        {
            email = "naoexiste@biblioteca.com",
            senha = "Senha123"
        };

        // Act
        var resposta = await _client.PostAsJsonAsync(
            "/api/autenticacao/login",
            login);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, resposta.StatusCode);
    }

    [Fact]
    public async Task Login_ComDadosValidos_DeveGerarJwtValido()
    {
        // Arrange: cadastrar usuário
        var cadastro = new
        {
            nome = "Usuario JWT",
            cpf = "52998224725",
            email = "jwt@biblioteca.com",
            senha = "Senha123"
        };

        await _client.PostAsJsonAsync(
            "/api/autenticacao/register",
            cadastro);

        // Act: realizar login
        var login = new
        {
            email = "jwt@biblioteca.com",
            senha = "Senha123"
        };

        var resposta = await _client.PostAsJsonAsync(
            "/api/autenticacao/login",
            login);

        var conteudo = await resposta.Content
            .ReadFromJsonAsync<JsonElement>();

        var token = conteudo.GetProperty("token").GetString();

        // Assert
        Assert.NotNull(token);

        var handler = new JwtSecurityTokenHandler();

        var parametros = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "SistemaBiblioteca.API",

            ValidateAudience = true,
            ValidAudience = "SistemaBiblioteca.Client",

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    "ChaveExclusivaParaTestesBiblioteca123456789"
                )),

            ClockSkew = TimeSpan.FromSeconds(30)
        };

        var principal = handler.ValidateToken(
            token!,
            parametros,
            out var tokenValidado);

        Assert.NotNull(principal);
        Assert.True(tokenValidado.ValidTo > DateTime.UtcNow);
    }

}