namespace Migration.Models;
using Newtonsoft.Json;

public class Stamp
{
    [JsonIgnore]
    public int DisplayOrder { get; set; }
    [JsonIgnore]
    public string? Type { get; set; }
    public string? Code { get; set; }
    [JsonProperty(PropertyName = "FileName")]
    public string? Url { get; set; }
    public string? RelatedSmallImageUrl { get; set; }

    [JsonProperty(PropertyName = "RelatedOperate")]
    public int? TypeId { get; set; }

}