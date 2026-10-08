using loja_produtos_api.Model.Response;
using loja_produtos_api.Services;

namespace loja_produtos_api.Endpoints
{
    public static class ProdutosEndpoint
    {
        public static void AddProdutosEndpoint(this IEndpointRouteBuilder app) 
        {
            app.MapGet("/api/produtos", async (IProdutosService _service) =>
            {
                try
                {
                    var response = await _service.PegarTodosProdutos();
                    if (response == null || response.Count == 0) return Results.Ok(new { sts = true, msg = "Nenhum registro encontrado.", Produtos = new List<ProdutosResponse>() });
                    return Results.Ok(new { sts = true, msg = "Registro encontrado.", Produtos = response });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { sts = false, msg = $"Api Erro: {ex.Message}" });
                }
            }).WithTags("Produtos");
        }
    }
}
