using System;
using Microsoft.AspNetCore.Http;

namespace AzureFunctionFundamentals.Models;

public class SalesRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Status { get; set; }
    // public IFormFile Resume { get; set; }
}