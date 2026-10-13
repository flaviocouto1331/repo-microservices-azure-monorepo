namespace card_api.Model.Entities
{
    public class ConsultaCartao
    {
        public ConsultaCartao() { }
        public ConsultaCartao(string cartao, string cartaoStatus) 
        {
            Cartao = cartao;
            CartaoStatus = cartaoStatus;
        }

        public string Cartao { get; private set; } = null!;
        public string CartaoStatus { get; private set; } = null!;
    }
}
