using Dapper;
using loja_pedidos_api.Data.Dapper;
using loja_pedidos_api.Model.Request;
using loja_pedidos_api.Model.Response;
using System.Data;
using System.Text;

namespace loja_pedidos_api.Data.Repositories
{
    public class PedidosRepository : IPedidosRepository
    {
        private DapperContext _dapper;
        public PedidosRepository(DapperContext dapper) => _dapper = dapper ?? throw new ArgumentException("Dapper inválido.", nameof(dapper));

        public async Task<PedidosResponse> CriarPedido(PedidosRequest pedido)
        {
            StringBuilder query = new();
            query.Append(" insert into Pedidos");
            query.Append(" (IdCliente, NomeCliente, CartaoCliente, IdProduto, NomeProduto, StatusPedido)");
            query.Append(" output inserted.Id, inserted.DataCadastro, inserted.NomeCliente, inserted.NomeProduto, inserted.StatusPedido");
            query.Append(" values");
            query.Append(" (@IdCliente, @NomeCliente, @CartaoCliente, @IdProduto, @NomeProduto, @StatusPedido);");
            using var _ctx = _dapper.DapperConnection();
            var pedidoResponse = await _ctx.QuerySingleAsync<PedidosResponse>(
                query.ToString(),
                new
                {
                    pedido.IdCliente,
                    pedido.NomeCliente,
                    pedido.CartaoCliente,
                    pedido.IdProduto,
                    pedido.NomeProduto,
                    StatusPedido = "PEDIDO PROCESSADO"
                });
            return pedidoResponse;
        }

        public async Task<List<PedidosResponse>> PegarTodosPedidos()
        {
            StringBuilder query = new();
            query.Append("select Id, DataCadastro, NomeCliente, NomeProduto, StatusPedido from Pedidos order by DataCadastro desc;");
            using var _ctx = _dapper.DapperConnection();
            var pedidos = await _ctx.QueryAsync<PedidosResponse>(
                query.ToString(),
                commandType: CommandType.Text
                );
            return [.. pedidos];
        }
    }
}
