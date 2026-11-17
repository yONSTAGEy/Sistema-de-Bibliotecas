namespace SistemaBiblioteca.API.DTOs;

public class LivrosEmprestadosAtrasadosDTO
{
    public int ID {get; set;}
    public int LivroID {get; set;}
    public int UsuarioID {get; set;}
    public DateTime DataDevolucaoPrevista {get; set;}
    public decimal Multa {get; set;}
}