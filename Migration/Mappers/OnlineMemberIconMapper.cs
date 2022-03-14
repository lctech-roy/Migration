using Dapper;
using Migration.Models;
using MySql.Data.MySqlClient;

namespace Migration.Mappers;

public class OnlineMemberIconMapper : ListMapper<OnlineMemberIcon>,IMapper
{
    private readonly string _group;

    public OnlineMemberIconMapper(string @group) : base(@group)
    {
        _group = @group;
    }

    public List<Configuration> GetConfigurations(MySqlConnection mySqlConnection)
    {
        var querySql = "SELECT * FROM pre_forum_onlinelist";
        var settings = mySqlConnection.Query<OnlineMemberIcon>(querySql);
        return GetMappedList(settings.ToList());
    }
}