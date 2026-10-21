using Azure.Messaging.ServiceBus;
using azure_function_pagamento_worker.Model.Request;
using azure_function_pagamento_worker.Model.Response;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;

namespace azure_function_pagamento_worker;

public class FunctionPagamentoServiceBusTriegger
{
    private readonly ILogger<FunctionPagamentoServiceBusTriegger> _logger;
    private readonly IConfiguration _configuracao;
    private readonly HttpClient _http;

    public FunctionPagamentoServiceBusTriegger(ILogger<FunctionPagamentoServiceBusTriegger> logger, IConfiguration configuracao, HttpClient http)
    {
        _logger = logger;
        _configuracao = configuracao ?? throw new ArgumentNullException("Configuração inválida.", nameof(configuracao));
        _http = http ?? throw new ArgumentNullException("Configuração inválida.", nameof(http));
    }

    [Function(nameof(FunctionPagamentoServiceBusTriegger))]
    public async Task Run(
        [ServiceBusTrigger("fila-pagamento", Connection = "AzureServiceBusConnectionString")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation("Message ID: {id}", message.MessageId);
        _logger.LogInformation("Message Body: {body}", message.Body);
        _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

        _logger.LogInformation("Inciando azure service bus.");
        _logger.LogInformation("Requisição mensageria fila-pagamento recebida.");

        try
        {
            #region pedido

            var requisicao = JsonSerializer.Deserialize<PedidosFilaRequest>(message.Body.ToString());
            if (requisicao is null) throw new ArgumentNullException("Function json inválido.", nameof(requisicao));
            _logger.LogInformation("Pedido Id {Id} IdCliente {IdCliente} CartaoCliente {CartaoCliente} IdProduto {IdProduto}", requisicao.Id, requisicao.IdCliente, requisicao.CartaoCliente, requisicao.IdProduto);

            #endregion

            #region api cartão

            var cartao = new ConsultaCartaoRequest() { Cartao = requisicao.CartaoCliente };
            string cartaoUrl = _configuracao["url_loja_cartao_api"] ?? throw new InvalidOperationException("Api url cartão inválida");

            using HttpResponseMessage cartaoApi = await _http.PostAsJsonAsync(cartaoUrl, cartao);
            string cartaoApiCondicao;
            if (cartaoApi.IsSuccessStatusCode)
            {
                var cartaoApiData = await cartaoApi.Content.ReadFromJsonAsync<ConsultaCartaoResponse>();
                if (cartaoApiData is null) throw new ArgumentException("Api json cartão inválido.");
                cartaoApiCondicao = cartaoApiData.CondicaoCartao == "APROVADO" ? "PEDIDO_AGUARDANDO_ENVIO" : "PEDIDO_CANCELADO_CARTAO_RECUSADO";
                _logger.LogInformation("Api cartão {CondicaoProduto} {produtoApiCondicao}", cartaoApiData.CondicaoCartao, cartaoApiCondicao);
            }
            else
            {
                var cartaoApiDataErro = await cartaoApi.Content.ReadAsStringAsync();
                _logger.LogError("Api cartão inválido {cartaoApiDataErro} {StatusCode}", cartaoApiDataErro, (int)cartaoApi.StatusCode);
                throw new ArgumentException($"Api cartão inválido {cartaoApiDataErro} {(int)cartaoApi.StatusCode}");
            }

            #endregion

            #region api pedido

            var pedido = new PedidosStatusPedidoRequest() { Id = requisicao.Id, StatusPedido = cartaoApiCondicao };
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

            _logger.LogInformation("Requisição mensageria fila-pagamento finalizada.");

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