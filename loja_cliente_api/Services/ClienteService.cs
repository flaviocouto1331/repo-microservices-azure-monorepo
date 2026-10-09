using loja_cliente_api.Data.Repositories;
using loja_cliente_api.Model.Entities;
using loja_cliente_api.Model.Response;

namespace loja_cliente_api.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _repository;
        public ClienteService(IClienteRepository repository) => _repository = repository ?? throw new ArgumentException("Repository inválido.", nameof(repository));

        public async Task<List<ClienteResponse>> PegarTodosCliente()
        {
            List<ClienteEntity> cliente = await _repository.PegarTodosCliente();
            if (cliente.Count == 0) throw new Exception("Nenhum cliente encontrado.");
            List<ClienteResponse> clienteResponse = cliente.Select(o => new ClienteResponse
            {
                Id = o.Id,
                Nome = o.Nome,
                Cartao = o.Cartao
            }).ToList();           
            return [.. clienteResponse];
        }
    }
}
