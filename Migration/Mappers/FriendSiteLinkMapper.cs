using Dapper;
using Migration.Models;
using MySql.Data.MySqlClient;

namespace Migration.Mappers;

public class FriendSiteLinkMapper : ListMapper<FriendSiteLink>,IMapper
{
    private readonly string _group;

    public FriendSiteLinkMapper(string @group) : base(@group)
    {
        _group = @group;
    }

    public List<Configuration> GetConfigurations(MySqlConnection mySqlConnection)
    {
        var querySql = "SELECT * FROM pre_common_friendlink";
        var settings = mySqlConnection.Query<FriendSiteLink>(querySql);
        return GetMappedList(settings.ToList());
    }
}