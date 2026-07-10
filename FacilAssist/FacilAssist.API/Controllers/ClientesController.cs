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
        private readonly ILogger<ClientesController> _logger;

        public ClientesController(IClienteService clienteService, ILogger<ClientesController> logger)
        {
            _clienteService = clienteService;
            _logger = logger;
        }

        /// <summary>
        /// Cadastra um novo cliente.
        /// </summary>
        /// <param name="input">Dados do cliente que será cadastrado.</param>
        /// <returns>Mensagem de confirmação do cadastro.</returns>
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
            _logger.LogInformation("Cliente cadastrado pela API.");

            return StatusCode(201, new { mensagem = "Cliente cadastrado com sucesso!" });
        }

        /// <summary>
        /// Lista todos os clientes cadastrados.
        /// </summary>
        /// <returns>Lista de clientes com a descrição da situação.</returns>
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
            }).ToList();

            _logger.LogInformation("Listagem de clientes retornou {Quantidade} registros.", resultado.Count);

            return Ok(resultado);
        }

        /// <summary>
        /// Atualiza os dados de um cliente existente.
        /// </summary>
        /// <param name="id">Identificador do cliente.</param>
        /// <param name="input">Dados atualizados do cliente.</param>
        /// <returns>Mensagem de confirmação da atualização.</returns>
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
            _logger.LogInformation("Cliente {ClienteId} atualizado pela API.", id);

            return Ok(new { mensagem = "Cliente atualizado com sucesso!" });
        }

        /// <summary>
        /// Exclui um cliente pelo identificador.
        /// </summary>
        /// <param name="id">Identificador do cliente.</param>
        /// <returns>Mensagem de confirmação da exclusão.</returns>
        [HttpDelete("{id}")]
        public IActionResult ExcluirCliente(int id)
        {
            _clienteService.Excluir(id);
            _logger.LogInformation("Cliente {ClienteId} excluido pela API.", id);

            return Ok(new { mensagem = "Cliente excluido com sucesso!" });
        }
    }
}
