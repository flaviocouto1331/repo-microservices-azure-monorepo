using System.Text.Json.Serialization;

namespace loja_cliente_api.Models.Response
{
    public class ClienteResponse
    {
        [JsonPropertyOrder(1)]
        public Guid Id { get; set; }
        [JsonPropertyOrder(2)]
        public string Nome { get; set; } = null!;
        [JsonPropertyOrder(3)]
        public string Cartao { get; set; } = null!;
    }
}
