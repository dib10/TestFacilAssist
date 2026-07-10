using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using FacilAssist.API.Models;

namespace FacilAssist.API.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly string _connectionString;

        public ClienteRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string não encontrada.");
        }

        public void Inserir(Cliente cliente)
        {
            using IDbConnection dbConnection = new SqlConnection(_connectionString);

            var parametros = new DynamicParameters();
            parametros.Add("@Nome", cliente.Nome);
            parametros.Add("@Cpf", cliente.Cpf);
            parametros.Add("@DataNascimento", cliente.DataNascimento);
            parametros.Add("@Sexo", cliente.Sexo);
            parametros.Add("@SituacaoClienteId", cliente.SituacaoClienteId);

            dbConnection.Execute(
                "sp_InserirCliente",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

        public IEnumerable<Cliente> Listar()
        {
            using IDbConnection dbConnection = new SqlConnection(_connectionString);

            return dbConnection.Query<Cliente>(
                "sp_ListarClientes",
                commandType: CommandType.StoredProcedure
            );
        }

        public void Atualizar(Cliente cliente)
        {
            using IDbConnection dbConnection = new SqlConnection(_connectionString);

            var parametros = new DynamicParameters();
            parametros.Add("@Id", cliente.Id);
            parametros.Add("@Nome", cliente.Nome);
            parametros.Add("@Cpf", cliente.Cpf);
            parametros.Add("@DataNascimento", cliente.DataNascimento);
            parametros.Add("@Sexo", cliente.Sexo);
            parametros.Add("@SituacaoClienteId", cliente.SituacaoClienteId);

            dbConnection.Execute(
                "sp_AtualizarCliente",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

        public void Excluir(int id)
        {
            using IDbConnection dbConnection = new SqlConnection(_connectionString);

            var parametros = new DynamicParameters();
            parametros.Add("@Id", id);

            dbConnection.Execute(
                "sp_ExcluirCliente",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
