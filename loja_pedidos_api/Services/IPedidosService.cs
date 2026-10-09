using loja_pedidos_api.Model.Entities;
using loja_pedidos_api.Model.Request;
using loja_pedidos_api.Model.Response;

namespace loja_pedidos_api.Services
{
    public interface IPedidosService
    {
        Task<List<PedidosResponse>> PegarTodosPedidos();
        Task<PedidosResponse> CriarPedido(PedidosRequest pedido);
    }
}
