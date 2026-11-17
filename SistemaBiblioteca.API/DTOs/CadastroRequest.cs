using System.ComponentModel.DataAnnotations;

namespace SistemaBiblioteca.API.DTOs;

public class CadastroRequest
{
    [Required]
    public string Nome { get; set; } = string.Empty;

    [Required]
    public string CPF { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string Senha { get; set; } = string.Empty;
}