using card_api.Model.Request;

namespace card_api.Service
{
    public interface IConsultaCartaoService
    {
        Task<string> ConsultarCartao(ConsultaCartaoRequest cartao);
    }
}
