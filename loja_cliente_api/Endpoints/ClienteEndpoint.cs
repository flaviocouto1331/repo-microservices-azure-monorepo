using loja_cliente_api.Model.Response;
using loja_cliente_api.Services;

namespace loja_cliente_api.Endpoints
{
    public static class ClienteEndpoint
    {
        public static void AddClienteEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/cliente", async (IClienteService _service) => {
                try
                {
                    List<ClienteResponse> clientes = await _service.PegarTodosCliente();
                    if(clientes == null || !clientes.Any()) return Results.Ok(new { sts = true, msg = "Nenhum cliente encontrado.", Cliente = new List<ClienteResponse>() });
                    return Results.Ok(new { sts = true, msg = "Registro encontrado.", Cliente = clientes });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { sts = false, msg = $"Api Erro: {ex.Message}." });
                }        
            }).WithTags("Cliente");
        }
    }
}
