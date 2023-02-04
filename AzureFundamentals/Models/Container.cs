using System.ComponentModel.DataAnnotations;

namespace AzureFundamentals.Models;

public class Container
{
    [Required] public string Name { get; set; }
}