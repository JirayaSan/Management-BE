using Management_BE.Data.AuthenticationData;
using Management_BE.Interfaces.Documents;
using Management_BE.Models.DatabaseModels;
using Microsoft.EntityFrameworkCore;

namespace Management_BE.Repositories.Documents
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly ApplicationDataContext _applicationDataContext;

        public DocumentRepository(ApplicationDataContext applicationDataContext)
        {
            _applicationDataContext = applicationDataContext;
        }

        public async Task<Document> CreateAsync(Document documentRequest)
        {
            // Aggiungo i dati ricevuti nel data context
            _applicationDataContext.Document.Add(documentRequest);
            // Eseguo il salvataggio dei cambiamenti in modo asyncrono (attendo l'elaborazione)
            await _applicationDataContext.SaveChangesAsync();

            // Restituisco i valori inseriti
            return documentRequest;
        }

        public async Task<Document> GetByTitleAsync(string title)
        {
            Document documentData = new();

            documentData = await _applicationDataContext.Document.Where(t => t.Title.Equals(title))
                                                                .FirstOrDefaultAsync();

            return documentData;
        }

        public async Task<List<Document>> GetDocumentWithUserByIdAsync(int idDocument)
        {
            List<Document> documentData = await _applicationDataContext.Document
                                .Where(d => d.Id == idDocument)
                                .Include(u => u.User)
                                .Include(r => r.User.Role)
                                .ToListAsync();
            return documentData;
        }
        
    }
}
