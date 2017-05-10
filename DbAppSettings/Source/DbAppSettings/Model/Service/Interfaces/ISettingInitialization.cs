using System;
using System.Collections.Generic;
using DbAppSettings.Model.DataAccess.Interfaces;

namespace DbAppSettings.Model.Service.Interfaces
{
    /// <summary>
    /// Internal representation of DbAppSettingManagerArguments for use within the cache
    /// </summary>
    internal interface ISettingInitialization
    {
        IDbAppSettingDao DbAppSettingDao { get; }
        IDbAppSettingSaveNewSettingDao DbAppSettingSaveNewSettingDao { get; }
        IDbAppSettingLazyLoadDao DbAppSettingLazyLoadDao { get; }
        List<string> Applications { get; }
        TimeSpan CacheRefreshTimeout { get; }
    }
}
