using loja_pedidos_api.Messaging;
using loja_pedidos_api.Model.Request;
using loja_pedidos_api.Model.Response;
using loja_pedidos_api.Services;

namespace loja_pedidos_api.Endpoints
{
    public static class PedidosEndpoint
    {
        public static void AddPedidosEndpoint(this IEndpointRouteBuilder app) 
        {
            app.MapGet("/api/pedidos", async (IPedidosService _service) =>
            {
                try
                {
                    var pedidos = await _service.PegarTodosPedidos();
                    if (pedidos.Count == 0) return Results.Ok(new { status = true, mensagem = "Nenhum registro encontrado.", Pedidos = new List<PedidosResponse>() });
                    return Results.Ok(new { status = true, mensagem = "Registro encontrado.", Pedidos = pedidos });
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { status = false, mensagem = "Requisição inválida.", detalhe = ex.Message });
                }
                catch (Exception ex)
                {                    
                    return Results.Json(new { status = false, mensagem = "Erro na API.", detalhe = ex.Message }, statusCode: StatusCodes.Status500InternalServerError);
                }
            }).WithTags("Pedidos");

            //app.MapPost("/api/pedidos", async (PedidosRequest pedido, IConfiguration _configuracao, IPedidosService _service) =>
            app.MapPost("/api/pedidos", async (PedidosRequest pedido, IServiceBus _serviceBus, IPedidosService _service) =>
            {
                try
                {
                    var pedidoCompletoFilaResponse = await _service.CriarPedido(pedido);
 
                    #region Azure Service Bus

                    //await EnviarAzureMessagingServiceBus(_configuracao, pedidoCompletoResponse);
                    await _serviceBus.EnviarMensagemFilaAzureServiceBus(pedidoCompletoFilaResponse.PedidosFilaRequest);

                    #endregion

                    return Results.Ok(new { status = true, menssagem = "Registro criado com sucesso.", Pedido = pedidoCompletoFilaResponse.PedidosResponse });
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { status = false, messagem = "Requisição inválida.", detalhe = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.Json(new { status = false, menssagem = "Erro na API.", detalhe = ex.Message }, statusCode: StatusCodes.Status500InternalServerError);
                }
            }).WithTags("Pedidos");

            app.MapPut("/api/pedidos", async (PedidosStatusPedidoRequest pedido, IPedidosService _service) => {
                try
                {
                    await _service.AtualizarStatusPedido(pedido);
                    return Results.Ok(new { status = true, mensagem = "Registro atualizado com sucesso.", Id = pedido.Id });
                }
                catch (ArgumentException ex) 
                {
                    return Results.BadRequest(new { status = false, mensagem = "Requisição inválida.", detalhe = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.Json(new { status = false, mensagem = "Erro na API.", detalhe = ex.Message }, statusCode: StatusCodes.Status500InternalServerError);
                }
            }).WithTags("Pedidos");
        }

        //public static async Task EnviarAzureMessagingServiceBus(IConfiguration _configuracao, PedidosCompletoResponse pedido)
        //{
        //    string conexao = _configuracao["AzureServiceBusConnectionString"] ?? string.Empty;
        //    string queueName = "fila-estoque";

        //    await using var client = new ServiceBusClient(conexao);
        //    ServiceBusSender sender = client.CreateSender(queueName);

        //    string mensagemJson = JsonSerializer.Serialize(pedido);
        //    ServiceBusMessage mensagem = new ServiceBusMessage(mensagemJson);

        //    await sender.SendMessageAsync(mensagem);
        //}
    }
}
