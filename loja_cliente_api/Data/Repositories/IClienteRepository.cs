using loja_cliente_api.Model.Entities;
using loja_cliente_api.Model.Request;

namespace loja_cliente_api.Data.Repositories
{
    public interface IClienteRepository
    {
        Task<List<ClienteEntity>> PegarTodosCliente();
        Task<bool> ConfirmarCliente(ConfirmarClienteRequest cliente);
    }
}
