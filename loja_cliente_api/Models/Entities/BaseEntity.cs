namespace loja_cliente_api.Models.Entities
{
    public class BaseEntity
    {
        public BaseEntity() { }
        public BaseEntity(Guid id) { Id = id; }

        public Guid Id { get; private set; } 
    }
}