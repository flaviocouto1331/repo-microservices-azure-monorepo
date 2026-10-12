namespace loja_cliente_api.Model.Request
{
    public class ConfirmarClienteRequest
    {
        public Guid IdCliente { get; set; }
        public string CartaoCliente { get; set; } = null!;
    }
}
