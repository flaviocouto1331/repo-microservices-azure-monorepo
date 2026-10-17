using Azure.Messaging.ServiceBus;
using azure_function_estoque_worker.Model.Request;
using azure_function_estoque_worker.Model.Response;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;

namespace azure_function_estoque_worker;

public class FunctionServiceBusTrigger
{
    private readonly ILogger<FunctionServiceBusTrigger> _logger;
    private readonly IConfiguration _configuracao;
    private readonly HttpClient _http;

    public FunctionServiceBusTrigger(ILogger<FunctionServiceBusTrigger> logger, IConfiguration configuracao, HttpClient http)
    {
        _logger = logger;
        _configuracao = configuracao ?? throw new ArgumentNullException("Configuração inválida.", nameof(configuracao));
        _http = http ?? throw new ArgumentNullException("Configuração inválida.", nameof(http));
    }

    [Function(nameof(FunctionServiceBusTrigger))]
    public async Task Run(
        [ServiceBusTrigger("fila-estoque", Connection = "AzureServiceBusConnectionString")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation("Message ID: {id}", message.MessageId);
        _logger.LogInformation("Message Body: {body}", message.Body);
        _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

        _logger.LogInformation("Inciando azure service bus.");
        _logger.LogInformation("Requisição mensageria fila-estoque recebida.");

        try
        {
            #region pedido

            var requisicao = JsonSerializer.Deserialize<PedidosFilaRequest>(message.Body.ToString());
            if (requisicao is null) throw new ArgumentNullException("Function json inválido.", nameof(requisicao));
            _logger.LogInformation("Pedido Id {Id} IdCliente {IdCliente} CartaoCliente {CartaoCliente} IdProduto {IdProduto}", requisicao.Id, requisicao.IdCliente, requisicao.CartaoCliente, requisicao.IdProduto);

            #endregion

            #region api produto

            var produto = new VerificarQtdEstoqueRequest() { IdProduto = requisicao.IdProduto };
            string produtoUrl = _configuracao["url_loja_produtos_api"] ?? throw new InvalidOperationException("Api url produto inválida");

            using HttpResponseMessage produtoApi = await _http.PostAsJsonAsync(produtoUrl, produto);
            string produtoApiCondicao;
            if (produtoApi.IsSuccessStatusCode)
            {
                var produtoApiData = await produtoApi.Content.ReadFromJsonAsync<VerificarQtdEstoqueResponse>();
                if (produtoApiData is null) throw new ArgumentException("Api json produto inválido.");
                produtoApiCondicao = produtoApiData.CondicaoProduto ? "PEDIDO_AGUARDANDO_PAGAMENTO" : "PEDIDO_CANCELADO_FALTA_ESTOQUE";
                _logger.LogInformation("Api produto {CondicaoProduto} {produtoApiCondicao}", produtoApiData.CondicaoProduto, produtoApiCondicao);
            }
            else
            {
                var produtoApiDataErro = await produtoApi.Content.ReadAsStringAsync();
                _logger.LogError("Api produto inválido {produtoApiDataErro} {StatusCode}", produtoApiDataErro, (int)produtoApi.StatusCode);
                throw new ArgumentException($"Api produto inválido {produtoApiDataErro} {(int)produtoApi.StatusCode}");
            }

            #endregion

            #region api pedido

            var pedido = new PedidosStatusPedidoRequest() { Id = requisicao.Id, StatusPedido = produtoApiCondicao };
            string pedidoUrl = _configuracao["url_loja_pedidos_api"] ?? throw new InvalidOperationException("Api url pedido inválida.");

            using HttpResponseMessage pedidoApi = await _http.PutAsJsonAsync(pedidoUrl, pedido);
            if (pedidoApi.IsSuccessStatusCode)
            {
                var pedidoApiData = await pedidoApi.Content.ReadFromJsonAsync<PedidosStatusPedidoResponse>();
                if (pedidoApiData is null) throw new ArgumentException("Api json pedido inválido.");
                _logger.LogInformation("Api pedido {Id}", pedidoApiData.Id);
            }
            else
            {
                var pedidoApiDataDataErro = await pedidoApi.Content.ReadAsStringAsync();
                _logger.LogError("Api pedido inválido {pedidoApiDataDataErro} {StatusCode}", pedidoApiDataDataErro, (int)pedidoApi.StatusCode);
                throw new ArgumentException($"Api produto inválido {pedidoApiDataDataErro} {(int)pedidoApi.StatusCode}");
            }

            #endregion

            // Complete the message
            await messageActions.CompleteMessageAsync(message);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Mensagem {MessageId} contém JSON inválido.", message.MessageId);
            await messageActions.DeadLetterMessageAsync(message, null, "JsonInvalido", "O corpo da mensagem não contém um JSON válido.");
            return;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Mensagem {MessageId} não contém os dados esperados.", message.MessageId);
            await messageActions.DeadLetterMessageAsync(message, null, "DadosObrigatoriosAusentes", ex.Message);
            return;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha ao processar a mensagem {MessageId}.", message.MessageId);
            throw;
        }
    }
}