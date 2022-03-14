using Dapper;
using Migration.Models;
using MySql.Data.MySqlClient;

namespace Migration.Mappers;

public class RecommendFriendMapper: ListMapper<RecommendMember>,IMapper
{
    private readonly string _group;
    private readonly string _key;

    public RecommendFriendMapper(string @group,string @key) : base(@group,@key)
    {
        _group = @group;
        _key = @key;
    }

    public List<Configuration> GetConfigurations(MySqlConnection mySqlConnection)
    {
        var querySql = @"SELECT *,
        CAST(dateline AS UNSIGNED) AS dateline
        FROM pre_home_specialuser
        WHERE STATUS = 1";
        var settings = mySqlConnection.Query<RecommendMember>(querySql);
        return GetMappedList(settings.ToList());
    }
}