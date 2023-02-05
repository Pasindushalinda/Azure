using AzureFundamentals.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AzureFundamentals.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SalesController : ControllerBase
{
    private readonly BlobServiceClient _blobServiceClient;
    private static readonly HttpClient _httpClient = new HttpClient();
    private static string functionAppUri = "http://localhost:7071/api/OnSaleUploadWriteToQueue";

    public SalesController(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    [HttpPost]
    public async Task<IActionResult> OnSaleUploadWriteToQueue([FromForm] SalesRequest salesRequest)
    {
        salesRequest.Id = Guid.NewGuid();

        if (salesRequest.Image == null)
        {
            return BadRequest();
        }
        else
        {
            using (var content = new StringContent(JsonConvert.SerializeObject(salesRequest),
                       Encoding.UTF8, "application/json"))
            {
                //Call to function app
                await _httpClient.PostAsync(functionAppUri, content);
                // return Ok(response.Content.ReadAsStreamAsync().Result);
            }
            
            var fileName = salesRequest.Id + Path.GetExtension(salesRequest.Image.FileName);
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient("fuctionalsalesrep");
            var blobClient = blobContainerClient.GetBlobClient(fileName);

            var httpHeaders = new BlobHttpHeaders()
            {
                ContentType = salesRequest.Image.FileName
            };

            await blobClient.UploadAsync(salesRequest.Image.OpenReadStream(), httpHeaders);
            return Ok("Image uploaded to the storege");
        }
    }
}