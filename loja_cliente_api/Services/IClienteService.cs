using loja_cliente_api.Model.Response;

namespace loja_cliente_api.Services
{
    public interface IClienteService
    {
        Task<List<ClienteResponse>> PegarTodosCliente();
    }
}
