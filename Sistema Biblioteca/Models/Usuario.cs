using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Biblioteca.Models;

public class Usuario : IdentityUser<int>
{
    public string Nome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
}