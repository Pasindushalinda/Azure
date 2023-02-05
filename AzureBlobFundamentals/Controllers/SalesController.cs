using AzureFundamentals.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace AzureFundamentals.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SalesController : ControllerBase
{
    private static readonly HttpClient _httpClient = new HttpClient();
    private static string functionAppUri = "http://localhost:7071/api/OnSaleUploadWriteToQueue";

    [HttpPost]
    public async Task<IActionResult> OnSaleUploadWriteToQueue([FromForm] SalesRequest salesRequest)
    {
        salesRequest.Id = Guid.NewGuid();

        using (var content = new StringContent(JsonConvert.SerializeObject(salesRequest),
                   Encoding.UTF8, "application/json"))
        {
            //Call to function app
            HttpResponseMessage response = await _httpClient.PostAsync(functionAppUri, content);
            return Ok(response.Content.ReadAsStreamAsync().Result);
        }
    }
}