using card_api.Model.Request;

namespace card_api.Data.Repositories
{
    public interface IConsultaCartaoRepository
    {
        Task<string> ConsultarCartao(ConsultaCartaoRequest cartao);
    }
}
