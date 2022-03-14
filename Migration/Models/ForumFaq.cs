using Newtonsoft.Json;

namespace Migration.Models;

public class ForumFaq
{
    [JsonIgnore]
    public int Id { get; set; }
    [JsonIgnore]
    public int Fpid { get; set; }
    public int DisplayOrder { get; set; }
    public string? Identifier { get; set; }
    public string? Keyword { get; set; }
    public string? Title { get; set; }
    [JsonProperty(PropertyName = "Content")]
    public string? Message { get; set; }
}