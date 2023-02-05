using System;
using System.IO;
using System.Threading.Tasks;
using AzureFunctionFundamentals.Data;
using AzureFunctionFundamentals.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureFunctionFundamentals.Grocery;

public class GroceryApi
{
    private readonly AzureFunctionDbContext _context;

    public GroceryApi(AzureFunctionDbContext context)
    {
        _context = context;
    }

    [FunctionName("CreateGrocery")]
    public async Task<IActionResult> CreateGrocery(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "GroceryList")]
        HttpRequest req, ILogger log)
    {
        log.LogInformation("Creating grocery list item");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var data = JsonConvert.DeserializeObject<GroceryItem>(requestBody);

        var groceryItem = new GroceryItem
        {
            Name = data.Name
        };

        _context.GroceryItems.Add(groceryItem);
        await _context.SaveChangesAsync();

        return new OkObjectResult(groceryItem);
    }

    [FunctionName("GetGroceryItems")]
    public async Task<IActionResult> GetGroceryItems(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GroceryList")]
        HttpRequest req)
    {
        return new OkObjectResult(await _context.GroceryItems.ToListAsync());
    }

    [FunctionName("GetGroceryItemById")]
    public async Task<IActionResult> GetGroceryItemById(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GroceryList/{id}")]
        HttpRequest req, string id)
    {
        var item = await _context.GroceryItems.FirstOrDefaultAsync(x => x.Id == id);

        if (item is null) return new NotFoundResult();

        return new OkObjectResult(item);
    }

    [FunctionName("UpdateGroceryItem")]
    public async Task<IActionResult> UpdateGroceryItem(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "GroceryList/{id}")]
        HttpRequest req, string id)
    {
        var item = await _context.GroceryItems.FirstOrDefaultAsync(x => x.Id == id);

        if (item is null) return new NotFoundResult();

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var data = JsonConvert.DeserializeObject<GroceryItem>(requestBody);

        item.Name = data.Name;

        _context.GroceryItems.Update(item);
        await _context.SaveChangesAsync();

        return new OkObjectResult(item);
    }

    [FunctionName("DeleteGroceryItem")]
    public async Task<IActionResult> DeleteGroceryItem(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "GroceryList/{id}")]
        HttpRequest req, string id)
    {
        var item = await _context.GroceryItems.FirstOrDefaultAsync(x => x.Id == id);

        if (item is null) return new NotFoundResult();

        _context.GroceryItems.Remove(item);
        await _context.SaveChangesAsync();

        return new ObjectResult("Item has been deleted");
    }
}