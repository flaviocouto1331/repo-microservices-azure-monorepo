using loja_produtos_api.Model.Response;

namespace loja_produtos_api.Services
{
    public interface IProdutosService
    {
        Task<List<ProdutosResponse>> PegarTodosProdutos();
    }
}
