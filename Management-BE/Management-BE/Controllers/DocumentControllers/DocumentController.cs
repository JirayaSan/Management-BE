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
        public async Task<IActionResult> PostAddDocument(Document documentRequest)
        {
            Document document = new();

            if (documentRequest == null)
            {
                return BadRequest(new ErrorResponse("Register data not contains values"));
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

            return Ok(valueQueryJoin);
        }
    }
}
