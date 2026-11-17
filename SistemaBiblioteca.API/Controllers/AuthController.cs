
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Biblioteca.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SistemaBiblioteca.API.DTOs;
using SistemaBiblioteca.API.Validators;


namespace SistemaBiblioteca.API.Controllers;

[ApiController]
[Route("api/autenticacao")]
[Tags("Autenticação")]
public class AuthController : ControllerBase
{
    private readonly UserManager<Usuario> _userManager;
    private readonly IConfiguration _configuration;

    public AuthController(
        UserManager<Usuario> userManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(
        CadastroRequest request)
    {
        var email = request.Email.Trim();
        var cpf = new string(request.CPF.Where(char.IsDigit).ToArray());
        if(cpf.Length != 11)
        return BadRequest(new
        {
            mensagem = "CPF precisa ter 11 dígitos"
        });

        if (!CpfValidator.Valido(cpf))
        {
            return BadRequest(new
            {
                mensagem = "CPF inválido"
            });
        }

        if (await _userManager.FindByEmailAsync(email) is not null)
            return Conflict(new { mensagem = "E-mail já cadastrado." });

        var usuarioExistente = _userManager.Users
            .FirstOrDefault(u => u.CPF == cpf);

        if (usuarioExistente is not null)
            return Conflict(new { mensagem = "CPF já cadastrado." });

        var usuario = new Usuario
        {
            Nome = request.Nome.Trim(),
            CPF = cpf,
            Email = email,
            UserName = email
        };

        var resultado = await _userManager.CreateAsync(
            usuario, request.Senha);

        if (!resultado.Succeeded)
            return BadRequest(new
            {
                erros = resultado.Errors.Select(e => e.Description)
            });

        var resultadoPapel = await _userManager.AddToRoleAsync(
            usuario, "Cliente");

        if (!resultadoPapel.Succeeded)
        {
            await _userManager.DeleteAsync(usuario);

            return StatusCode(500, new
            {
                mensagem = "Não foi possível atribuir o papel ao usuário."
            });
        }

        return Created("", new
        {
            mensagem = "Cadastro realizado com sucesso."
        });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var usuario = await _userManager.FindByEmailAsync(
            request.Email.Trim());

        if (usuario is null ||
            !await _userManager.CheckPasswordAsync(
                usuario, request.Senha))
        {
            return Unauthorized(new
            {
                mensagem = "E-mail ou senha inválidos."
            });
        }

        var papeis = await _userManager.GetRolesAsync(usuario);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new(ClaimTypes.Name, usuario.UserName ?? usuario.Email ?? ""),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        claims.AddRange(papeis.Select(
            papel => new Claim(ClaimTypes.Role, papel)));

        var chave = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("Jwt:Key não configurada.");

        var emissor = _configuration["Jwt:Issuer"]
            ?? throw new InvalidOperationException("Jwt:Issuer não configurado.");

        var audiencia = _configuration["Jwt:Audience"]
            ?? throw new InvalidOperationException("Jwt:Audience não configurada.");

        var credencial = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chave)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: emissor,
            audience: audiencia,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credencial);

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            expiracao = token.ValidTo,
            usuario = new
            {
                usuario.Id,
                usuario.Nome,
                usuario.Email,
                papeis
            }
        });
    }
}
