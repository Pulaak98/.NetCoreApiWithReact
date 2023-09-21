namespace WebApi.Model
{
    public class UploadDocument
    {
        public IFormFile File { get; set; }
        public string Title { get; set; }
        public string StudentId { get; set; }
    }
}
