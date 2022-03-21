using Dapper;
using Migration.Models;
using MySql.Data.MySqlClient;
using Netcorext.Algorithms;
using Newtonsoft.Json;

namespace Migration.Mappers;

public class ForumFaqMapper: IMapper
{
    private readonly string _group;
    
    public ForumFaqMapper(string @group)
    {
        _group = @group;
    }

    public List<Configuration> GetConfigurations(MySqlConnection mySqlConnection)
    {
        var configurationList = new List<Configuration>();
        var forumFaqList = mySqlConnection.Query<ForumFaq>
            ("SELECT * FROM pre_forum_faq").ToList();;

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

        var parentFaqList =
            forumFaqList.Where(x => x.Fpid == 0)
            .OrderBy(x=>x.Id).ToList();
        
        var idMappedDic = new Dictionary<int,(Configuration Configuration, int ChildCount)>();

        for (var i = 0; i < parentFaqList.Count; i++)
        {
            var parentId = Snowflake.Instance.Generate();
            var parentFaq = parentFaqList[i];

            configuration = new Configuration
            {
                Id = parentId,
                Key = $"{_group}_{i + 1}",
                Group = _group,
                ParentId = rootId,
                Value = JsonConvert.SerializeObject(parentFaq),
                Hierarchy = $"{rootId}/{parentId}",
                Level = 2,
                SortingIndex = parentFaq.DisplayOrder
            };
            configurationList.Add(configuration);
            idMappedDic.Add(parentFaq.Id, (configuration,0));
        }

        for (var i = 0; i < forumFaqList.Count; i++)
        {
            var forumFaq = forumFaqList[i];

            if (forumFaq.Fpid == 0)
                continue;

            var id = Snowflake.Instance.Generate();
            var (parentConfiguration, childCount) = idMappedDic[forumFaq.Fpid];
            var parentId = parentConfiguration.Id;

            configuration = new Configuration
            {
                Id = id,
                Key = $"{parentConfiguration.Key}_{++childCount}",
                Group = _group,
                ParentId = parentId,
                Value =  JsonConvert.SerializeObject(forumFaq),
                Hierarchy = $"{rootId}/{parentId}/{id}",
                Level = 3,
                SortingIndex = forumFaq.DisplayOrder
            };
            configurationList.Add(configuration);
            //更新數量
            idMappedDic[forumFaq.Fpid] = (parentConfiguration, childCount);
        }

        return configurationList;
    }
}