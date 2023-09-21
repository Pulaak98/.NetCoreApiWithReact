using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.BLL;
using WebApi.Model;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly DocumentManager _documentManager;

        public DocumentsController(DocumentManager documentManager)
        {
            _documentManager = documentManager;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadDocument([FromForm] UploadDocument model)
        {
            try
            {
                if (model.File != null && model.File.Length > 0)
                {

                    var createdDocument = await _documentManager.UploadDocumentAsync(model);


                    return Ok(createdDocument);
                }


                return BadRequest("Invalid file.");
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("mydocuments")]
        public async Task<IActionResult> GetMyDocuments(string studentId)
        {
            try
            {
                
                var documents = await _documentManager.GetDocumentsByStudentIdAsync(studentId);

                if (documents == null)
                {
                    return NotFound("Student not found or has no documents.");
                }

                
                return Ok(documents);
            }
            catch (Exception ex)
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{documentId}")]
        public async Task<IActionResult> UpdateDocument(int documentId, [FromForm] UpdateDocument model)
        {
            try
            {
              
                var updatedDocument = await _documentManager.UpdateDocumentAsync(documentId, model);

                if (updatedDocument == null)
                {
                    return NotFound("Document not found.");
                }

                
                return Ok(updatedDocument);
            }
            catch (Exception ex)
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{documentId}")]
        public async Task<IActionResult> DeleteDocument(int documentId)
        {
            try
            {
                
                var deletedDocument = await _documentManager.DeleteDocumentAsync(documentId);

                if (deletedDocument == null)
                {
                    return NotFound("Document not found.");
                }

                
                return Ok($"Document with ID {documentId} has been deleted.");
            }
            catch (Exception ex)
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("delete-multiple")]
        public async Task<IActionResult> DeleteMultipleDocuments([FromBody] List<int> documentIds)
        {
            try
            {
                
                var deletedDocumentIds = await _documentManager.DeleteMultipleDocumentsAsync(documentIds);

                if (deletedDocumentIds.Count == 0)
                {
                    return NotFound("No documents were deleted.");
                }

                
                return Ok($"Documents with IDs {string.Join(", ", deletedDocumentIds)} have been deleted.");
            }
            catch (Exception ex)
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpDelete("delete-all/{studentId}")]
        public async Task<IActionResult> DeleteAllDocuments(string studentId)
        {
            try
            {
                
                var deletedCount = await _documentManager.DeleteAllDocumentsForStudentAsync(studentId);

                if (deletedCount == 0)
                {
                    return NotFound("No documents were deleted.");
                }

                
                return Ok($"All documents for Student ID {studentId} have been deleted.");
            }
            catch (Exception ex)
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



    }
}



       





