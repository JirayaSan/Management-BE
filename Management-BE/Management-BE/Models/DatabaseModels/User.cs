using System.Text.Json.Serialization;

namespace Management_BE.Models.DatabaseModels
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
        // One to one relationship with Role Table
        public int RoleId { get; set; }

        //[JsonIgnore]
        public Role? Role { get; set; }

        // Document data
        [JsonIgnore]
        public ICollection<Document>? Documents { get; set; }
    }
}
