namespace loja_cliente_api.Model.Entities
{
    public class ClienteEntity : BaseEntity
    {
        public ClienteEntity() { }
        public ClienteEntity(Guid id, string nome, string cartao)
            : base(id)
        {
            Nome = nome;
            Cartao = cartao;
        }

        public string Nome { get; private set; } = null!;
        public void AlterarNome(string nome) => Nome = nome.Trim().ToUpperInvariant();

        public string Cartao { get; private set; } = null!;
    }
}
