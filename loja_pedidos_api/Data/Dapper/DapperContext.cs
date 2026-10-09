using Microsoft.Data.SqlClient;
using System.Data;

namespace loja_pedidos_api.Data.Dapper
{
    public class DapperContext
    {
        private readonly IConfiguration _configuracao;
        private readonly string _conexao;

        public DapperContext(IConfiguration configuracao) 
        {
            _configuracao = configuracao ?? throw new ArgumentException("Configuração inválida.", nameof(configuracao));
            _conexao = _configuracao.GetConnectionString("conexao") ?? throw new InvalidOperationException("Configuração inválida.");
        }

        public IDbConnection DapperConnection() => new SqlConnection(_conexao);
    }
}
