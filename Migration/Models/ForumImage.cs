using Newtonsoft.Json;

namespace Migration.Models;

public class ForumImage
{
    [JsonIgnore]
    public int TypeId { get; set; }
    public bool Available { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
    [JsonIgnore]
    public int DisplayOrder { get; set; }
    public string? Directory { get; set; }
}