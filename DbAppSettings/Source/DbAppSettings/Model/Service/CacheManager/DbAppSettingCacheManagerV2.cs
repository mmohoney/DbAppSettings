using System;
using DbAppSettings.Model.DataAccess.Implementations;
using DbAppSettings.Model.DataAccess.Interfaces;
using DbAppSettings.Model.Service.CacheManager.Arguments;
using DbAppSettings.Model.Service.Interfaces;

namespace DbAppSettings.Model.Service.CacheManager
{
    /// <summary>
    /// Wrapper class that needs to be implemented to wire up the SettingCache. Wraps all necessary methods to invoke
    /// and initalize the cache
    /// </summary>
    public class DbAppSettingCacheManagerV2
    {
        private readonly RetrieveAllManagerArguments _arguments;
        private readonly ISettingCache _settingCache;

        internal DbAppSettingCacheManagerV2(RetrieveAllManagerArguments arguments)
        {
            _arguments = arguments;
            _arguments.RetrieveAllSettingDao = _arguments.RetrieveAllSettingDao ?? new DefaultRetrieveAllSettingDao();
            _settingCache = SettingCache.Instance;
        }

        
        internal DbAppSettingCacheManagerV2(IRetrieveAllSettingDao dbAppSettingDao, ISettingCache settingCache)
        {
            _arguments = new RetrieveAllManagerArguments() { RetrieveAllSettingDao = dbAppSettingDao ?? new DefaultRetrieveAllSettingDao() };
            _settingCache = settingCache;
        }

        public static DbAppSettingCacheManagerV2 CreateAndIntialize(RetrieveAllManagerArguments arguments)
        {
            if (arguments == null)
                throw new NullReferenceException("arguments cannot be null");
            
            return new DbAppSettingCacheManagerV2(arguments).InitializeCache();
        }

        public static DbAppSettingCacheManagerV2 CreateAndIntialize(IRetrieveAllSettingDao dbAppSettingDao)
        {
            if (dbAppSettingDao == null)
                throw new NullReferenceException("dbAppSettingDao cannot be null");

            return new DbAppSettingCacheManagerV2(new RetrieveAllManagerArguments { RetrieveAllSettingDao = dbAppSettingDao }).InitializeCache();
        }

      
        public bool IsCacheInitalized { get; private set; }
      
        internal IRetrieveAllSettingDao AppSettingDao => _arguments.RetrieveAllSettingDao;
        
      
        internal DbAppSettingCacheManagerV2 InitializeCache()
        {
            if (IsCacheInitalized)
                return this;

            ISettingInitialization settingInitialization = new SettingInitialization(_arguments);
            _settingCache.InitializeCache(settingInitialization);

            IsCacheInitalized = true;

            return this;
        }
    }
}
