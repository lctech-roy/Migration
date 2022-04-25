using Migration.Models;
using Netcorext.Algorithms;
using Newtonsoft.Json;

namespace Migration.Helper;

public static class ConfigurationColumnHelper
{
    public static List<Configuration> GetColumnConfigurations(Configuration configuration,string rowJson)
    {
        var columnDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(rowJson);

        var configurationColumns = new List<Configuration>();

        foreach(var (key, value) in columnDic!)
        {
            var id = SnowflakeJavaScriptSafeInteger.Instance.Generate();
            var configurationColumn = new Configuration()
            {
                Id = id,
                Key = $"{configuration.Key}_{key}",
                Group = configuration.Group,
                ParentId = configuration.Id,
                Value =  value ?? string.Empty,
                Hierarchy = $"{configuration.Hierarchy}/{id}",
                Level = configuration.Level,
                SortingIndex = configuration.SortingIndex
            };
            // var spliterIndex = configuration.Key!.LastIndexOf('_');
            // configurationColumn.Key = configurationColumn.Key!.Insert(spliterIndex, key);
            configurationColumns.Add(configurationColumn);
        }

        return configurationColumns;
    }
}