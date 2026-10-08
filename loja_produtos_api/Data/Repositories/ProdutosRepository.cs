using Dapper;
using loja_produtos_api.Data.Dapper;
using loja_produtos_api.Model.Entities;
using System.Data;
using System.Text;

namespace loja_produtos_api.Data.Repositories
{
    public class ProdutosRepository : IProdutosRepository
    {
        private readonly DapperContext _dapper;
        public ProdutosRepository(DapperContext dapper) => _dapper = dapper ?? throw new ArgumentException("Dapper inválido.", nameof(dapper)); 

        public async Task<List<ProdutosEntity>> PegarTodosProdutos()
        {
            StringBuilder query = new();
            query.Append("select Id, NomeProduto, ValorProduto, EstoqueProduto from Produtos;");
            using var _ctx = _dapper.DapperConnection();
            var produtos = await _ctx.QueryAsync<ProdutosEntity>(
                query.ToString(),
                commandType: CommandType.Text
                );
            return [.. produtos];
        }
    }
}
