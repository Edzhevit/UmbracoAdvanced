using System.ComponentModel.DataAnnotations;

namespace UmbracoAdvanced.Core.Models;

public record ProductUpdateItem
{
    public string? ProductName { get; set; }
    public decimal? Price { get; set; }
    public List<string>? Categories { get; set; }
    public string? Description { get; set; }
    public string? SKU { get; set; }
    public string? PhotoFileName { get; set; }
    public string? Photo { get; set; }
}