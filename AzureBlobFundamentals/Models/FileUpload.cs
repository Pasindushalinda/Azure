namespace AzureFundamentals.Models;

public class FileUpload
{
    public string Title { get; set; }
    public string Comment { get; set; }
    public IFormFile File { get; set; }
}