using Dapper;
using Migration.Models;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Netcorext.Algorithms;

namespace Migration.Mappers;

public class StampMapper : IMapper
{
    private readonly string _group;

    public StampMapper(string @group)
    {
        _group = @group;
    }

    public List<Configuration> GetConfigurations(MySqlConnection mySqlConnection)
    {
        var configurationList = new List<Configuration>();

        var stampList = mySqlConnection.Query<Stamp>
            //tinyint(1)的資料類型dapper轉換會出問題，所以select出來轉UNSIGNED
            (@"SELECT *,CAST(displayorder AS UNSIGNED) AS displayorder
                    FROM pre_common_smiley WHERE type like '%stamp%'").ToList();

        if (!stampList?.Any() ?? true)
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
        
        const string stampListType = "stamplist";
        var smallStampList = stampList.Where(x => x.Type == stampListType).ToList();
        stampList.RemoveAll(x => x.Type == stampListType);

        for (var i = 0; i < stampList.Count; i++)
        {
            var id = Snowflake.Instance.Generate();
            var stamp = stampList[i];
            var stampFileName =  Path.GetFileNameWithoutExtension(stamp.Url);
            stamp.RelatedSmallImageUrl =
                smallStampList.FirstOrDefault(x => x.Url!.Contains(stampFileName!))?.Url;

            configuration = new Configuration
            {
                Id = id,
                Key = $"{_group}_{i+1}",
                Group = _group,
                ParentId = rootId,
                Value = JsonConvert.SerializeObject(stamp),
                Hierarchy = $"{rootId}/{id}",
                Level = 2,
                Index = stamp.DisplayOrder
            };
            configurationList.Add(configuration);
        }
        return configurationList;
    }
}