using Dapper;
using Migration.Models;
using MySql.Data.MySqlClient;

namespace Migration.Mappers;

public class BannedIpMapper : ListMapper<BannedIp>,IMapper
{
    private readonly string _group;

    public BannedIpMapper(string @group) : base(@group)
    {
        _group = @group;
    }

    public List<Configuration> GetConfigurations(MySqlConnection mySqlConnection)
    {
        //int unsigned轉long一樣會有問題,一律改成unsigned
        var querySql = @"SELECT *,
        CAST(dateline AS UNSIGNED) AS dateline,
        CAST(expiration AS UNSIGNED) AS expiration 
        FROM pre_common_banned";
        var settings = mySqlConnection.Query<BannedIp>(querySql);
        return GetMappedList(settings.ToList());
    }
}