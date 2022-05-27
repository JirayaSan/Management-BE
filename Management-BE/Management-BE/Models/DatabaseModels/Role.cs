using System.Text.Json.Serialization;

namespace Management_BE.Models.DatabaseModels
{
    public class Role
    {
        // Primary Key
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Foreign key
        // One to one relationship with User Table
        //[JsonIgnore]
        //public int IdUser { get; set; }
        //public User? User { get; set; }
    }
}
