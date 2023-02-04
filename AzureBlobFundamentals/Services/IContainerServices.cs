namespace AzureFundamentals.Services;

public interface IContainerServices
{
    Task<List<string>> GetAllContainerAndBlobs();
    Task<List<string>> GetAllContainers();
    Task CreateContainer(string containerName);
    Task DeleteContainer(string containerName);
}