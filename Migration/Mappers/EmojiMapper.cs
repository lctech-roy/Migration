using Dapper;
using Migration.Models;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Netcorext.Algorithms;

namespace Migration.Mappers;

public class EmojiMapper : IMapper
{
    private readonly string _group;
    
    public EmojiMapper(string @group)
    {
        _group = @group;
    }
    
    public List<Configuration> GetConfigurations(MySqlConnection mySqlConnection)
    {
        var configurationList = new List<Configuration>();
        var forumImages = mySqlConnection.Query<ForumImage>("SELECT * FROM pre_forum_imagetype");

        var forumImageList = forumImages.ToList();
        if (!forumImageList?.Any() ?? true)
            return configurationList;
        
        var rootId = Snowflake.Instance.Generate();
        var configuration = new Configuration
        {
            Id = rootId,
            Group = _group,
            Key = _group
        };
        configurationList.Add(configuration);
        
        for (var i = 0; i < forumImageList.Count; i++)
        {
            var parentId = Snowflake.Instance.Generate();
            var parentKey = $"{_group}_{i + 1}";
            configuration = new Configuration
            {
                Id = parentId,
                Key = parentKey,
                Group = _group,
                ParentId = rootId,
                Value = JsonConvert.SerializeObject(forumImageList[i])
            };
            configurationList.Add(configuration);
            
            var smileies = mySqlConnection.Query<Smiley>
            //tinyint(1)的資料類型dapper轉換會出問題，所以select出來轉UNSIGNED
            ("SELECT CAST(displayorder AS UNSIGNED) AS displayorder,Type,Code,Url FROM pre_common_smiley WHERE typeid = @typeId"
                ,new {typeId =forumImageList[i].TypeId});

            var smileyList = smileies.ToList();
            if (!smileyList?.Any() ?? true)
                continue;
            
            for (var j = 0; j < smileyList.Count; j++)
            {
                configuration = new Configuration
                {
                    Id = Snowflake.Instance.Generate(),
                    Key = $"{parentKey}_{j+1}",
                    Group = _group,
                    ParentId = parentId,
                    Value = JsonConvert.SerializeObject(smileyList[j])
                };
                configurationList.Add(configuration);
            }
        }

        return configurationList;
    }
}