using loja_pedidos_api.Data.Repositories;
using loja_pedidos_api.Model.Request;
using loja_pedidos_api.Model.Response;

namespace loja_pedidos_api.Services
{
    public class PedidosService : IPedidosService
    {
        private IPedidosRepository _repository;
        public PedidosService(IPedidosRepository repository) => _repository = repository ?? throw new ArgumentNullException("Repositório inválido.", nameof(repository));

        public Task AtualizarStatusPedido(PedidosStatusPedidoRequest pedido) => _repository.AtualizarStatusPedido(pedido);

        public async Task<PedidosCompletoFilaResponse> CriarPedido(PedidosRequest pedido)
        {
            var pedidoCompletoResponse = await _repository.CriarPedido(pedido);
            
            var pedidoFilaRequest = new PedidosFilaRequest()
            {
                Id = pedidoCompletoResponse.Id,
                IdCliente = pedidoCompletoResponse.IdCliente,
                CartaoCliente = pedidoCompletoResponse.CartaoCliente,
                IdProduto = pedidoCompletoResponse.IdProduto
            };

            var pedidoResponse = new PedidosResponse()
            {
                Id = pedidoCompletoResponse.Id,
                DataCadastro = pedidoCompletoResponse.DataCadastro,
                NomeCliente = pedidoCompletoResponse.NomeCliente,
                NomeProduto = pedidoCompletoResponse.NomeProduto,
                StatusPedido = pedidoCompletoResponse.StatusPedido
            };

            var pedidoCompletoFilaResponse = new PedidosCompletoFilaResponse()
            {
                PedidosFilaRequest = pedidoFilaRequest,
                PedidosResponse = pedidoResponse
            };

            return pedidoCompletoFilaResponse;
        }

        public async Task<List<PedidosResponse>> PegarTodosPedidos() => await _repository.PegarTodosPedidos();  
    }
}
