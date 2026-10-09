using loja_produtos_api.Data.Repositories;
using loja_produtos_api.Model.Response;

namespace loja_produtos_api.Services
{
    public class ProdutosService : IProdutosService
    {
        private readonly IProdutosRepository _repository;
        public ProdutosService(IProdutosRepository repository) => _repository = repository ?? throw new ArgumentException("Repositório inválido.", nameof(repository));

        public async Task<List<ProdutosResponse>> PegarTodosProdutos()
        {
            var produtos = await _repository.PegarTodosProdutos();
            if (produtos.Count == 0) throw new ArgumentException("Nenhum registro encontrado.", nameof(produtos));
            var produtosResponse = produtos.Select(o => new ProdutosResponse
            {
                Id = o.Id,
                NomeProduto = o.NomeProduto,
                ValorProduto = o.ValorProduto,
                EstoqueProduto = o.EstoqueProduto
            }).ToList();
            return [.. produtosResponse];
        }
    }
}
