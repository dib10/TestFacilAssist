using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using FacilAssist.API.Models;
using System.Data.Common;

namespace FacilAssist.API.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly string _connectionString;
        public ClienteRepository(IConfiguration configuration)
        {
            //pego a string de con do appsettings.Development.json
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string não encontrada.");
        }
        //Inserir
        public void Inserir(Cliente cliente)
        {
            using IDbConnection dbConnection = new SqlConnection(_connectionString); //abre e fecha a conexão automaticamente
            var parametros = new DynamicParameters();
            parametros.Add("@Nome", cliente.Nome);
            parametros.Add("@Cpf", cliente.Cpf);
            parametros.Add("@DataNascimento", cliente.DataNascimento);
            parametros.Add("@Sexo", cliente.Sexo);
            parametros.Add("@SituacaoClienteId", cliente.SituacaoClienteId);


            //chamando a procedure
            dbConnection.Execute(
                "sp_InserirCliente",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

        //Listar
        public IEnumerable<Cliente> Listar()
        {
            using IDbConnection dbConnection = new SqlConnection(_connectionString);

            return dbConnection.Query<Cliente>(
                "sp_ListarClientes",
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
