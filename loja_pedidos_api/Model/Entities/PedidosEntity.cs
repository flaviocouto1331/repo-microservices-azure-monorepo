namespace loja_pedidos_api.Model.Entities
{
    public class PedidosEntity : BaseEntity
    {
        public PedidosEntity() { }
        public PedidosEntity(Guid id, DateTime dataCadastro, bool statusCadastro, Guid idCliente, string nomeCliente, string cartaoCliente, Guid idProduto, string nomeProduto, string statusPedido) 
            : base(id)
        { 
            DataCadastro = dataCadastro;
            StatusCadastro = statusCadastro;
            IdCliente = idCliente;
            NomeCliente = nomeCliente;
            CartaoCliente = cartaoCliente;
            IdProduto = idProduto;
            NomeProduto = nomeProduto;
            StatusPedido = statusPedido;
        }

        public DateTime DataCadastro { get; private set; }
        public bool StatusCadastro { get; private set; } 
        public Guid IdCliente { get; private set; }
        public string NomeCliente { get; private set; } = null!;
        public string CartaoCliente { get; private set; } = null!;
        public Guid IdProduto { get; private set; }
        public string NomeProduto { get; private set; } = null!;
        public string StatusPedido { get; private set; } = null!;
    }
}