using System;
using System.Collections.Generic;
using DbAppSettings.Model.DataAccess.Interfaces;
using DbAppSettings.Model.Service.Interfaces;

namespace DbAppSettings.Model.Service
{
    /// <summary>
    /// Internal representation of DbAppSettingManagerArguments for use within the cache
    /// </summary>
    internal class SettingInitialization : ISettingInitialization
    {
        internal SettingInitialization(RetrieveAllManagerArguments arguments)
        {
            DbAppSettingDao = arguments.DbAppSettingDao;
            DbAppSettingSaveNewSettingDao = arguments.DbAppSettingSaveNewSettingDao;
            Applications = arguments.Applications;
            CacheRefreshTimeout = arguments.CacheRefreshTimeout();
        }

        public IDbAppSettingDao DbAppSettingDao { get; set; }
        public IDbAppSettingSaveNewSettingDao DbAppSettingSaveNewSettingDao { get; set; }
        public IDbAppSettingLazyLoadDao DbAppSettingLazyLoadDao { get; }
        public List<string> Applications { get; set; } 
        public TimeSpan CacheRefreshTimeout { get; }
    }
}
