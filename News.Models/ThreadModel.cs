using System.Text.Json.Serialization;

namespace News.Models;

public class ThreadModel
{
    [JsonPropertyName("site")] public string? Site { get; set; }

    [JsonPropertyName("site_full")] public string? SiteFull { get; set; }

    public string? Country { get; set; }

    [JsonPropertyName("main_image")] public string? MainImage { get; set; }

    [JsonPropertyName("domain_rank")] public int? DomainRank { get; set; }
}