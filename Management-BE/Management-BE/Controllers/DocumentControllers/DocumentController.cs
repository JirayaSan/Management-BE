using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Management_BE.Models.DatabaseModels;
using Management_BE.Services;
using Management_BE.Interfaces.Documents;
using static Management_BE.Models.RequestModels.DocumentsRequest;

namespace Management_BE.Controllers.DocumentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentRepository _iDocumentRepository;

        public DocumentController(IDocumentRepository iDocumentRepository)
        {
            _iDocumentRepository = iDocumentRepository;
        }

        [HttpPost("AddDocument")]
        public async Task<IActionResult> PostAddDocument(DocumentRequest documentRequest)
        {
            Document document = new();

            DocumentData documentData = new();

            if (documentRequest == null)
            {
                return BadRequest(new ErrorResponse("Document data not contains values"));
            }

            // Verifica titolo se già presente
            Document existingDocumentTitle = await _iDocumentRepository.GetByTitleAsync(documentRequest.Title);
            if (existingDocumentTitle != null)
            {
                return Conflict(new ErrorResponse("Title already exist."));
            }

            // Inizializzo l'oggetto da inviare
            document.Title = documentRequest.Title;
            document.Description = documentRequest.Description;
            document.Comment = documentRequest.Comment;
            document.DateDocument = documentRequest.DateDocument;
            document.FilePath = documentRequest.FilePath;
            document.DateInsertDocument = DateTime.Now;
            document.UserId = documentRequest.UserId;

            // Eseguo l'inserimento dei dati
            Document valueInsertData = await _iDocumentRepository.CreateAsync(document);

            var valueQueryJoin = await _iDocumentRepository.GetDocumentWithUserByIdAsync(valueInsertData.Id);

            documentData.Id = valueQueryJoin.FirstOrDefault().Id;
            documentData.Title = valueQueryJoin.FirstOrDefault().Title;

            return Ok(documentData);
        }

        [HttpPost("AllDocuments")]
        public async Task<IActionResult> PostAllDocument(int userId)
        {
            List<Document> documentsList = new();

            List<DocumentsData> documentsData = new();

            // Verifica dei dati presenti nella richiesta
            if (userId.ToString() == null)
            {
                return BadRequest(new ErrorResponse("User data request not contains values"));
            }

            // Query per l'acquisizione dei Documenti in base all'id dell'utente
            documentsList = await _iDocumentRepository.GetDocumentsByUserIdAsync(userId);

            // Inizializzazione dei dati da restituire
            // Tutte le informazioni (compreso id) del documento, nessuna informazione per l'utente
            //documentsData = documentsList;
            documentsData = documentsList.Select(res => new DocumentsData
            {
                Id = res.Id,
                Title = res.Title,
                Description = res.Description,
                Comment = res.Comment,
                DateDocument = res.DateDocument,
                FilePath = res.FilePath,
                DateInsertDocument = res.DateDocument,
                UserId = res.UserId
            }).ToList();

            return Ok(documentsData);
        }
    }
}
