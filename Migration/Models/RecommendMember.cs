using Newtonsoft.Json;

namespace Migration.Models;

public class RecommendMember
{
    [JsonProperty(PropertyName = "MemberId")]
    public int Uid { get; set; }
    [JsonProperty(PropertyName = "MemberName")]
    public string? UserName { get; set; }
    public string? Reason { get; set; }
    [JsonProperty(PropertyName = "OperatorId")]
    public int OpUid { get; set; }
    [JsonProperty(PropertyName = "OperatorName")]
    public string? OpUserName { get; set; }
    [JsonIgnore]
    public int DisplayOrder { get; set; }
    [JsonProperty(PropertyName = "CreateDate")]
    public int DateLine { get; set; }
}

// create table pre_home_specialuser
// (
//     uid          mediumint unsigned  default 0  not null,
//     username     varchar(60)                    null,
//     status       tinyint(1) unsigned default 0  not null,
//     defaultpoke  text                           null,
//     dateline     int(10)             default 0  not null,
//     reason       text                           not null,
//     opuid        mediumint unsigned  default 0  not null,
//     opusername   varchar(15)         default '' not null,
//     displayorder mediumint unsigned  default 0  not null
// );