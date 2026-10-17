using azure_function_estoque_worker.Model.Request;
using azure_function_estoque_worker.Model.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace azure_function_estoque_worker;

public class FunctionHttpTrigger
{
    private readonly ILogger<FunctionHttpTrigger> _logger;
    private readonly IConfiguration _configuracao;
    private readonly HttpClient _http;

    public FunctionHttpTrigger(ILogger<FunctionHttpTrigger> logger, IConfiguration configuracao, HttpClient http)
    {
        _logger = logger;
        _configuracao = configuracao ?? throw new ArgumentNullException("Configuração inválida.", nameof(configuracao));
        _http = http ?? throw new ArgumentNullException("Configuração inválida.", nameof(http));
    }
    
    [Function("function-estoque-httptrigger")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
    {
        _logger.LogInformation("Function requisição recebida.");

        try
        {
            #region pedido

            var requisicao = await req.ReadFromJsonAsync<PedidosFilaRequest>();
            if (requisicao is null) throw new ArgumentNullException(nameof(requisicao), "Function json inválido.");
            _logger.LogInformation("Pedido Id {Id} IdCliente {IdCliente} CartaoCliente {CartaoCliente} IdProduto {IdProduto}", requisicao.Id, requisicao.IdCliente, requisicao.CartaoCliente, requisicao.IdProduto);

            #endregion

            await Task.Delay(30000)
;
            #region produto api 

            var produto = new VerificarQtdEstoqueRequest() { IdProduto = requisicao.IdProduto };
            string produtoUrl = _configuracao["url_loja_produtos_api"] ?? throw new InvalidOperationException("Api url produto inválida");

            using HttpResponseMessage produtoApi = await _http.PostAsJsonAsync(produtoUrl, produto);
            string produtoApiCondicao;
            if (produtoApi.IsSuccessStatusCode)
            {
                var produtoApiData = await produtoApi.Content.ReadFromJsonAsync<VerificarQtdEstoqueResponse>();
                if (produtoApiData is null) throw new ArgumentException("Api json produto inválido.");
                produtoApiCondicao = produtoApiData.CondicaoProduto ? "PEDIDO_SEPARADO" : "PEDIDO_CANCELADO_FALTA_ESTOQUE";
                _logger.LogInformation("Api produto {CondicaoProduto} {produtoApiCondicao}", produtoApiData.CondicaoProduto, produtoApiCondicao);
            }
            else
            {
                var produtoApiDataErro = await produtoApi.Content.ReadAsStringAsync();
                _logger.LogError("Api produto inválido {produtoApiDataErro} {StatusCode}", produtoApiDataErro, (int)produtoApi.StatusCode);
                throw new ArgumentException($"Api produto inválido {produtoApiDataErro} {(int)produtoApi.StatusCode}");
            }

            #endregion

            #region pedido api

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

            _logger.LogInformation("Function requisição finalizada.");
            return new OkObjectResult(new { status = true, mensagem = "Registro atualizado com sucesso." });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Requisição inválida.");
            return new BadRequestObjectResult(new { status = false, mensagem = "Requisição inválida.", detalhe = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError("Function erro.");
            return new ObjectResult(new { status = false, mensagem = "Erro na Fcuntion.", detalhe = ex.Message }) { StatusCode = StatusCodes.Status500InternalServerError };
        }
    }
}