using System.Text.Json.Serialization;

namespace Management_BE.Models.DatabaseModels
{
    public class Document
    {
        // Primary Key
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Comment { get; set; }
        public DateTime DateDocument { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public DateTime DateInsertDocument { get; set; }

        // Foreign Key
        public int UserId { get; set; }
        public User? User { get; set; }

    }
}
