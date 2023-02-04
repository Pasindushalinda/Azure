using AzureFundamentals.Models;

namespace AzureFundamentals.Services;

public interface IBlobService
{
    Task<List<string>> GetAllBlobs(string container);
    Task<string> GetBob(string name, string container);
    Task<bool> UploadBlob(string name, IFormFile file, string container,FileUpload fileUpload);
    Task<bool> DeleteBlob(string name, string container);
    Task<List<Blob>> GetAllBlobWithUri(string conatiner);
}