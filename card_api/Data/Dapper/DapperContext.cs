using Microsoft.Data.SqlClient;
using System.Data;

namespace card_api.Data.Dapper
{
    public class DapperContext
    {
        private readonly IConfiguration _configuracao;
        private readonly string conexao;
        public DapperContext(IConfiguration configuracao) 
        {
            _configuracao = configuracao ?? throw new ArgumentException("Configuração inválida.", nameof(configuracao));
            conexao = _configuracao.GetConnectionString("conexao") ?? throw new InvalidOperationException("Conexão inválida.");
        }

        public IDbConnection DapperConnecion() => new SqlConnection(conexao);
    }
}
