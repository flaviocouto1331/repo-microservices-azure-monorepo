using card_api.Data.Repositories;
using card_api.Model.Request;

namespace card_api.Service
{
    public class ConsultaCartaoService : IConsultaCartaoService
    {
        private readonly IConsultaCartaoRepository _repository;
        public ConsultaCartaoService(IConsultaCartaoRepository repository) => _repository = repository ?? throw new ArgumentException("Repositório inválido.", nameof(repository));

        public async Task<string> ConsultarCartao(ConsultaCartaoRequest cartao) => await _repository.ConsultarCartao(cartao);
  
    }
}
