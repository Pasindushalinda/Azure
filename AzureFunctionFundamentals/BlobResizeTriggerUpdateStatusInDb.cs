using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AzureFunctionFundamentals.Data;
using AzureFunctionFundamentals.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AzureFunctionFundamentals;

public class BlobResizeTriggerUpdateStatusInDb
{
    private readonly AzureFunctionDbContext _context;

    public BlobResizeTriggerUpdateStatusInDb(AzureFunctionDbContext context)
    {
        _context = context;
    }

    [FunctionName("BlobResizeTriggerUpdateStatusInDb")]
    public async Task RunAsync(
        [BlobTrigger("fuctionalsalesrep-sm/{name}", Connection = "AzureWebJobsStorage")]
        Stream myBlob,
        string name, ILogger log)
    {
        log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

        var fileName = Path.GetFileNameWithoutExtension(name);

        SalesRequest sales = await _context.SalesRequests.FirstOrDefaultAsync(x => x.Id == fileName);

        if (sales is not null)
        {
            sales.Status = "Image Processed";
            _context.SalesRequests.Update(sales);
            await _context.SaveChangesAsync();
        }
    }
}