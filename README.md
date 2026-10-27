# 📚 Sistema de Bibliotecas

Sistema de gerenciamento de biblioteca desenvolvido em C# e .NET, com foco na organização de livros, usuários e empréstimos. O projeto está sendo desenvolvido para praticar conceitos de programação orientada a objetos, acesso a dados, APIs REST e testes automatizados.

## 🚀 Tecnologias utilizadas

- **C#** — linguagem de programação.
- **.NET 9** — plataforma de desenvolvimento.
- **Entity Framework Core** — mapeamento objeto-relacional (ORM).
- **SQLite** — banco de dados.
- **ASP.NET Core Web API** — criação de endpoints HTTP.
- **Swagger** — documentação e exploração dos endpoints da API.
- **xUnit** — testes automatizados.

## 📂 Estrutura do projeto

```text
Projeto Biblioteca/
├── Sistema Biblioteca/
│   ├── Data/
│   ├── Helpers/
│   ├── Migrations/
│   ├── Models/
│   ├── Service/
│   ├── Program.cs
│   └── Sistema Biblioteca.csproj
│
├── SistemaBiblioteca.API/
│   ├── DTOs/
│   ├── Controllers/
│   ├── Program.cs
│   └── SistemaBiblioteca.API.csproj
│
├── SistemaBiblioteca.Tests/
│   ├── EmprestimoServiceTest.cs
│   ├── LivroServiceTests.cs
│   └── SistemaBiblioteca.Tests.csproj
│
├── .gitignore
└── README.md
```

### Descrição dos projetos

- **Sistema Biblioteca:** aplicação principal, com modelos, serviços, acesso ao banco de dados e regras de negócio.
- **SistemaBiblioteca.API:** API REST para disponibilizar funcionalidades do sistema por meio de requisições HTTP.
- **SistemaBiblioteca.Tests:** projeto de testes automatizados para verificar o comportamento dos serviços.

## ⚙️ Pré-requisitos

Antes de executar o projeto, instale:

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Visual Studio Code](https://code.visualstudio.com/) ou Visual Studio.

## ▶️ Como executar

Abra um terminal na pasta raiz do projeto.

### 1. Executar a aplicação principal

```bash
dotnet run --project "Sistema Biblioteca/Sistema Biblioteca.csproj"
```

### 2. Executar a API

```bash
dotnet run --project "SistemaBiblioteca.API/SistemaBiblioteca.API.csproj"
```

Em ambiente de desenvolvimento, a interface do Swagger pode ser utilizada para visualizar e testar os endpoints da API. O endereço exato depende das configurações de execução exibidas no terminal.

### 3. Executar os testes

```bash
dotnet test "SistemaBiblioteca.Tests/SistemaBiblioteca.Tests.csproj"
```

## 🧩 Funcionalidades

O projeto contempla funcionalidades relacionadas a:

- Cadastro e gerenciamento de livros.
- Gerenciamento de usuários.
- Registro e controle de empréstimos.
- Cálculo de multas por atraso.
- Persistência de dados com SQLite.
- Disponibilização de funcionalidades por meio de uma API REST.
- Testes automatizados dos serviços.

## 📌 Objetivo

Este projeto faz parte do meu aprendizado em desenvolvimento de software com C# e .NET. O objetivo é evoluí-lo progressivamente, aprimorando a arquitetura, a qualidade do código, os testes e a experiência do usuário.

## 🛠️ Próximas etapas

- [ ] Revisar e ampliar os testes automatizados.
- [ ] Melhorar a documentação dos endpoints.
- [ ] Criar uma interface gráfica para o sistema.
- [ ] Desenvolver uma versão com Razor Pages.
- [ ] Recriar a interface utilizando Blazor.
- [ ] Desenvolver uma versão utilizando React.

---

**Desenvolvido por Vinicius** como projeto de estudo em C# e .NET.
