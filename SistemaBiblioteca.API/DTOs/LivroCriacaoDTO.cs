using System.ComponentModel.DataAnnotations;
using Biblioteca.Models;

public class LivroCriacaoDTO
{
    [Required]
    public string Titulo { get; set; } = string.Empty;
    [Required]
    public List<string> Autores { get; set; } = new();
    [Range(1000, 9999)]
    public int Ano { get; set; }
}