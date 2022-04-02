using Migration.Enums;
using Migration.Models;
using Netcorext.Algorithms;
using Newtonsoft.Json;

namespace Migration.Helper;

public static class BannedMemberMessages
{
    public static IEnumerable<Configuration> TransferValue(Configuration configuration,CommonSetting setting)
    {
        var options = new List<BooleanOption>()
        {
            new () {
                Key = "postContent",
                Value = "1,3,5,7".Contains(setting.svalue!),
                Text = "文章內容"
            },
            new () {
                Key = "memberPicture",
                Value = "2,3,6,7".Contains(setting.svalue!),
                Text = "會員頭像"
            },
            new () {
                Key = "memberSign",
                Value = "4,5,6,7".Contains(setting.svalue!),
                Text = "會員簽名"
            }
        };

        var rowColumnConfigurations = new List<Configuration>();

        for (var i = 0; i < options.Count; i++)
        {
            var id = Snowflake.Instance.Generate();
            var option = options[i];

            var optionConfiguration = new Configuration
            {
                Id = id,
                Key = $"{configuration.Key}_{i + 1}",
                Group = configuration.Group,
                ParentId = configuration.Id,
                Hierarchy = $"{configuration.Id}/{id}",
                Level = 2,
                SortingIndex = i + 1
            };
            rowColumnConfigurations.Add(optionConfiguration);

            var value = JsonConvert.SerializeObject(option);
            var columnConfigurations = ConfigurationColumnHelper.GetColumnConfigurations(optionConfiguration, value);
            rowColumnConfigurations.AddRange(columnConfigurations);
        }

        return rowColumnConfigurations;
    }
}