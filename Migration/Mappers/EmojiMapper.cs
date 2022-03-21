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
            Key = _group,
            Hierarchy = rootId.ToString(),
            Level = 1,
        };
        configurationList.Add(configuration);
        
        for (var i = 0; i < forumImageList.Count; i++)
        {
            var parentId = Snowflake.Instance.Generate();
            var parentKey = $"{_group}_{i + 1}";
            var forumImage = forumImageList[i];

            configuration = new Configuration
            {
                Id = parentId,
                Key = parentKey,
                Group = _group,
                ParentId = rootId,
                Value = JsonConvert.SerializeObject(forumImage),
                Hierarchy = $"{rootId}/{parentId}",
                Level = 2,
                SortingIndex = forumImage.DisplayOrder
            };
            configurationList.Add(configuration);
            
            var smileyList = mySqlConnection.Query<Smiley>
            //tinyint(1)的資料類型dapper轉換會出問題，所以select出來轉UNSIGNED
            (@"SELECT CAST(displayorder AS UNSIGNED) AS displayorder,Type,Code,Url 
            FROM pre_common_smiley WHERE typeid = @typeId AND type = 'smiley'"
                ,new {typeId =forumImageList[i].TypeId}).ToList();

            if (!smileyList?.Any() ?? true)
                continue;
            
            for (var j = 0; j < smileyList.Count; j++)
            {
                var id = Snowflake.Instance.Generate();
                var smiley = smileyList[j];

                configuration = new Configuration
                {
                    Id = id,
                    Key = $"{parentKey}_{j+1}",
                    Group = _group,
                    ParentId = parentId,
                    Value = JsonConvert.SerializeObject(smiley),
                    Hierarchy = $"{rootId}/{parentId}/{id}",
                    Level = 3,
                    SortingIndex = smiley.DisplayOrder
                };
                configurationList.Add(configuration);
            }
        }

        return configurationList;
    }
}