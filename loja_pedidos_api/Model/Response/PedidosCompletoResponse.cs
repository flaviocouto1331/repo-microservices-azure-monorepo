using System.Text.Json.Serialization;

namespace loja_pedidos_api.Model.Response
{
    public class PedidosCompletoResponse : PedidosResponse
    {
        [JsonPropertyOrder(2)]
        public bool StatusCadastro { get; private set; }
        [JsonPropertyOrder(4)]
        public Guid IdCliente { get; private set; }
        [JsonPropertyOrder(6)]
        public string CartaoCliente { get; private set; } = null!;
        [JsonPropertyOrder(7)]
        public Guid IdProduto { get; private set; }
    }
}
