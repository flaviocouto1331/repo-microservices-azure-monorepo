using loja_pedidos_api.Model.Request;
using loja_pedidos_api.Model.Response;

namespace loja_pedidos_api.Data.Repositories
{
    public interface IPedidosRepository
    {
        Task<List<PedidosResponse>> PegarTodosPedidos();
        Task<PedidosCompletoResponse> CriarPedido(PedidosRequest pedido);
        Task AtualizarStatusPedido(PedidosStatusPedidoRequest pedido);
    }
}
