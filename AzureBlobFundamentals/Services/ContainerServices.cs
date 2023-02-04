using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AzureFundamentals.Services;

public class ContainerServices : IContainerServices
{
    private readonly BlobServiceClient _client;

    public ContainerServices(BlobServiceClient client)
    {
        _client = client;
    }

    public async Task<List<string>> GetAllContainerAndBlobs()
    {
        List<string> conatainerAndBlobNames = new();
        conatainerAndBlobNames.Add("Account Name : " + _client.AccountName);
        conatainerAndBlobNames.Add("-------------------------------------");

        await foreach (var blobContainerItem in _client.GetBlobContainersAsync())
        {
            conatainerAndBlobNames.Add("--" + blobContainerItem.Name);

            var _blobContainer = _client.GetBlobContainerClient(blobContainerItem.Name);

            await foreach (var blobItem in _blobContainer.GetBlobsAsync())
            {
                //get metadata
                var blobClient = _blobContainer.GetBlobClient(blobItem.Name);
                BlobProperties blobProperties = await blobClient.GetPropertiesAsync();
                string blobToAdd = blobItem.Name;
                if (blobProperties.Metadata.ContainsKey("title"))
                {
                    blobToAdd += "(" + blobProperties.Metadata["title"] + ")";
                }

                conatainerAndBlobNames.Add("----" + blobToAdd);
            }

            conatainerAndBlobNames.Add("-------------------------------------");
        }

        return conatainerAndBlobNames;
    }

    public async Task<List<string>> GetAllContainers()
    {
        List<string> conatinerNames = new();

        await foreach (BlobContainerItem containerItem in _client.GetBlobContainersAsync())
        {
            conatinerNames.Add(containerItem.Name);
        }

        return conatinerNames;
    }

    public async Task CreateContainer(string containerName)
    {
        BlobContainerClient blobContainerClient = _client.GetBlobContainerClient(containerName);
        await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);
    }

    public async Task DeleteContainer(string containerName)
    {
        BlobContainerClient blobContainerClient = _client.GetBlobContainerClient(containerName);
        await blobContainerClient.DeleteIfExistsAsync();
    }
}