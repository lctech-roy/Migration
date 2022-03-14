using Migration.Enums;
using Migration.Models;
using Newtonsoft.Json;

namespace Migration.Helper;

public static class BannedMemberMessages
{
    public static void TransferValue(CommonSetting setting,Configuration configuration)
    {
        var options = new List<BooleanOption>();
        var option1 = new BooleanOption
        {
            Key = "postContent",
            Value = "1,3,5,7".Contains(setting.svalue!),
            Text = "文章內容"
        };
        var option2 = new BooleanOption
        {
            Key = "memberPicture",
            Value = "2,3,6,7".Contains(setting.svalue!),
            Text = "會員頭像"
        };
        var option3 = new BooleanOption
        {
            Key = "memberSign",
            Value = "4,5,6,7".Contains(setting.svalue!),
            Text = "會員簽名"
        };
        options.Add(option1);
        options.Add(option2);
        options.Add(option3);
        configuration.Value = JsonConvert.SerializeObject(options);
    }
}