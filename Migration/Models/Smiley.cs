namespace Migration.Models;
using Newtonsoft.Json;

public class Smiley
{
    [JsonIgnore]
    public int DisplayOrder { get; set; }
    public string? Type { get; set; }
    public string? Code { get; set; }
    [JsonProperty(PropertyName = "FileName")]
    public string? Url { get; set; }
}