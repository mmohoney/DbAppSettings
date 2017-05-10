using System;
using System.Collections.Generic;
using DbAppSettings.Model.DataAccess.Interfaces;
using DbAppSettings.Model.Service.Interfaces;

namespace DbAppSettings.Test.Mock
{
    internal class DummySettingInitialization : ISettingInitialization
    {
        internal DummySettingInitialization(IDbAppSettingDao appSettingDao, TimeSpan cacheRefreshTimeoutMs, List<string> applications)
        {
            DbAppSettingDao = appSettingDao;
            Applications = applications;
            CacheRefreshTimeout = cacheRefreshTimeoutMs;
        }

        internal DummySettingInitialization(IDbAppSettingDao appSettingDao, TimeSpan cacheRefreshTimeoutMs, string application)
            :this (appSettingDao, cacheRefreshTimeoutMs, new List<string> { application })
        {

        }

        internal DummySettingInitialization(IDbAppSettingDao appSettingDao, TimeSpan cacheRefreshTimeoutMs)
            : this(appSettingDao, cacheRefreshTimeoutMs, new List<string>())
        {

        }

        public IDbAppSettingDao DbAppSettingDao { get; }
        public IDbAppSettingSaveNewSettingDao DbAppSettingSaveNewSettingDao { get; }
        public IDbAppSettingLazyLoadDao DbAppSettingLazyLoadDao { get; }
        public List<string> Applications { get; }
        public TimeSpan CacheRefreshTimeout { get; }
    }
}
