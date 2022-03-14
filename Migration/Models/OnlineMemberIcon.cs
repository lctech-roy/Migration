using Newtonsoft.Json;

namespace Migration.Models;

public class OnlineMemberIcon
{
    public int GroupId { get; set; }
    public int DisplayOrder { get; set; }
    public string? Title { get; set; }
    [JsonProperty(PropertyName = "FileName")]
    public string? Url { get; set; }
}
