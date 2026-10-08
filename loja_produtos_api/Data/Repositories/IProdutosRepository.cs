using loja_produtos_api.Model.Entities;

namespace loja_produtos_api.Data.Repositories
{
    public interface IProdutosRepository
    {
        Task<List<ProdutosEntity>> PegarTodosProdutos();
    }
}
