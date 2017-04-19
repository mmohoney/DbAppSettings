using System.Collections.Generic;
using DbAppSettings.Model.DataAccess.Interfaces;

namespace DbAppSettings.Model.Service.Interfaces
{
    /// <summary>
    /// Represents a container of values for the SettingCache
    /// </summary>
    internal interface ISettingInitialization
    {
        IDbAppSettingDao AppSettingDao { get; }
        List<string> Applications { get; }
        int CacheRefreshTimeoutMs { get; }
    }
}
