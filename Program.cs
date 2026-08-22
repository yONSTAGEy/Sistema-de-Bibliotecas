using Biblioteca.Models;
using Biblioteca.Service;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Biblioteca.Helpers;

namespace SistemaDeBiblioteca
{
    class Program
    {
        static void Main()
        {
            EmprestimoService emprestimoService = new EmprestimoService();
            LivroService livroService = new LivroService();
            UsuarioService usuarioService = new UsuarioService();
            while (true)
            {
                Console.WriteLine("1 - Adicionar livros");
                Console.WriteLine("2 - Adicionar Usuário");
                Console.WriteLine("3 - Listar livros");
                Console.WriteLine("4 - Buscar livros");
                Console.WriteLine("5 - Atualizar livros");
                Console.WriteLine("6 - Remover livros");
                Console.WriteLine("7 - Criar empréstimo");
                Console.WriteLine("8 - Listar empréstimos");
                Console.WriteLine("9 - Devolver livro");
                Console.WriteLine("10 - Sair");
                int op = ConsoleHelper.LerInteiro("Escolha uma opção: ");

                if (op == 1)
                {
                    Console.WriteLine("Qual o titulo deseja adicionar");
                    string titulo = Console.ReadLine() ?? "";
                    Console.WriteLine("Qual o autor");
                    string autor = Console.ReadLine() ?? "";
                    int ano = ConsoleHelper.LerInteiro("Qual o ano?");
                    Livro livro = new Livro()
                    {
                        Titulo = titulo,
                        Autor = autor,
                        Ano = ano
                    };
                    livroService.AdicionarLivro(livro);
                }
                else if (op == 2)
                {
                    Console.WriteLine("Qual o nome do Usuário que deseja adicionar");
                    string NomeDoUCriado = Console.ReadLine() ?? "";
                    int CPFuCriado = ConsoleHelper.LerInteiro("Qual o CPF do Usuário que deseja adicionar?");
                    Console.WriteLine("Qual o Email do Usuário que deseja adicionar");
                    string EMAILuCriado = Console.ReadLine() ?? "";
                    Usuario usuario = new Usuario()
                    {
                        Nome = NomeDoUCriado,
                        CPF = CPFuCriado,
                        Email = EMAILuCriado
                    };
                    usuarioService.AdicionarUsuario(usuario);
                }
                else if (op == 3)
                {
                    var Listar = livroService.ListarLivros();
                    Console.WriteLine("=========== LISTAGEM DE LIVROS ===========");
                    foreach (Livro livro in Listar)
                    {
                        Console.WriteLine($"ID: {livro.ID}");
                        Console.WriteLine($"Titulo: {livro.Titulo }");
                        Console.WriteLine($"Autor: {livro.Autor}");
                        Console.WriteLine($"Ano: {livro.Ano}");
                        Console.WriteLine("----------------");
                    }
                }
                else if (op == 4)
                {
                    var livroSelecionado = ConsoleHelper.BuscarESelecionarLivro(livroService);
                    if (livroSelecionado == null)
                    {
                        continue; // ou continue, dependendo de como está seu while
                    }

                    Console.WriteLine();
                    Console.WriteLine("===== LIVRO SELECIONADO =====");
                    Console.WriteLine($"ID: {livroSelecionado.ID}");
                    Console.WriteLine($"Titulo: {livroSelecionado.Titulo}");
                    Console.WriteLine($"Autor: {livroSelecionado.Autor}");
                    Console.WriteLine($"Ano: {livroSelecionado.Ano}");
                    Console.WriteLine("-----------------------------");

                }
                else if (op == 5)
                {
                    Console.WriteLine("Qual o Título do livro");
                    string tituloBusca = Console.ReadLine() ?? "";
                    var Buscar = livroService.BuscarPorTitulo(tituloBusca);
                    Console.WriteLine("=========== LIVROS ENCONTRADOS ===========");
                    if (Buscar.Count == 0)
                    {
                        Console.WriteLine("Nenhum livro encontrado.");
                        continue; // ou continue, dependendo de como está seu while
                    }

                    ConsoleHelper.MostrarLivros(Buscar);

                    int escolha = ConsoleHelper.LerInteiro("Digite o número do livro escolhido: ");

                    var LivroEscolhido = SelecaoHelper.SelecionarPorNumero(Buscar, escolha);

                    if (LivroEscolhido == null)
                    {
                        Console.WriteLine("Escolha inválida.");
                        continue; // ou break, dependendo de como está seu while
                    }

                    Console.WriteLine();
                    Console.WriteLine("===== LIVRO SELECIONADO =====");
                    Console.WriteLine($"Título: {LivroEscolhido.Titulo}");
                    Console.WriteLine($"Autor: {LivroEscolhido.Autor}");
                    Console.WriteLine($"Ano: {LivroEscolhido.Ano}");
                    Console.WriteLine("-----------------------------");
                    Console.WriteLine();

                    Console.WriteLine($"Qual o novo Titulo (Atual: {LivroEscolhido.Titulo})");
                    string novoTitulo = Console.ReadLine() ?? "";
                    if (string.IsNullOrWhiteSpace(novoTitulo))
                    {
                        novoTitulo = LivroEscolhido.Titulo; // Mantém o título atual se a entrada for vazia
                    }
                    Console.WriteLine("Qual o novo Autor do livro");
                    string autor = Console.ReadLine() ?? "";
                    if (string.IsNullOrWhiteSpace(autor))
                    {
                        autor = LivroEscolhido.Autor; // Mantém o autor atual se a entrada for vazia
                    }
                    Console.WriteLine("Qual o novo Ano do livro");
                    string novoAnoInput = Console.ReadLine() ?? "";
                    int Novoano;
                    if (string.IsNullOrWhiteSpace(novoAnoInput))
                    {
                        Novoano = LivroEscolhido.Ano;// Mantém o ano atual se a entrada for vazia
                    }
                    
                    else if (!int.TryParse(novoAnoInput, out Novoano))
                    {
                        Console.WriteLine("Ano inválido. Mantendo o ano atual.");
                        Novoano = LivroEscolhido.Ano; // Mantém o ano atual se a entrada
                    }
                    bool Atualizar = livroService.Update(LivroEscolhido.ID, novoTitulo, autor, Novoano);
                    if (Atualizar)
                    {
                        Console.WriteLine();
                        Console.WriteLine("===== LIVRO ATUALIZADO =====");
                        Console.WriteLine($"Título: {novoTitulo}");
                        Console.WriteLine($"Autor: {autor}");
                        Console.WriteLine($"Ano: {novoAnoInput}");
                        Console.WriteLine("-----------------------------");
                    }
                    else
                    {
                        Console.WriteLine("Livro não encontrado");
                    }

                }
                else if (op == 6)
                {
                    Console.WriteLine("Qual o Título do livro");
                    string tituloBusca = Console.ReadLine() ?? "";
                    var Buscar = livroService.BuscarPorTitulo(tituloBusca);
                    Console.WriteLine("=========== LIVROS ENCONTRADOS ===========");
                    if (Buscar.Count == 0)
                    {
                        Console.WriteLine("Nenhum livro encontrado.");
                        continue; // ou continue, dependendo de como está seu while
                    }

                    ConsoleHelper.MostrarLivros(Buscar);

                    int escolha = ConsoleHelper.LerInteiro("Escolha um Livro pelo número");

                    var LivroEscolhido = SelecaoHelper.SelecionarPorNumero(Buscar, escolha);

                    if (LivroEscolhido == null)
                    {
                        Console.WriteLine("Escolha inválida.");
                        continue; // ou break, dependendo de como está seu while
                    }

                    Console.WriteLine($"Deseja realmente remover o livro \"{LivroEscolhido.Titulo}\"? (s/n)");
                    string confirmacao = Console.ReadLine() ?? "";
                    if (confirmacao.ToLower() == "s")
                    {
                        bool Remover = livroService.RemoverLivro(LivroEscolhido.ID);
                        if (Remover)
                        {
                            Console.WriteLine("Livro removido");
                        }
                        else
                        {
                            Console.WriteLine("Erro ao remover livro");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Operação cancelada.");
                    }
                }
                else if (op == 7)
                {
                    Console.WriteLine("Qual o Título do livro");
                    string TituloBusca = Console.ReadLine() ?? "";
                    var Buscar = livroService.BuscarPorTitulo(TituloBusca);

                    Console.WriteLine("=========== LIVROS ENCONTRADOS ===========");
                    if (Buscar.Count == 0)
                    {
                        Console.WriteLine("Nenhum livro encontrado.");
                        continue; // ou continue, dependendo de como está seu while
                    }

                    int i = 1;

                    ConsoleHelper.MostrarLivros(Buscar);

                    int escolha = ConsoleHelper.LerInteiro("Escolha um Livro pelo número");

                    var LivroEscolhido = SelecaoHelper.SelecionarPorNumero(Buscar, escolha);

                    if (LivroEscolhido == null)
                    {
                        Console.WriteLine("Escolha inválida.");
                        continue; // ou break, dependendo de como está seu while
                    }

                    i = 1;

                    Console.WriteLine("Qual o nome do Usuario");
                    string NomeDoU = Console.ReadLine() ?? "";
                    var BuscarU = usuarioService.BuscarPorNome(NomeDoU);

                    Console.WriteLine("=========== USUÁRIOS ENCONTRADOS ===========");
                    if (BuscarU.Count == 0)
                    {
                        Console.WriteLine("Nenhum usuário encontrado.");
                        continue; // ou continue, dependendo de como está seu while
                    }

                    foreach (Usuario usuario in BuscarU)
                    {
                        Console.WriteLine($" {i}. Nome: {usuario.Nome}");
                        Console.WriteLine($" CPF: {usuario.CPF}");
                        Console.WriteLine($" Email: {usuario.Email}");
                        i++;
                        Console.WriteLine("-------------------------------------");
                    }

                    int escolhaUSUARIO = ConsoleHelper.LerInteiro("Escolha um usuário pelo número");

                    var UsuarioEscolhido = SelecaoHelper.SelecionarPorNumero(BuscarU, escolhaUSUARIO);

                    if (UsuarioEscolhido == null)
                    {
                        Console.WriteLine("Escolha inválida.");
                        continue; // ou break, dependendo de como está seu while
                    }

                    Emprestimo emprestimoLivro = new Emprestimo()
                    {
                        UsuarioID = UsuarioEscolhido.ID,
                        LivroID = LivroEscolhido.ID,
                    };

                    bool Criado = emprestimoService.Emprestimo(emprestimoLivro);
                    if (Criado)
                    {
                        Console.WriteLine("Criado empréstimo");
                    }
                    else
                    {
                        Console.WriteLine("Erro ao criar empréstimo");
                    }

                }
                else if (op == 8)
                {
                    Console.WriteLine("=========== CONSULTAR EMPRÉSTIMOS ===========");

                    Console.WriteLine("Qual o nome do usuário para consultar os empréstimos:");
                    string nomeUsuario = Console.ReadLine() ?? "";

                    var usuario = usuarioService.BuscarPorNome(nomeUsuario);
                    if (usuario.Count == 0)
                    {
                        Console.WriteLine("Usuário não encontrado.");
                        continue;
                    }

                    ConsoleHelper.MostrarUsuarios(usuario);

                    int escolhaUSUARIO = ConsoleHelper.LerInteiro("Escolha um usuário pelo número");

                    var usuarioSelecionado = SelecaoHelper.SelecionarPorNumero(usuario, escolhaUSUARIO);
                    if (usuarioSelecionado == null)
                    {
                        Console.WriteLine("Escolha inválida.");
                        continue;
                    }
                    var emprestimosDoUsuario = emprestimoService.ListarEmprestimo().Where(e => e.UsuarioID == usuarioSelecionado.ID).ToList();

                    if (emprestimosDoUsuario.Count == 0)
                    {
                        Console.WriteLine("Nenhum empréstimo encontrado para este usuário.");
                        continue;
                    }

                    Console.WriteLine($"=========== EMPRÉSTIMOS DE {usuarioSelecionado.Nome.ToUpper()} ===========");
                    ConsoleHelper.MostrarEmprestimos(emprestimosDoUsuario, livroService, usuarioSelecionado.Nome);

                }
                else if (op == 9)
                {
                    Console.WriteLine("=========== DEVOLUÇÃO DE LIVRO ===========");
                    Console.WriteLine("Qual o nome do usuário para consultar os empréstimos:");
                    string nomeUsuario = Console.ReadLine() ?? "";

                    var usuario = usuarioService.BuscarPorNome(nomeUsuario);
                    if (usuario.Count == 0)
                    {
                        Console.WriteLine("Usuário não encontrado.");
                        continue;
                    }

                    ConsoleHelper.MostrarUsuarios(usuario);

                    int escolhaUSUARIO = ConsoleHelper.LerInteiro("Escolha um usuário pelo número");

                    var usuarioSelecionado = SelecaoHelper.SelecionarPorNumero(usuario, escolhaUSUARIO);
                    if (usuarioSelecionado == null)
                    {
                        Console.WriteLine("Escolha inválida.");
                        continue;
                    }

                    var ListarEmprestimo = emprestimoService.ListarEmprestimo();
                    var emprestimosPendentes = ListarEmprestimo.Where(e => e.UsuarioID == usuarioSelecionado.ID && e.DataDevolucaoReal == null).ToList();

                    if (emprestimosPendentes.Count == 0)
                    {
                        Console.WriteLine("Nenhum empréstimo encontrado deste usuário.");
                        continue; // ou continue, dependendo de como está seu while
                    }

                    

                    Console.WriteLine($"=========== EMPRÉSTIMOS DE {usuarioSelecionado.Nome.ToUpper()} ===========");

                    ConsoleHelper.MostrarEmprestimos(emprestimosPendentes, livroService, usuarioSelecionado.Nome);   

                    int escolhaEmprestimo = ConsoleHelper.LerInteiro("Escolha um empréstimo pelo número");

                    var emprestimoSelecionado = SelecaoHelper.SelecionarPorNumero(emprestimosPendentes, escolhaEmprestimo);

                    if (emprestimoSelecionado == null)
                    {
                        Console.WriteLine("Escolha inválida. ");
                        continue; // ou break, dependendo de como está seu while
                    }

                    emprestimoService.DevolverEmprestimo(emprestimoSelecionado);
                    Console.WriteLine("Livro devolvido com sucesso!");

                    var multa = emprestimoSelecionado.CalcularMulta();
                    if (multa > 0)
                    {
                        Console.WriteLine($"O empréstimo está atrasado. Multa: R${multa}");
                    }
                    else
                    {
                        Console.WriteLine("O empréstimo foi devolvido dentro do prazo.");
                    }

                }
                else if (op == 10)
                {
                    Console.WriteLine("Saindo...");
                    break;
                }
                else
                {
                    Console.WriteLine("Opção inválida");
                }

            }
        }
    }
}