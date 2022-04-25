using Migration.Helper;
using Migration.Models;
using Netcorext.Algorithms;
using Newtonsoft.Json;

namespace Migration.Mappers;

public class ListMapper<T>
{
    private readonly string _group;
    private readonly string _key;

    protected ListMapper(string group)
    {
        _group = @group;
        _key=  @group;
    }
    
    protected ListMapper(string group,string key)
    {
        _group = @group;
        _key= @key;
    }

    protected List<Configuration> GetMappedList(List<T> oldSettingList)
    {
        List<Configuration> configurationList = new List<Configuration>();
        
        if (!oldSettingList?.Any() ?? true)
            return configurationList;
        
        var rootId = SnowflakeJavaScriptSafeInteger.Instance.Generate();
        var configuration = new Configuration
        {
            Id = rootId,
            Group = _group,
            Key = _key,
            Hierarchy = rootId.ToString(),
            Level = 1,
        };
        configurationList.Add(configuration);
        
        for (var i = 0; i < oldSettingList.Count; i++)
        {
            var id = SnowflakeJavaScriptSafeInteger.Instance.Generate();
            dynamic? oldSetting = oldSettingList[i];
            configuration = new Configuration
            {
                Id = id,
                Key = $"{_key}_{i + 1}",
                Group = _group,
                ParentId = rootId,
                Hierarchy = $"{rootId}/{id}",
                Level = 2,
                SortingIndex = oldSetting?.GetType().GetProperty("DisplayOrder") != null ?
                    oldSetting.DisplayOrder : 0
            };
            configurationList.Add(configuration);

            var columnConfigurations = ConfigurationColumnHelper.GetColumnConfigurations(configuration, JsonConvert.SerializeObject(oldSetting));
            configurationList.AddRange(columnConfigurations);
        }

        return configurationList;
    }
}