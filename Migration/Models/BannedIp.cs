using Newtonsoft.Json;

namespace Migration.Models;

public class BannedIp
{
    public int Ip1 { get; set; }
    public int Ip2 { get; set; }
    public int Ip3 { get; set; }
    public int Ip4 { get; set; }
    [JsonProperty(PropertyName = "Operator")]
    public string? Admin { get; set; }
    [JsonProperty(PropertyName = "StartTime")]
    public long DateLine { get; set; }
    [JsonProperty(PropertyName = "EndTime")]
    public long Expiration { get; set; }
}
