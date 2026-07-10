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
            _clienteRepository.Inserir(cliente);
        }

        public IEnumerable<Cliente> Listar()
        {
            return _clienteRepository.Listar();
        }

        public void Atualizar(int id, Cliente cliente)
        {
            if (id <= 0)
                throw new ArgumentException("O id do cliente é inválido.");

            cliente.Id = id;

            ValidarCliente(cliente);
            _clienteRepository.Atualizar(cliente);
        }

        public void Excluir(int id)
        {
            if (id <= 0)
                throw new ArgumentException("O id do cliente é inválido.");

            _clienteRepository.Excluir(id);
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

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(cpf[i].ToString()) * (11 - i);

            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;

            return digito2 == int.Parse(cpf[10].ToString());
        }
    }
}
