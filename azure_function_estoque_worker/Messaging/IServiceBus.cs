using azure_function_estoque_worker.Model.Request;

namespace azure_function_estoque_worker.Messaging
{
    public interface IServiceBus
    {
        Task EnviarMensagemFilaAzureServiceBus(PedidosFilaRequest pedido, string filaNome);
    }
}
