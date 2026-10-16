namespace loja_pedidos_api.Model.Request
{
    public class PedidosStatusPedidoRequest
    {
        public Guid Id { get; set; }
        public string StatusPedido { get; set; } = null!;
    }
}
