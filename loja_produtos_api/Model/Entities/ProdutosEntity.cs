namespace loja_produtos_api.Model.Entities
{
    public class ProdutosEntity : BaseEntity
    {
        public ProdutosEntity() { }
        public ProdutosEntity(Guid id, string nomeProduto, decimal valorProduto, int estoqueProduto) 
            : base(id) 
        {
            NomeProduto = nomeProduto;
            ValorProduto = valorProduto;
            EstoqueProduto = estoqueProduto;
        }

        public string NomeProduto { get; private set; } = null!;
        public void AlterarNomeProduto(string nomeProduto) => NomeProduto = nomeProduto.Trim().ToUpperInvariant();
        public decimal ValorProduto { get; private set; }
        public int EstoqueProduto { get; private set; }

    }
}
