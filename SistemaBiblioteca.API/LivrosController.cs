using Biblioteca.Models;
using Biblioteca.Service;
using Microsoft.AspNetCore.Mvc;

namespace SistemaBiblioteca.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LivrosController : ControllerBase
    {
        private readonly LivroService _livroService;

        public LivrosController(LivroService livroService)
        {
            _livroService = livroService;
        }

        [HttpGet]
        public ActionResult<List<Livro>> ListarLivros()
        {
            return Ok(_livroService.ListarLivros());
        }

        [HttpGet("{id}")]
        public ActionResult<Livro> BuscarPorId(int id)
        {
            var livro = _livroService.BuscarPorId(id);

            if (livro == null)
            {
                return NotFound();
            }

            return Ok(livro);
        }

        [HttpPost]
        public ActionResult<Livro> AdicionarLivro([FromBody] LivroCriacaoDTO livroDTO)
        {
            Livro livro = new Livro
            {
                Ano = livroDTO.Ano,
                Autor = livroDTO.Autor,
                Titulo = livroDTO.Titulo
            };

            var NovoLivro = _livroService.AdicionarLivro(livro);
            if (NovoLivro == false)
            {
                return BadRequest("Não foi possível adicionar o livro.");
            }
            return CreatedAtAction(
                "BuscarPorId",
                new { id = livro.ID },
                livro
            );
        }

        [HttpPut("{id}")]
        public ActionResult<Livro> AtualizarLivro(int id, Livro livro)
        {
            if (_livroService.BuscarPorId(id) == null)
            {
                return NotFound("livro não encontrado");
            }

            var livroAtualizado = _livroService.Update(id, livro.Titulo, livro.Autor, livro.Ano);

            if (livroAtualizado == false)
            {
                return BadRequest("Dados inválidos para atualizar o livro.");

            }
            return Ok(livro);
        }

        [HttpDelete("{id}")]
        public ActionResult<Livro> ExcluirLivro(int id)
        {
            var livro = _livroService.BuscarPorId(id);

            if (livro == null)
            {
                return NotFound("Livro não encontrado");
            }

            var LivroExcluido = _livroService.RemoverLivro(id);

            if (LivroExcluido == false)
            {
                return BadRequest("Não é possível excluir um livro que possui empréstimo.");
            }

            return NoContent();
        }
    }
}