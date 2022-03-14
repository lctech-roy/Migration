using MySql.Data.MySqlClient;

namespace Migration.Models;

public interface IMapper
{
     List<Configuration> GetConfigurations(MySqlConnection mySqlConnection);
}