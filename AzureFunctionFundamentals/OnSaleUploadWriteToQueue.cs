using System.IO;
using System.Threading.Tasks;
using AzureFunctionFundamentals.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureFunctionFundamentals;

public static class OnSaleUploadWriteToQueue
{
    [FunctionName("OnSaleUploadWriteToQueue")]
    public static async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
        HttpRequest req,
        [Queue("SalesRequestInBound", Connection = "AzureWebJobsStorage")]
        IAsyncCollector<SalesRequest> salesRequestQueue,
        ILogger log)
    {
        log.LogInformation("Sales request received by OnSaleUploadWriteToQueue function");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var data = JsonConvert.DeserializeObject<SalesRequest>(requestBody);

        await salesRequestQueue.AddAsync(data);
        string responseMeg = "Sales request has been recived for " + data.Name;

        return new OkObjectResult(responseMeg);
    }
}