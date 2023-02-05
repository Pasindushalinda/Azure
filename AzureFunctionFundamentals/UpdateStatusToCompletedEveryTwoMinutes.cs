using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureFunctionFundamentals.Data;
using AzureFunctionFundamentals.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzureFunctionFundamentals;

public class UpdateStatusToCompletedEveryTwoMinutes
{
    private readonly AzureFunctionDbContext _context;

    public UpdateStatusToCompletedEveryTwoMinutes(AzureFunctionDbContext context)
    {
        _context = context;
    }

    [FunctionName("UpdateStatusToCompletedEveryTwoMinutes")]
    public async Task RunAsync([TimerTrigger("0 */2 * * * *")] TimerInfo myTimer, ILogger log)
    {
        log.LogInformation($"C# Timer trigger function executed at: {DateTime.UtcNow}");

        IEnumerable<SalesRequest> salesRequestsFromDb =
            _context.SalesRequests.Where(x => x.Status == "Image Processed");

         foreach (var saleReq in salesRequestsFromDb)
         {
             saleReq.Status = "Completed";
         }

         _context.UpdateRange(salesRequestsFromDb);
         await _context.SaveChangesAsync();
    }
}