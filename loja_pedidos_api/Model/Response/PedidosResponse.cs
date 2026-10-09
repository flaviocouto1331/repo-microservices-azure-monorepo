using System.Text.Json.Serialization;

namespace loja_pedidos_api.Model.Response
{
    public class PedidosResponse
    {
        [JsonPropertyOrder(1)]
        public Guid Id { get; set; }
        [JsonPropertyOrder(3)]
        public DateTime DataCadastro { get; set; }
        [JsonPropertyOrder(5)]
        public string NomeCliente { get; set; } = null!;
        [JsonPropertyOrder(8)]
        public string NomeProduto { get; set; } = null!;
        [JsonPropertyOrder(9)]
        public string StatusPedido { get; set; } = null!;
    }
}