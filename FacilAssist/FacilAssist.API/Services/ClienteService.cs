using System;
using System.Linq;
using System.Runtime.CompilerServices;
using FacilAssist.API.Models;
using FacilAssist.API.Repositories;

namespace FacilAssist.API.Services
{

    public class ClienteService : IClienteService
    {

        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public void Inserir(Cliente cliente)
        {
            ValidarCliente(cliente);
            _clienteRepository.Inserir(cliente); //salva no banco
        }

        public IEnumerable<Cliente> Listar()
        {
            return _clienteRepository.Listar();
        }



        public void ValidarCliente(Cliente cliente)
        {
            if (string.IsNullOrWhiteSpace(cliente.Nome))

                throw new ArgumentException("O nome do cliente é obrigatório.");

            if (string.IsNullOrWhiteSpace(cliente.Cpf))

                throw new ArgumentException("O CPF do cliente é obrigatório.");

            if (!CpfEhValido(cliente.Cpf))

                throw new ArgumentException("O CPF informado é inválido.");

            if (cliente.DataNascimento > DateTime.Now)

                throw new ArgumentException("A data de nascimento não pode ser no futuro.");
        }


        //métodos auxiliares/privados

        // Aqui eu poderia usar uma biblioteca externa para validar o CPF, mas utilzei a foirmula matemática para validar o CPF
        private bool CpfEhValido(string? cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf)) return false;

            
            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            if (cpf.Length != 11) return false;

            
            if (cpf.Distinct().Count() == 1) return false;

            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(cpf[i].ToString()) * (10 - i);

            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;

            if (digito1 != int.Parse(cpf[9].ToString())) return false;

            
            soma = 0 ;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(cpf[i].ToString()) * (11 - i);

            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;

            return digito2 == int.Parse(cpf[10].ToString());
        }

    }
        }