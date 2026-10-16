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
                    if (response == null || response.Count == 0) return Results.Ok(new { status = true, mensagem = "Nenhum registro encontrado.", Produtos = new List<ProdutosResponse>() });
                    return Results.Ok(new { status = true, mensagem = "Registro encontrado.", Produtos = response });
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { status = false, mensagem = "Requisição inválida.", detalhe = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.Json(new { status = false, mensagem = "Erro na API.", detalhe = ex.Message }, statusCode: StatusCodes.Status500InternalServerError);
                }
            }).WithTags("Produtos");

            app.MapPost("/api/produtos/verificar", async (VerificarQtdEstoqueRequest produto, IProdutosService _service) => 
            {
                try
                {
                    bool condicao = await _service.VerificarQtdEstoque(produto);
                    return Results.Ok(new { status = true, mensagem = "Solicitação efetuada com sucesso.", Produtos = condicao });
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { status = false, mensagem = "Requisição inválida.", detalhe = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.Json(new { status = false, mensagem = "Erro na API.", detalhe = ex.Message }, statusCode: StatusCodes.Status500InternalServerError);
                }
            }).WithTags("Produtos");
        }
    }
}
