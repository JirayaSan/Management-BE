using Management_BE.Models.DatabaseModels;

namespace Management_BE.Interfaces.Documents
{
    public interface IDocumentRepository
    {
        Task<Document> CreateAsync(Document user);

        Task<Document> GetByTitleAsync(string title);

        Task<List<Document>> GetDocumentWithUserByIdAsync(int idDocument);

        Task<List<Document>> GetDocumentsByUserIdAsync(int userId);
    }
}
