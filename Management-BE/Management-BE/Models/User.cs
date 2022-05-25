namespace Management_BE.Models
{
    public class User
    {
        // Primary Key
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime Date { get; set; }

        // Foreing key
        public int RoleId { get; set; }
        public Role? Role { get; set; }
    }
}
