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
                    return Results.Ok(new { sts = true, msg = "Solicitação efetuada com sucesso.", Cartao = condicao });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { sts = false, msg = $"Api Erro: { ex.Message }" }); 
                }
            }).WithTags("Cartão");
        }
    }
}
