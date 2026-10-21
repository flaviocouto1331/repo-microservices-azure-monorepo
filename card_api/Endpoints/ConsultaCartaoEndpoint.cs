using card_api.Model.Request;
using card_api.Service;

namespace card_api.Endpoints
{
    public static class ConsultaCartaoEndpoint
    {
        public static void AddConsultaCartaoEndpoint(this IEndpointRouteBuilder app) 
        {
            app.MapPost("/api/cartao/consultar", async (ConsultaCartaoRequest cartao, IConsultaCartaoService _service) => {
                try
                {
                    string condicao = await _service.ConsultarCartao(cartao);
                    return Results.Ok(new { status = true, mensagem = "Solicitação efetuada com sucesso.", CondicaoCartao = condicao });
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { status = false, mensagem = "Requisição inválida.", detalhe = ex.Message }); 
                }
                catch (Exception ex) 
                {
                    return Results.Json(new { status = false, mensagem = "Erro na API.", detalhe = ex.Message }, statusCode: StatusCodes.Status500InternalServerError);
                }
            }).WithTags("Cartão");
        }
    }
}
