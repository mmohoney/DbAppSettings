using System;
using DbAppSettings.Model.DataAccess;
using DbAppSettings.Model.DataAccess.Implementations;
using DbAppSettings.Model.DataAccess.Interfaces;
using DbAppSettings.Model.Service;
using DbAppSettings.Model.Service.Interfaces;

namespace DbAppSettings
{
    /// <summary>
    /// Wrapper class that needs to be implemented to wire up the SettingCache. Wraps all necessary methods to invoke
    /// and initalize the cache
    /// </summary>
    public class DbAppSettingCacheManager
    {
        private readonly RetrieveAllManagerArguments _arguments;
        private readonly ISettingCache _settingCache;

        internal DbAppSettingCacheManager(RetrieveAllManagerArguments arguments)
        {
            _arguments = arguments;
            _arguments.DbAppSettingDao = _arguments.DbAppSettingDao ?? new DefaultRetrieveAllSettingDao();
            _settingCache = SettingCache.Instance;
        }

        /// <summary>
        /// Internally used for testing
        /// </summary>
        /// <param name="dbAppSettingDao"></param>
        /// <param name="settingCache"></param>
        internal DbAppSettingCacheManager(IRetrieveAllSettingDao dbAppSettingDao, ISettingCache settingCache)
        {
            _arguments = new RetrieveAllManagerArguments() { DbAppSettingDao = dbAppSettingDao ?? new DefaultRetrieveAllSettingDao() };
            _settingCache = settingCache;
        }

        /// <summary>
        /// Static create method used to instantiate the manager. Arguments must be implemented.
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static DbAppSettingCacheManager CreateAndIntialize(RetrieveAllManagerArguments arguments)
        {
            if (arguments == null)
                throw new NullReferenceException("arguments cannot be null");
            
            return new DbAppSettingCacheManager(arguments).InitializeCache();
        }

        /// <summary>
        /// Static create method used to instantiate the manager.
        /// </summary>
        /// <param name="dbAppSettingDao"></param>
        /// <returns></returns>
        public static DbAppSettingCacheManager CreateAndIntialize(IRetrieveAllSettingDao dbAppSettingDao)
        {
            if (dbAppSettingDao == null)
                throw new NullReferenceException("dbAppSettingDao cannot be null");

            return new DbAppSettingCacheManager(new RetrieveAllManagerArguments { DbAppSettingDao = dbAppSettingDao }).InitializeCache();
        }

        /// <summary>
        /// Returns whether or not the SettingCache is intialized
        /// </summary>
        public bool IsCacheInitalized { get; private set; }
        /// <summary>
        /// Implementation of the data access layer
        /// </summary>
        internal IRetrieveAllSettingDao AppSettingDao => _arguments.DbAppSettingDao;
        
        /// <summary>
        /// Method needs to be invoked in order to intialize cache
        /// </summary>
        /// <returns></returns>
        internal DbAppSettingCacheManager InitializeCache()
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
