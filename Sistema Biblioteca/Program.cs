using Biblioteca.Models;
using Biblioteca.Service;
using Biblioteca.Helpers;
using Biblioteca.Data;

namespace SistemaDeBiblioteca
{
    class Program
    {
        static void Main()
        {
            AppDbContext context = new AppDbContext();
            EmprestimoService emprestimoService = new EmprestimoService(context);
            LivroService livroService = new LivroService(context);
            UsuarioService usuarioService = new UsuarioService(context);
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
                    Console.WriteLine("Quais o(s) autor(s)");
                    int quantidade = ConsoleHelper.LerInteiro("Quantos autores? ");
                    List<Autor> Autores = new List<Autor>();
                    for(int i = 0; i < quantidade; i++)
                    {
                        Console.WriteLine($"Nome do autor {i + 1}: ");
                        string nome = Console.ReadLine() ?? "";

                        Autor autor = new Autor()
                        {
                            Nome = nome
                        };

                        Autores.Add(autor);
                    }
                    int ano = ConsoleHelper.LerInteiro("Qual o ano?");
                    Livro livro = new Livro()
                    {
                        Titulo = titulo,
                        Autores = Autores ,
                        Ano = ano
                    };
                    livroService.AdicionarLivro(livro);
                }
                else if (op == 2)
                {
                    Console.WriteLine("Qual o nome do Usuário que deseja adicionar");
                    string NomeDoUCriado = Console.ReadLine() ?? "";
                    string CPFuCriado = ConsoleHelper.LerCPF("Qual o CPF do Usuário que deseja adicionar?");
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
                    var Listar = livroService.ListarLivros(null, null);
                    Console.WriteLine("=========== LISTAGEM DE LIVROS ===========");
                    foreach (Livro livro in Listar.Livros)
                    {
                        Console.WriteLine($"ID: {livro.ID}");
                        Console.WriteLine($"Titulo: {livro.Titulo}");
                        Console.WriteLine($" Autores: {string.Join(", ", livro.Autores.Select(a => a.Nome))}");
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
                    Console.WriteLine($"Autor: {string.Join(", ",livroSelecionado.Autores.Select(a => a.Nome))}");
                    Console.WriteLine($"Ano: {livroSelecionado.Ano}");
                    Console.WriteLine("-----------------------------");

                }
                else if (op == 5)
                {
                    var LivroEscolhido = ConsoleHelper.BuscarESelecionarLivro(livroService);
                    if (LivroEscolhido == null)
                    {
                        continue; // ou continue, dependendo de como está seu while
                    }

                    Console.WriteLine();
                    Console.WriteLine("===== LIVRO SELECIONADO =====");
                    Console.WriteLine($"Título: {LivroEscolhido.Titulo}");
                    Console.WriteLine($"Autores: {string.Join(", ", LivroEscolhido.Autores.Select(a => a.Nome))}");
                    Console.WriteLine($"Ano: {LivroEscolhido.Ano}");
                    Console.WriteLine("-----------------------------");
                    Console.WriteLine();

                    Console.WriteLine($"Qual o novo Titulo (Atual: {LivroEscolhido.Titulo})");
                    string novoTitulo = Console.ReadLine() ?? "";
                    if (string.IsNullOrWhiteSpace(novoTitulo))
                    {
                        novoTitulo = LivroEscolhido.Titulo; // Mantém o título atual se a entrada for vazia
                    }

                    Console.WriteLine("Deseja Alterar os Autores? S/N: ");
                    string Resposta = Console.ReadLine() ?? "";

                    while(Resposta.ToUpper() != "S" && Resposta.ToUpper() != "N")
                    {
                        Console.WriteLine("Resposta invalida, Digite S ou N");
                        Resposta = Console.ReadLine() ?? "";
                    }

                    List<Autor> autores = new List<Autor>();

                    if(Resposta.ToUpper() == "S")
                    {
                        int quantidade = ConsoleHelper.LerInteiro("Quantos novos autores? ");

                        while(quantidade <= 0)
                        {
                            Console.WriteLine("A quantidade precisa ser maior que 0");
                            quantidade = ConsoleHelper.LerInteiro("Quantos novos autores? ");
                        }
                        
                        for(int i = 0; i < quantidade; i++)
                        {
                            Console.WriteLine("Qual o nome do autor? ");
                            string nomeAutor = Console.ReadLine() ?? "";

                            while (string.IsNullOrWhiteSpace(nomeAutor))
                            {
                                Console.WriteLine("O nome não pode ficar vazio.");
                                Console.WriteLine("Qual o nome do autor? ");
                                nomeAutor = Console.ReadLine() ?? "";
                            }

                            Autor novoAutor = new Autor()
                            {
                                Nome = nomeAutor
                            };

                            autores.Add(novoAutor);
                        }
                    }
                    else if(Resposta.ToUpper() == "N")
                    {
                        autores = LivroEscolhido.Autores.ToList();
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
                    bool Atualizar = livroService.Update(LivroEscolhido.ID, novoTitulo, autores, Novoano);
                    if (Atualizar)
                    {
                        Console.WriteLine();
                        Console.WriteLine("===== LIVRO ATUALIZADO =====");
                        Console.WriteLine($"Título: {novoTitulo}");
                        Console.WriteLine($"Autores: {string.Join(", ", autores.Select(a => a.Nome))}");
                        Console.WriteLine($"Ano: {Novoano}");
                        Console.WriteLine("-----------------------------");
                    }
                    else
                    {
                        Console.WriteLine("Livro não encontrado");
                    }

                }
                else if (op == 6)
                {
                    var LivroEscolhido = ConsoleHelper.BuscarESelecionarLivro(livroService);
                    if (LivroEscolhido == null)
                    {
                        continue; // ou continue, dependendo de como está seu while
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
                    var LivroEscolhido = ConsoleHelper.BuscarESelecionarLivro(livroService);
                    if (LivroEscolhido == null)
                    {
                        continue; // ou continue, dependendo de como está seu while
                    }

                    int i = 1;

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
                        UsuarioID = UsuarioEscolhido.Id,
                        LivroID = LivroEscolhido.ID,
                    };

                    string Criado = emprestimoService.Emprestimo(emprestimoLivro);
                    if (Criado == "Empréstimo feito")
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
                    var emprestimosDoUsuario = emprestimoService.ListarEmprestimo().Where(e => e.UsuarioID == usuarioSelecionado.Id).ToList();

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
                    var emprestimosPendentes = ListarEmprestimo.Where(e => e.UsuarioID == usuarioSelecionado.Id && e.DataDevolucaoReal == null).ToList();

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

                    string resultado = emprestimoService.DevolverEmprestimo(emprestimoSelecionado.ID, usuarioSelecionado.Id);
                    if (resultado == "Devolução realizada")
                    {
                        Console.WriteLine("Livro devolvido com sucesso!");
                    }
                    else
                    {
                        Console.WriteLine(resultado);
                    }

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