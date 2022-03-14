using Dapper;
using Migration.Enums;
using Migration.Helper;
using Migration.Models;
using MySql.Data.MySqlClient;
using Netcorext.Algorithms;

namespace Migration.Mappers;

public class CommonMapper : IMapper
{
    private static string? _group;
    
    public CommonMapper(string @group)
    {
        _group = @group;
    }
    
    public List<Configuration> GetConfigurations(MySqlConnection mySqlConnection)
    {
        var configurationList = new List<Configuration>();
        var commonSettings = mySqlConnection.Query<CommonSetting>
            ("SELECT * FROM pre_common_setting WHERE skey IN @keys",
            new { keys = CommonDictionary.Keys.ToArray() });
        
        foreach (var commonSetting in commonSettings)
        {
            if (string.IsNullOrEmpty(commonSetting.skey))
                continue;

            var mapper = CommonMapper.CommonDictionary[commonSetting.skey];
            var mappedConfigurationList = mapper.MapFunction!(commonSetting, mapper.Configuration!);
            configurationList.AddRange(mappedConfigurationList);
        }

        return configurationList;
    }
    
    public class Mapper
    {
        public Configuration? Configuration { get; set; }
        public Func<CommonSetting,Configuration,List<Configuration>>? MapFunction { get; set; }
    }
    
    private static readonly Func<CommonSetting, Configuration, List<Configuration>> SingleRow = (setting, configuration) =>
    {
        configuration.Id = Snowflake.Instance.Generate();
        //數字轉換boolean string
        if(configuration.Key?.IndexOf("is", StringComparison.Ordinal) == 0)
            setting.svalue = setting.svalue == "0" ? "false" : 
                setting.svalue == "1" ? "true" : setting.svalue;
        
        configuration.Value = setting.svalue;
        configuration.Group = _group;
        return new List<Configuration> {configuration};
    };
    
    private static readonly Func<CommonSetting, Configuration, List<Configuration>> MultipleCheckBox = (setting, configuration) =>
    {
        configuration.Id = Snowflake.Instance.Generate();
        configuration.Value = setting.svalue;
        configuration.Group = _group;
        switch (Enum.Parse<NewCommonKey>(configuration.Key!))
        {
            case NewCommonKey.bannedMemberMessages:
                BannedMemberMessages.TransferValue(setting,configuration);
                break;
        }
        return new List<Configuration> {configuration};
    };

    private static readonly Dictionary<string, Mapper> CommonDictionary = new Dictionary<string, Mapper>() 
        {
            //站點名稱
            {OldCommonKey.bbname.ToString(), new Mapper()
                {
                    Configuration = new Configuration() {Key = NewCommonKey.browserSiteName.ToString()},
                    MapFunction = SingleRow
                }
            },
            //網站名稱
            {OldCommonKey.sitename.ToString(), new Mapper()
                {
                    Configuration = new Configuration() {Key = NewCommonKey.siteName.ToString()},
                    MapFunction = SingleRow
                }
            },
            //網站URL
            {OldCommonKey.siteurl.ToString(), new Mapper()
                {
                    Configuration = new Configuration() {Key = NewCommonKey.siteUrl.ToString()},
                    MapFunction = SingleRow
                }
            },
            //管理員信箱
            {OldCommonKey.adminemail.ToString(), new Mapper()
                {
                    Configuration = new Configuration() {Key = NewCommonKey.adminEmail.ToString()},
                    MapFunction = SingleRow
                }
            },
            //關閉站點
            {OldCommonKey.bbclosed.ToString(), new Mapper()
                {
                    Configuration = new Configuration() {Key = NewCommonKey.isSiteClosed.ToString()},
                    MapFunction = SingleRow
                }
            }, 
            //刪帖不減積分時間期限(天)
            {OldCommonKey.losslessdel.ToString(), new Mapper()
                {
                    Configuration = new Configuration() {Key = NewCommonKey.postRemovedIntegrationKeepDay.ToString()},
                    MapFunction = SingleRow
                }
            },
            //管理操作理由選項
            {OldCommonKey.modreasons.ToString(), new Mapper()
                {
                    Configuration = new Configuration() {Key = NewCommonKey.rejectReasons.ToString()},
                    MapFunction = SingleRow
                }
            },
            //會員評分理由選項
            {OldCommonKey.userreasons.ToString(), new Mapper()
                {
                    Configuration = new Configuration() {Key =NewCommonKey.memberScoreReasons.ToString()},
                    MapFunction = SingleRow
                }
            },
            //隱藏敏感文章內容
            {OldCommonKey.bannedmessages.ToString(), new Mapper()
                {
                    Configuration = new Configuration() {Key = NewCommonKey.bannedMemberMessages.ToString()},
                    MapFunction = MultipleCheckBox
                }
            },
            //會員被警告多少次自動禁言
            {OldCommonKey.warninglimit.ToString(), new Mapper()
                {
                    Configuration = new Configuration() {Key = NewCommonKey.muteWarningTime.ToString()},
                    MapFunction = SingleRow
                }
            },
            //警告有效期(天)
            {OldCommonKey.warningexpiration.ToString(), new Mapper()
                {
                    Configuration = new Configuration() {Key = NewCommonKey.warningExpirationDay.ToString()},
                    MapFunction = SingleRow
                }
            },
            //熱門關鍵詞
            {OldCommonKey.srchhotkeywords.ToString(), new Mapper()
                {
                    Configuration = new Configuration() {Key = NewCommonKey.hotKeyWords.ToString()},
                    MapFunction = SingleRow
                }
            },
            
        };
}