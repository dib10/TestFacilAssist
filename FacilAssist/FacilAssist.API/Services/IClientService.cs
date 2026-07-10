using FacilAssist.API.Models;

namespace FacilAssist.API.Services
{
    public interface IClienteService
    {
        void ValidarCliente(Cliente cliente);
        void Inserir(Cliente cliente);
        IEnumerable<Cliente> Listar();
    }
}