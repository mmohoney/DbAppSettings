using System;
using DbAppSettings.Model.DataAccess;
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
        private readonly DbAppSettingManagerArguments _arguments;
        private readonly ISettingCache _settingCache;

        internal DbAppSettingCacheManager(DbAppSettingManagerArguments arguments)
        {
            _arguments = arguments;
            _arguments.AppSettingDao = _arguments.AppSettingDao ?? new DefaultDbAppSettingDao();
            _settingCache = SettingCache.Instance;
        }

        /// <summary>
        /// Internally used for testing
        /// </summary>
        /// <param name="dbAppSettingDao"></param>
        /// <param name="settingCache"></param>
        internal DbAppSettingCacheManager(IDbAppSettingDao dbAppSettingDao, ISettingCache settingCache)
        {
            _arguments = new DbAppSettingManagerArguments() { AppSettingDao = dbAppSettingDao ?? new DefaultDbAppSettingDao() };
            _settingCache = settingCache;
        }

        /// <summary>
        /// Static create method used to instantiate the manager. Arguments must be implemented.
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static DbAppSettingCacheManager CreateAndIntialize(DbAppSettingManagerArguments arguments)
        {
            if (arguments == null)
                throw new NullReferenceException("arguments cannot be null");
            
            return new DbAppSettingCacheManager(arguments).InitializeCache();
        }

        /// <summary>
        /// Returns whether or not the SettingCache is intialized
        /// </summary>
        public bool IsCacheInitalized { get; private set; }
        /// <summary>
        /// Implementation of the data access layer
        /// </summary>
        internal IDbAppSettingDao AppSettingDao => _arguments.AppSettingDao;
        
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
