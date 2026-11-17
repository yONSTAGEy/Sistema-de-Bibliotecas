
namespace SistemaBiblioteca.API.DTOs;

public class GoogleBooksItem
{
    public string Id {get; set;} = string.Empty;
    public GoogleBooksVolumeInfo? VolumeInfo {get; set;} 
}