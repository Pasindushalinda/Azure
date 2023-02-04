using AzureFundamentals.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureFundamentals.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContainerController : ControllerBase
{
    private readonly IContainerServices _containerServices;

    public ContainerController(IContainerServices containerServices)
    {
        _containerServices = containerServices;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllContainers()
    {
        var allContainers = await _containerServices.GetAllContainers();
        return Ok(allContainers);
    }

    [HttpPost]
    public async Task<IActionResult> CreateContainer(string name)
    {
        await _containerServices.CreateContainer(name);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteContainer(string naem)
    {
        await _containerServices.DeleteContainer(naem);
        return Ok();
    }

    [HttpGet("GetAllContainersAndBlobs")]
    public async Task<IActionResult> GetAllContainersAndBlobs()
    {
        return Ok(await _containerServices.GetAllContainerAndBlobs());
    }
}