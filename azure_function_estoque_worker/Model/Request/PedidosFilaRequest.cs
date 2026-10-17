namespace azure_function_estoque_worker.Model.Request
{
    public class PedidosFilaRequest
    {
        public Guid Id { get; set; }
        public Guid IdCliente { get; set; }
        public string CartaoCliente { get; set; } = null!;
        public Guid IdProduto { get; set; }
    }
}
