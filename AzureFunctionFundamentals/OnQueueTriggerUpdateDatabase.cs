using System.Threading.Tasks;
using AzureFunctionFundamentals.Data;
using AzureFunctionFundamentals.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AzureFunctionFundamentals;

public class OnQueueTriggerUpdateDatabase
{
    private readonly AzureFunctionDbContext _context;

    public OnQueueTriggerUpdateDatabase(AzureFunctionDbContext context)
    {
        _context = context;
    }

    [FunctionName("OnQueueTriggerUpdateDatabase")]
    public async Task RunAsync(
        [QueueTrigger("SalesrequestInBound", Connection = "AzureWebJobsStorage")]
        SalesRequest myQueueItem,
        ILogger log)
    {
        log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

        myQueueItem.Status = "Submitted";
        _context.SalesRequests.Add(myQueueItem);
        await _context.SaveChangesAsync();
    }
}