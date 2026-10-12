using loja_produtos_api.Model.Request;
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

            app.MapPost("/api/produtos/verificar", async (VerificarQtdEstoqueRequest produto, IProdutosService _service) => 
            {
                try
                {
                    bool condicao = await _service.VerificarQtdEstoque(produto);
                    return Results.Ok(new { sts = true, msg = "Solicitação efetuada com sucesso.", Produtos = condicao });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { sts = false, msg = $"Api Erro: { ex.Message }" });
                } 
            }).WithTags("Produtos");
        }
    }
}
