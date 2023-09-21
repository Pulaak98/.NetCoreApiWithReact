using WebApi.Model;
using WebApi.Repository;

namespace WebApi.BLL
{
    public class DocumentManager
    {
        private readonly DocumentRepository _documentRepository;
        private readonly StudentRepository _studentRepository;

        public DocumentManager(DocumentRepository documentRepository, StudentRepository studentRepository)
        {
            _documentRepository = documentRepository;
            _studentRepository = studentRepository;
        }

        public async Task<Document> UploadDocumentAsync(UploadDocument model)
        {

            var student = await _studentRepository.GetStudentByStudentIdAsync(model.StudentId);

            if (student == null)
            {
                throw new Exception("Student not found.");
            }

            var studentFolderPath = Path.Combine("uploads", "student_" + student.StudentId.ToString());

            if (!Directory.Exists(studentFolderPath))
            {
                Directory.CreateDirectory(studentFolderPath);
            }


            var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.File.FileName;
            var filePath = Path.Combine(studentFolderPath, uniqueFileName);


            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await model.File.CopyToAsync(fileStream);
            }


            var document = new Document
            {
                Title = model.Title,
                ContentType = model.File.ContentType,
                StudentId = model.StudentId,
                FilePath = filePath,

            };

            var createdDocument = await _documentRepository.CreateAsync(document);

            return createdDocument;
        }

        public async Task<List<Document>> GetDocumentsByStudentIdAsync(string studentId)
        {
            
            var documents = await _documentRepository.GetDocumentsByStudentIdAsync(studentId);

            return documents;
        }

        public async Task<Document> UpdateDocumentAsync(int documentId, UpdateDocument model)
        {
            
            var existingDocument = await _documentRepository.GetDocumentByIdAsync(documentId);

            if (existingDocument == null)
            {
                return null; 
            }

            
            existingDocument.Title = model.Title;
            existingDocument.ContentType = model.ContentType;
            

            
            await _documentRepository.SaveChangesAsync();

            return existingDocument; 
        }

        public async Task<Document> DeleteDocumentAsync(int documentId)
        {
            try
            {
                
                var documentToDelete = await _documentRepository.GetDocumentByIdAsync(documentId);

                if (documentToDelete == null)
                {
                    return null; 
                }

                _documentRepository.Delete(documentToDelete); 
                await _documentRepository.SaveChangesAsync();

                return documentToDelete; 
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public async Task<List<int>> DeleteMultipleDocumentsAsync(List<int> documentIds)
        {
            var deletedDocumentIds = new List<int>();

            foreach (var documentId in documentIds)
            {
                var deletedDocument = await DeleteDocumentAsync(documentId);
                if (deletedDocument != null)
                {
                    deletedDocumentIds.Add(deletedDocument.Id);
                }
            }

            return deletedDocumentIds;
        }

        public async Task<int> DeleteAllDocumentsForStudentAsync(string studentId)
        {
            try
            {
                var deletedCount = await _documentRepository.DeleteAllDocumentsForStudentAsync(studentId);
                await _documentRepository.SaveChangesAsync();
                return deletedCount;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }


    }
}


        
