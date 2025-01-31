namespace BiiGBackend.Models.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
