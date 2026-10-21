using loja_pedidos_api.Model.Request;

namespace loja_pedidos_api.Messaging
{
    public interface IServiceBus
    {
        Task EnviarMensagemFilaAzureServiceBus(PedidosFilaRequest pedido, string filaNome);
    }
}
