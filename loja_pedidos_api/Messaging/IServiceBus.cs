using loja_pedidos_api.Model.Request;
using loja_pedidos_api.Model.Response;

namespace loja_pedidos_api.Messaging
{
    public interface IServiceBus
    {
        Task EnviarMensagemFilaAzureServiceBus(PedidosFilaRequest pedido, string filaNome = "fila-estoque");
    }
}
