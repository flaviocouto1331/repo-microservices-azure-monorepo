using loja_pedidos_api.Model.Request;

namespace loja_pedidos_api.Model.Response
{
    public class PedidosCompletoFilaResponse
    {
        public required PedidosFilaRequest PedidosFilaRequest { get; set; }
        public required PedidosResponse PedidosResponse { get; set; }
    }
}
