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
            try
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
            catch (ArgumentException ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = "O erro foi do nosso lado do servidor, tente mais tarde.", detalhe = ex.Message });
            }
        }
    }
}
