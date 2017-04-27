using System.Collections.Generic;
using DbAppSettings.Model.DataAccess.Interfaces;
using DbAppSettings.Model.Service.Interfaces;

namespace DbAppSettings.Test.Mock
{
    internal class DummySettingInitialization : ISettingInitialization
    {
        internal DummySettingInitialization(IDbAppSettingDao appSettingDao, int cacheRefreshTimeoutMs, List<string> applications)
        {
            DbAppSettingDao = appSettingDao;
            Applications = applications;
            CacheRefreshTimeoutMs = cacheRefreshTimeoutMs;
        }

        internal DummySettingInitialization(IDbAppSettingDao appSettingDao, int cacheRefreshTimeoutMs, string application)
            :this (appSettingDao, cacheRefreshTimeoutMs, new List<string> { application })
        {

        }

        internal DummySettingInitialization(IDbAppSettingDao appSettingDao, int cacheRefreshTimeoutMs)
            : this(appSettingDao, cacheRefreshTimeoutMs, new List<string>())
        {

        }

        public IDbAppSettingDao DbAppSettingDao { get; }
        public List<string> Applications { get; }
        public int CacheRefreshTimeoutMs { get; }
    }
}
