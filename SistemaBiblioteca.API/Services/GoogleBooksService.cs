using System.Net.Http.Json;
using SistemaBiblioteca.API.DTOs;

namespace SistemaDeBiblioteca.API.Services;

public class GoogleBooksService
{
    private readonly string _apiKey;
    private readonly HttpClient _httpClient;

    public GoogleBooksService(HttpClient httpClient, IConfiguration configuration)
    {
        this._apiKey = configuration["GoogleBooks:ApiKey"] ?? throw new InvalidOperationException("API Key não configurada");
        this._httpClient = httpClient;
    }

    public async Task<GoogleBooksResponse?> PesquisarLivros(string termo)
    {
        string url = "https://www.googleapis.com/books/v1/volumes?q=" + Uri.EscapeDataString(termo) + "&key=" + _apiKey;

        var resultado = await _httpClient.GetFromJsonAsync<GoogleBooksResponse>(url);
        return resultado;

    }


}


