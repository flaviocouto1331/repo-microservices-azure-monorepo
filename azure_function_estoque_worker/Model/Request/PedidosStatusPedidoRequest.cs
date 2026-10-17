namespace azure_function_estoque_worker.Model.Request
{
    public class PedidosStatusPedidoRequest
    {
        public Guid Id { get; set; }
        public string StatusPedido { get; set; } = null!;
    }
}
