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
            DbAppSettingDao = arguments.DbAppSettingDao;
            Applications = arguments.Applications;
            CacheRefreshTimeoutMs = arguments.CacheRefreshTimeoutMs();
        }

        /// <summary>
        /// Implementation of the data access layer
        /// </summary>
        public IDbAppSettingDao DbAppSettingDao { get; }
        /// <summary>
        /// Optional - If any applications are passed in. The SettingCache will only pull up to date
        /// values for the specified applications. Generally this will not be needed unless specific security is needed
        /// </summary>
        public List<string> Applications { get; }
        /// <summary>
        /// Static function that returns in milliseconds how long to wait before checking the data access layer for new values
        /// </summary>
        public int CacheRefreshTimeoutMs { get; }
    }
}
