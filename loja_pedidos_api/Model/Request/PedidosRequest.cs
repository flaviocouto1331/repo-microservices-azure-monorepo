namespace loja_pedidos_api.Model.Request
{
    public class PedidosRequest
    {
        public Guid IdCliente { get; set; }
        public string NomeCliente { get; set; } = null!;
        public string CartaoCliente { get; set; } = null!;
        public Guid IdProduto { get; set; }
        public string NomeProduto { get; set; } = null!;
    }
}
