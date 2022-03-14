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
        
        var rootId = Snowflake.Instance.Generate();
        var configuration = new Configuration
        {
            Id = rootId,
            Group = _group,
            Key = _key
        };
        configurationList.Add(configuration);
        
        for (var i = 0; i < oldSettingList.Count; i++)
        {
            configuration = new Configuration
            {
                Id = Snowflake.Instance.Generate(),
                Key = $"{_key}_{i + 1}",
                Group = _group,
                ParentId = rootId,
                Value = JsonConvert.SerializeObject(oldSettingList[i])
            };
            configurationList.Add(configuration);
        }

        return configurationList;
    }
}