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
        IRetrieveAllSettingDao DbAppSettingDao { get; }
        ISaveNewSettingDao DbAppSettingSaveNewSettingDao { get; }
        ILazyLoadSettingDao DbAppSettingLazyLoadDao { get; }
        List<string> Applications { get; }
        TimeSpan CacheRefreshTimeout { get; }
    }
}
