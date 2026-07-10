using Microsoft.AspNetCore.Mvc;
using FacilAssist.API.DTOs;
using FacilAssist.API.Models;
using FacilAssist.API.Services;

namespace FacilAssist.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpPost]
        public IActionResult InserirCliente([FromBody] ClienteInputDto input)
        {
            var cliente = new Cliente
            {
                Nome = input.Nome,
                Cpf = input.Cpf,
                DataNascimento = input.DataNascimento,
                Sexo = input.Sexo,
                SituacaoClienteId = input.SituacaoClienteId
            };

            _clienteService.Inserir(cliente);

            return StatusCode(201, new { mensagem = "Cliente cadastrado com sucesso!" });
        }

        [HttpGet]
        public IActionResult ListarClientes()
        {
            var clientes = _clienteService.Listar();

            var resultado = clientes.Select(cliente => new ClienteOutputDto
            {
                Id = cliente.Id,
                Nome = cliente.Nome,
                Cpf = cliente.Cpf,
                DataNascimento = cliente.DataNascimento,
                Sexo = cliente.Sexo,
                SituacaoClienteId = cliente.SituacaoClienteId,
                SituacaoDescricao = cliente.SituacaoDescricao
            });

            return Ok(resultado);
        }

        [HttpPut("{id}")]
        public IActionResult AtualizarCliente(int id, [FromBody] ClienteUpdateDto input)
        {
            var cliente = new Cliente
            {
                Nome = input.Nome,
                Cpf = input.Cpf,
                DataNascimento = input.DataNascimento,
                Sexo = input.Sexo,
                SituacaoClienteId = input.SituacaoClienteId
            };

            _clienteService.Atualizar(id, cliente);

            return Ok(new { mensagem = "Cliente atualizado com sucesso!" });
        }

        [HttpDelete("{id}")]
        public IActionResult ExcluirCliente(int id)
        {
            _clienteService.Excluir(id);

            return Ok(new { mensagem = "Cliente excluido com sucesso!" });
        }
    }
}
