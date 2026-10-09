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
                    if (pedidos.Count == 0) return Results.Ok(new { sts = true, msg = "Nenhum registro encontrado.", Pedidos = new List<PedidosResponse>() });
                    return Results.Ok(new { sts = true, msg = "Registro encontrado.", Pedidos = pedidos });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { sts = false, msg = $"Api Erro: {ex.Message}" });
                }
            }).WithTags("Pedidos");

            app.MapPost("/api/pedidos", async (PedidosRequest pedido, IPedidosService _service) => 
            {
                try
                {
                    var pedidosResponse = await _service.CriarPedido(pedido);
                    return Results.Ok(new { sts = true, msg = "Registro inserido com sucesso.", Pedidos = pedidosResponse });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { sts = false, msg = $"Api Erro: { ex.Message }" });
                }
            }).WithTags("Pedidos");
        }
    }
}
