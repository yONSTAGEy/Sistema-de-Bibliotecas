
namespace SistemaBiblioteca.API.DTOs;

public class GoogleBooksVolumeInfo
{
    public string Title {get; set; } = string.Empty;
    public List<string> Authors {get; set; } = new();
    public string PublishedDate {get; set; } = string.Empty;
    public string Description {get; set; } = string.Empty;
    public List<GoogleBooksIndustryIdentifier> IndustryIdentifiers {get; set; } = new();
    public GoogleBooksImageLinks? ImageLinks {get; set; }
}