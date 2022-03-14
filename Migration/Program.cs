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

using var mySqlConnection =  new MySqlConnection(config.GetSection(mySqlConnectionStr).Value);

var configurationList = new List<Configuration>();
//全局
configurationList.AddRange( new CommonMapper("common").GetConfigurations(mySqlConnection));
//界面 » 表情管理
configurationList.AddRange( new EmojiMapper("emoji").GetConfigurations(mySqlConnection));
//界面 » 在線列表圖標
configurationList.AddRange( new OnlineMemberIconMapper("onlineMemberIcon").GetConfigurations(mySqlConnection));
//會員 » 禁止 IP
configurationList.AddRange( new BannedIpMapper("bannedIp").GetConfigurations(mySqlConnection));
//會員 » 推薦關注
configurationList.AddRange( new RecommendFollowMapper("recommendMember","follow").GetConfigurations(mySqlConnection));
//會員 » 推薦好友
configurationList.AddRange( new RecommendFriendMapper("recommendMember","friend").GetConfigurations(mySqlConnection));
//運營 » 站點幫助
configurationList.AddRange( new ForumFaqMapper("forumFaq").GetConfigurations(mySqlConnection));
//運營 » 友情連接
configurationList.AddRange( new FriendSiteLinkMapper("friendSiteLink").GetConfigurations(mySqlConnection));


using var pgSqlConnection = new NpgsqlConnection(config.GetSection(pgSqlConnectionStr).Value);

foreach (var configuration in configurationList)
{
    Console.WriteLine("key" + configuration.Key);
    pgSqlConnection.Execute(
        @"INSERT INTO ""Configure"" (""Id"",""Key"", ""Value"", ""Group"", ""ParentId"", ""CreationDate"")
        VALUES (@Id,@Key,@Value,@Group,@ParentId,@CreationDate);", configuration);
}


//Console.WriteLine("Hello, World!");