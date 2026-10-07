using loja_cliente_api.Models.Entities;

namespace loja_cliente_api.Data.Repositories
{
    public interface IClienteRepository
    {
        Task<List<ClienteEntity>> PegarTodosCliente();
    }
}
