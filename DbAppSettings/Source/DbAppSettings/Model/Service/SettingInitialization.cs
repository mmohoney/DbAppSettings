using System.Collections.Generic;
using DbAppSettings.Model.DataAccess.Interfaces;
using DbAppSettings.Model.Service.Interfaces;

namespace DbAppSettings.Model.Service
{
    /// <summary>
    /// Represents a container of values for the SettingCache
    /// </summary>
    internal class SettingInitialization : ISettingInitialization
    {
        internal SettingInitialization(DbAppSettingManagerArguments arguments)
        {
            AppSettingDao = arguments.AppSettingDao;
            Applications = arguments.Applications;
            CacheRefreshTimeoutMs = arguments.CacheRefreshTimeoutMs();
        }

        public IDbAppSettingDao AppSettingDao { get; }
        public List<string> Applications { get; }
        public int CacheRefreshTimeoutMs { get; }
    }
}
