using Dapper;
using loja_produtos_api.Data.Dapper;
using loja_produtos_api.Model.Entities;
using loja_produtos_api.Model.Request;
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

        public async Task<bool> VerificarQtdEstoque(VerificarQtdEstoqueRequest produto)
        {
            StringBuilder query = new();
            query.Append(" select cast(");
            query.Append(" case when exists( select 1 from Produtos where Id = @IdProduto and EstoqueProduto > 0) then 1 else 0 end");
            query.Append(" as bit)");
            using var _ctx = _dapper.DapperConnection();
            bool condicao = await _ctx.ExecuteScalarAsync<bool>(
                query.ToString(),
                produto
                );
            return condicao;
        }
    }
}
