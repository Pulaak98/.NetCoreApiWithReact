using Microsoft.EntityFrameworkCore;
using WebApi.Database_Context;
using WebApi.Model;

namespace WebApi.Repository
{
    public class DocumentRepository
    {
        private readonly WebApiDbContext _context;

        public DocumentRepository(WebApiDbContext context)
        {
            _context = context;
        }

        public async Task<Document> CreateAsync(Document document)
        {
            _context.Documents.Add(document);
            await _context.SaveChangesAsync();
            return document;
        }

        public async Task<List<Document>> GetDocumentsByStudentIdAsync(string studentId)
        {
            
            var documents = await _context.Documents
                    .Where(d => d.StudentId == studentId)
                    .ToListAsync();
            ;

            return documents;
        }

        public async Task<Document> GetDocumentByIdAsync(int documentId)
        {
            try
            {
                var document = await _context.Documents
                    .Where(d => d.Id == documentId)
                    .FirstOrDefaultAsync();

                return document;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public void Delete(Document documentToDelete)
        {
            _context.Documents.Remove(documentToDelete);
        }

        public async Task<int> DeleteAllDocumentsForStudentAsync(string studentId)
        {
            try
            {
                var documentsToDelete = await _context.Documents
                    .Where(d => d.StudentId == studentId)
                    .ToListAsync();

                if (documentsToDelete != null && documentsToDelete.Any())
                {
                    _context.Documents.RemoveRange(documentsToDelete);
                    return documentsToDelete.Count;
                }
                else
                {
                    return 0; 
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }



        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }


    }
}


       