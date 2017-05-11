using System;
using System.Collections.Generic;
using DbAppSettings.Model.DataAccess.Interfaces;
using DbAppSettings.Model.DataTransfer;

namespace DebugConsole
{
public class MyImplLazyLoadSettingDao : ILazyLoadSettingDao
{
    public static readonly DbAppSettingDto Setting = new DebugConsoleSettings.EnableLogging().ToDto();

    public MyImplLazyLoadSettingDao()
    {
        Setting.Value = true.ToString();
    }

    public DbAppSettingDto GetDbAppSetting(DbAppSettingDto dbAppSettingDto)
    {
        return Setting;
    }

    public IEnumerable<DbAppSettingDto> GetChangedDbAppSettings(DateTime? latestDbAppSettingChangedDate)
    {
        return new List<DbAppSettingDto>{ Setting };
    }
}
}
