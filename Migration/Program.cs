// See https://aka.ms/new-console-template for more information

using Dapper;
using MySql.Data.MySqlClient;
using Migration.Models;
using Microsoft.Extensions.Configuration;
using Migration.Mappers;
using Npgsql;

const string appSettingsFileName = "appSettings.json";
const string mySqlConnectionStr = "mySqlConnection";
const string pgSqlConnectionStr = "pgSqlConnection";

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile(appSettingsFileName)
    .Build();

using var mySqlConnection = new MySqlConnection(config.GetSection(mySqlConnectionStr).Value);

var mapperList = new List<IMapper>()
{
     //全局
     new CommonMapper("common"),
     //界面 » 表情管理
     new EmojiMapper("emoji"),
     //界面 » 主題鑒定
     new StampMapper("stamp"),
     //界面 » 在線列表圖標
     new OnlineMemberIconMapper("onlineMemberIcon"),
     //會員 » 禁止 IP
     //new BannedIpMapper("bannedIp"),
     //會員 » 推薦關注
     new RecommendFollowMapper("recommendMember", "follow"),
     //會員 » 推薦好友
     new RecommendFriendMapper("recommendMember", "friend"),
     //運營 » 站點幫助
     new ForumFaqMapper("forumFaq"),
     //運營 » 友情連接
     //new FriendSiteLinkMapper("friendSiteLink")
};

var configurationList = new List<Configuration>();

mapperList.ForEach(mapper =>
{
    configurationList.AddRange(mapper.GetConfigurations(mySqlConnection));
});

using var pgSqlConnection = new NpgsqlConnection(config.GetSection(pgSqlConnectionStr).Value);
pgSqlConnection.Open();
using var trans = pgSqlConnection.BeginTransaction();

foreach (var configuration in configurationList)
{
    Console.WriteLine(configuration.Key);
    pgSqlConnection.Execute(
        @"INSERT INTO ""Configuration"" (""Id"",""Key"", ""Value"", ""Group"",""Source"", ""ParentId""
                                        , ""CreationDate"", ""CreatorId"", ""ModificationDate"", ""ModifierId"",
                                        ""Hierarchy"",""Level"",""SortingIndex"",""Version"")
        VALUES (@Id,@Key,@Value,@Group,@Source,@ParentId,@CreationDate,@CreatorId,@ModificationDate,
                @ModifierId,@Hierarchy,@Level,@SortingIndex,@Version);", configuration);
}

trans.Commit();
Console.WriteLine("Migration Finish!");