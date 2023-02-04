using AzureFundamentals.Models;
using AzureFundamentals.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureFundamentals.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlobController : ControllerBase
{
    private readonly IBlobService _blobService;

    public BlobController(IBlobService blobService)
    {
        _blobService = blobService;
    }

    [HttpGet]
    public async Task<IActionResult> GetBlobs(string containerName)
    {
        return Ok(await _blobService.GetAllBlobs(containerName));
    }

    [HttpGet("GetBlob")]
    public async Task<IActionResult> GetBlob(string name, string container)
    {
        return Ok(await _blobService.GetBob(name, container));
    }

    [HttpPost]
    public async Task<IActionResult> UploadFile(string container, [FromForm] FileUpload blob)
    {
        if (blob.File == null || blob.File.Length < 1) return BadRequest();

        var fileName = Path.GetFileNameWithoutExtension(blob.File.FileName)
                       + "-" + Guid.NewGuid() + Path.GetExtension(blob.File.FileName);

        var result = await _blobService.UploadBlob(fileName, blob.File, container, blob);

        if (result) return Ok();

        return BadRequest();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteBlob(string name, string container)
    {
        if (await _blobService.DeleteBlob(name, container)) return Ok();
        return BadRequest();
    }

    [HttpGet("GetAllBlobWithUri")]
    public async Task<IActionResult> GetAllBlobWithUri(string container)
    {
        return Ok(await _blobService.GetAllBlobWithUri(container));
    }
}