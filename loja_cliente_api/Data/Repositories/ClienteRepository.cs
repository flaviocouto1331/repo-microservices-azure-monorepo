using Dapper;
using loja_cliente_api.Data.Dapper;
using loja_cliente_api.Model.Entities;
using loja_cliente_api.Model.Request;
using System.Data;
using System.Text;

namespace loja_cliente_api.Data.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly DapperContext _dapper;
        public ClienteRepository(DapperContext dapper) => _dapper = dapper ?? throw new ArgumentException("Dapper inválido", nameof(dapper));

        public async Task<bool> ConfirmarCliente(ConfirmarClienteRequest cliente)
        {
            StringBuilder query = new();
            query.Append(" select cast(");
            query.Append(" case when exists ( select 1 from Cliente where Id = @IdCliente and Cartao = @CartaoCliente ) then 1 else 0 end");
            query.Append(" as bit)");
            using var _ctx = _dapper.DapperConnection();
            bool condicao = await _ctx.ExecuteScalarAsync<bool>(
                query.ToString(),
                cliente
                );
            return condicao;
        }

        public async Task<List<ClienteEntity>> PegarTodosCliente()
        {
            StringBuilder query = new();
            query.Append("select Id, Nome, Cartao from Cliente;");
            using var _ctx = _dapper.DapperConnection();
            List<ClienteEntity> clientes = (List<ClienteEntity>)await _ctx.QueryAsync<ClienteEntity>(
                query.ToString(),
                commandType: CommandType.Text
                );
            return [.. clientes];
        }
    }
}
