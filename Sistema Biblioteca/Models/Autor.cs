using System.Text.Json.Serialization;

namespace Biblioteca.Models;

public class Autor
{
    public int ID { get; set; }
    public string Nome { get; set; } = string.Empty;
    [JsonIgnore]
    public ICollection<Livro> Livros { get; set; } = new List<Livro>();

}