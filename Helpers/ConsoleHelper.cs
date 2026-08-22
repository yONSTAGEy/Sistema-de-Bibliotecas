using Biblioteca.Service;
using Biblioteca.Models;

namespace Biblioteca.Helpers;

public static class ConsoleHelper
{
    static public void MostrarLivros(List<Livro> livros)
    {
        int i = 1;
        foreach (Livro livro in livros)
        {
            Console.WriteLine($" {i}. Titulo: {livro.Titulo}");
            Console.WriteLine($" Autor: {livro.Autor}");
            Console.WriteLine($" Ano: {livro.Ano}");
            i++;
            Console.WriteLine("-------------------------------------");
        }
    }

    public static void MostrarUsuarios(List<Usuario> usuarios)
    {
        int i = 1;
        foreach (Usuario usuario in usuarios)
        {
            Console.WriteLine($"{i}. Nome: {usuario.Nome}");
            i++;
        }
    }

    public static void MostrarEmprestimos(List<Emprestimo> emprestimos, LivroService livroService, string NomeUsuario)
    {
        int i = 1;
        foreach (Emprestimo emprestimo in emprestimos)
        {
            var livro = livroService.BuscarPorId(emprestimo.LivroID);


            if (livro != null)
            {
                Console.WriteLine($"{i}. Livro: {livro.Titulo}");
                Console.WriteLine($"Usuário: {NomeUsuario}");
                Console.WriteLine($"Data Empréstimo: {emprestimo.DataEmprestimo:dd/MM/yyyy}");
                Console.WriteLine($"Data Prevista: {emprestimo.DataDevolucaoPrevista:dd/MM/yyyy}");

                if (emprestimo.DataDevolucaoReal.HasValue)
                {
                    Console.WriteLine($"Data Devolução: {emprestimo.DataDevolucaoReal.Value:dd/MM/yyyy}");
                }
                else
                {
                    Console.WriteLine("Data Devolução: Não devolvido");
                }
                i++;
                Console.WriteLine("-------------------------------------");
            }
        }
    }

    public static int LerInteiro(string mensagem)
    {
        Console.WriteLine(mensagem);
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out int valor))
            {
                return valor;
            }
            else
            {
                Console.WriteLine("Entrada inválida. Por favor, digite um número inteiro.");
            }
        }
    }

    public static Livro? BuscarESelecionarLivro(LivroService livroService)
    {
        Console.WriteLine("Qual o Título do livro?");
        string titulo = Console.ReadLine() ?? string.Empty;
        var livrosEncontrados = livroService.BuscarPorTitulo(titulo);
        if (livrosEncontrados.Count == 0)
        {
            Console.WriteLine("Nenhum livro encontrado.");
            return null;
        }

        Console.WriteLine("=========== LIVROS ENCONTRADOS ===========");
        MostrarLivros(livrosEncontrados);
        var escolha = LerInteiro("Escolha o livro pelo número: ");
        var livroSelecionado = SelecaoHelper.SelecionarPorNumero(livrosEncontrados, escolha);
        if (livroSelecionado == null)
        {
            Console.WriteLine("Escolha inválida");
            return null;
        }
        return livroSelecionado;




    }






}