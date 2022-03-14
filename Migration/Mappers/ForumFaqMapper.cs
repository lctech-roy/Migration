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
        var forumFaqs = mySqlConnection.Query<ForumFaq>("SELECT * FROM pre_forum_faq");

        var forumFaqsList = forumFaqs.ToList();
        var parentList =
            forumFaqsList.Where(x => x.Fpid == 0)
            .OrderBy(x=>x.Id).ToList();
        
        var idMappedDic = new Dictionary<int, long>();
        parentList.ForEach(x =>  
            idMappedDic.Add(x.Id, Snowflake.Instance.Generate()));

        for (int i = 0; i < forumFaqsList.Count; i++)
        {
            var forumFaq = forumFaqsList[i];
            var configuration = new Configuration();
            
            configuration.Group = _group;
            configuration.Key = $"{_group}_{i + 1}";
            
            if (forumFaq.Fpid == 0)
                configuration.Id = idMappedDic[forumFaq.Id];
            else
            {
                configuration.ParentId =  idMappedDic[forumFaq.Fpid];
                configuration.Id = Snowflake.Instance.Generate();
            }
            
            configuration.Value =  JsonConvert.SerializeObject(forumFaq);
            
            configurationList.Add(configuration);
        }

        return configurationList;
    }
}