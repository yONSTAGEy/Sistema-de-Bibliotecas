using System.ComponentModel.DataAnnotations;

public class LivroCriacaoDTO
{
    [Required]
    public string Titulo { get; set; } = string.Empty;
    [Required]
    public string Autor { get; set; } = string.Empty;
    [Range(1000, 9999)]
    public int Ano { get; set; }
}