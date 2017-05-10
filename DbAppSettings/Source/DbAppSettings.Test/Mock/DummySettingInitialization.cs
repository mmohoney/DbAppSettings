using System;
using System.Collections.Generic;
using DbAppSettings.Model.DataAccess.Interfaces;
using DbAppSettings.Model.Service.Interfaces;

namespace DbAppSettings.Test.Mock
{
    internal class DummySettingInitialization : ISettingInitialization
    {
        internal DummySettingInitialization(IRetrieveAllSettingDao appSettingDao, TimeSpan cacheRefreshTimeoutMs, List<string> applications)
        {
            DbAppSettingDao = appSettingDao;
            Applications = applications;
            CacheRefreshTimeout = cacheRefreshTimeoutMs;
        }

        internal DummySettingInitialization(IRetrieveAllSettingDao appSettingDao, TimeSpan cacheRefreshTimeoutMs, string application)
            :this (appSettingDao, cacheRefreshTimeoutMs, new List<string> { application })
        {

        }

        internal DummySettingInitialization(IRetrieveAllSettingDao appSettingDao, TimeSpan cacheRefreshTimeoutMs)
            : this(appSettingDao, cacheRefreshTimeoutMs, new List<string>())
        {

        }

        public IRetrieveAllSettingDao DbAppSettingDao { get; }
        public ISaveNewSettingDao DbAppSettingSaveNewSettingDao { get; }
        public ILazyLoadSettingDao DbAppSettingLazyLoadDao { get; }
        public List<string> Applications { get; }
        public TimeSpan CacheRefreshTimeout { get; }
    }
}
