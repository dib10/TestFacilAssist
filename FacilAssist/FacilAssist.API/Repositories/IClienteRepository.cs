using FacilAssist.API.Models;
namespace FacilAssist.API.Repositories
{
    public interface IClienteRepository
    {
        void Inserir(Cliente cliente);
        IEnumerable<Cliente> Listar();

        void Atualizar(Cliente cliente);
    }

}
