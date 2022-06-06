namespace Autonoma.Domain.Entities
{
    public class User : Entity
    {
        public string? Login { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? Email { get; set; }
    }
}