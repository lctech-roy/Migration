using Newtonsoft.Json;

namespace Migration.Models;

public class FriendSiteLink
{
    [JsonIgnore]
    public int DisplayOrder { get; set; }
    public string? Name  { get; set; }
    public string? Url  { get; set; }
    public string? Description  { get; set; }
    public string? Logo  { get; set; }
    public int Type  { get; set; }
}