using loja_produtos_api.Model.Entities;
using loja_produtos_api.Model.Request;

namespace loja_produtos_api.Data.Repositories
{
    public interface IProdutosRepository
    {
        Task<List<ProdutosEntity>> PegarTodosProdutos();
        Task<bool> VerificarQtdEstoque(VerificarQtdEstoqueRequest produto);
    }
}
