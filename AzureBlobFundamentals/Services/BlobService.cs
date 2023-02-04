using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using AzureFundamentals.Models;

namespace AzureFundamentals.Services;

public class BlobService : IBlobService
{
    private readonly BlobServiceClient _client;

    public BlobService(BlobServiceClient client)
    {
        _client = client;
    }

    public async Task<List<string>> GetAllBlobs(string container)
    {
        var blobContainerClient = _client.GetBlobContainerClient(container);
        var blobs = blobContainerClient.GetBlobsAsync();

        var blobString = new List<string>();

        await foreach (var item in blobs)
        {
            blobString.Add(item.Name);
        }

        return blobString;
    }

    public async Task<string> GetBob(string name, string container)
    {
        var blobContainerClient = _client.GetBlobContainerClient(container);
        var blob = blobContainerClient.GetBlobClient(name);
        return blob.Uri.AbsoluteUri;
    }

    public async Task<bool> UploadBlob(string name, IFormFile file, string container, FileUpload fileUpload)
    {
        var blobContainerClient = _client.GetBlobContainerClient(container);
        var blobClient = blobContainerClient.GetBlobClient(name);

        var httpHeader = new BlobHttpHeaders()
        {
            ContentType = file.ContentType
        };

        IDictionary<string, string> metadata = new Dictionary<string, string>();

        metadata.Add("title", fileUpload.Title);
        metadata.Add("comment", fileUpload.Comment);

        var result = await blobClient
            .UploadAsync(file.OpenReadStream(), httpHeader, metadata);

        if (result != null)
        {
            return true;
        }

        return false;
    }

    public async Task<bool> DeleteBlob(string name, string container)
    {
        var blobContainerClient = _client.GetBlobContainerClient(container);
        var blobClient = blobContainerClient.GetBlobClient(name);
        return await blobClient.DeleteIfExistsAsync();
    }

    public async Task<List<Blob>> GetAllBlobWithUri(string container)
    {
        var blobConatinerClient = _client.GetBlobContainerClient(container);
        var blobs = blobConatinerClient.GetBlobsAsync();
        var blobList = new List<Blob>();
        string sasContainerSignature = String.Empty;

        //genarate sas container level
        
        // if (blobConatinerClient.CanGenerateSasUri)
        // {
        //     BlobSasBuilder sasBuilder = new()
        //     {
        //         BlobContainerName = blobConatinerClient.Name,
        //         Resource = "b",
        //         ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(1)
        //     };
        //
        //     sasBuilder.SetPermissions(BlobSasPermissions.Read);
        //     sasContainerSignature = blobConatinerClient.GenerateSasUri(sasBuilder)
        //         .AbsoluteUri.Split('?')[1].ToString();
        // }

        await foreach (var item in blobs)
        {
            var blobClient = blobConatinerClient.GetBlobClient(item.Name);

            Blob blobIndividual = new()
            {
                Uri = blobClient.Uri.AbsoluteUri //+ "?" + sasContainerSignature
            };

            //genarate sas blob level

            // if (blobClient.CanGenerateSasUri)
            // {
            //     BlobSasBuilder sasBuilder = new()
            //     {
            //         BlobContainerName = blobClient.GetParentBlobContainerClient().Name,
            //         BlobName = blobClient.Name,
            //         Resource = "b",
            //         ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(1)
            //     };
            //     
            //     sasBuilder.SetPermissions(BlobSasPermissions.Read);
            //     blobIndividual.Uri = blobClient.GenerateSasUri(sasBuilder).AbsoluteUri;
            // }

            BlobProperties properties = await blobClient.GetPropertiesAsync();

            if (properties.Metadata.ContainsKey("title"))
            {
                blobIndividual.Title = properties.Metadata["title"];
            }

            if (properties.Metadata.ContainsKey("Comment"))
            {
                blobIndividual.Comment = properties.Metadata["comment"];
            }

            blobList.Add(blobIndividual);
        }

        return blobList;
    }
}