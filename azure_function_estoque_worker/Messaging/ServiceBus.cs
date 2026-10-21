using Azure.Messaging.ServiceBus;
using azure_function_estoque_worker.Model.Request;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace azure_function_estoque_worker.Messaging
{
    public class ServiceBus : IServiceBus
    {
        private readonly IConfiguration _configuracao;
        public ServiceBus(IConfiguration configuracao) => _configuracao = configuracao ?? throw new ArgumentNullException("Configuração inválida.", nameof(configuracao));

        public async Task EnviarMensagemFilaAzureServiceBus(PedidosFilaRequest pedido, string filaNome)
        {
            string conexao = _configuracao["AzureServiceBusConnectionString"] ?? throw new InvalidCastException("AzureServiceBusConnectionString inválida.");
            string queueName = filaNome;

            await using var client = new ServiceBusClient(conexao);
            ServiceBusSender sender = client.CreateSender(queueName);

            string mensagemJson = JsonSerializer.Serialize(pedido);
            ServiceBusMessage mensagem = new ServiceBusMessage(mensagemJson);

            await sender.SendMessageAsync(mensagem);
        }
    }
}
