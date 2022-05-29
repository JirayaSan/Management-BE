namespace Management_BE.Models.RequestModels
{
    public class DocumentsRequest
    {
        public class DocumentRequest
        {
            public string Title { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string? Comment { get; set; }
            public DateTime DateDocument { get; set; }
            public string FilePath { get; set; } = string.Empty;
            public DateTime DateInsertDocument { get; set; }

            public int UserId { get; set; }
        }

        public class DocumentData
        {
            public int Id { get; set; }
            public string Title { get; set; } = string.Empty;
        }

        public class DocumentsData
        {
            public int Id { get; set; }
            public string Title { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string? Comment { get; set; }
            public DateTime DateDocument { get; set; }
            public string FilePath { get; set; } = string.Empty;
            public DateTime DateInsertDocument { get; set; }

            public int UserId { get; set; }
        }
    }
}
