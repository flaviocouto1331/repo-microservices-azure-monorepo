using loja_pedidos_api.Data.Repositories;
using loja_pedidos_api.Model.Request;
using loja_pedidos_api.Model.Response;

namespace loja_pedidos_api.Services
{
    public class PedidosService : IPedidosService
    {
        private IPedidosRepository _repository;
        public PedidosService(IPedidosRepository repository) => _repository = repository ?? throw new ArgumentException("Repositório inválido.", nameof(repository));

        public async Task<PedidosResponse> CriarPedido(PedidosRequest pedido) => await _repository.CriarPedido(pedido);

        public async Task<List<PedidosResponse>> PegarTodosPedidos()
        {
            var pedidos = await _repository.PegarTodosPedidos();
            if (pedidos.Count == 0) return [];
            var pedidosResponse = pedidos.Select(o => new PedidosResponse()
            {
                Id = o.Id,
                DataCadastro = o.DataCadastro,
                NomeCliente = o.NomeCliente,
                NomeProduto = o.NomeProduto,
                StatusPedido = o.StatusPedido
            }).ToList();
            return pedidosResponse;
        }
    }
}
