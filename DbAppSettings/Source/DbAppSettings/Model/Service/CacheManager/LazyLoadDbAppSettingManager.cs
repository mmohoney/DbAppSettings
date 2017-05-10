using System;
using DbAppSettings.Model.DataAccess.Implementations;
using DbAppSettings.Model.DataAccess.Interfaces;
using DbAppSettings.Model.Service.CacheManager.Arguments;
using DbAppSettings.Model.Service.Factory;
using DbAppSettings.Model.Service.Factory.Interfaces;
using DbAppSettings.Model.Service.Interfaces;
using DbAppSettings.Model.Service.SettingCacheProvider.Interfaces;

namespace DbAppSettings.Model.Service.CacheManager
{
    /// <summary>
    /// Initalization class that needs to be implemented to wire up the setting caches with lazy load logic.
    /// </summary>
    public class LazyLoadDbAppSettingManager
    {
        private readonly ISettingCacheProviderFactory _settingCacheProviderFactory;
        private LazyLoadManagerArguments _lazyLoadManagerArguments;

        internal LazyLoadDbAppSettingManager(ISettingCacheProviderFactory settingCacheProviderFactory)
        {
            _settingCacheProviderFactory = settingCacheProviderFactory;
        }

        internal LazyLoadDbAppSettingManager()
            :this(new SettingCacheProviderFactory())
        {

        }

        internal ISettingCacheV2 SettingCacheInstance => SettingCacheV2.Instance;

        private LazyLoadDbAppSettingManager Create(LazyLoadManagerArguments lazyLoadManagerArguments)
        {
            _lazyLoadManagerArguments = lazyLoadManagerArguments;
            _lazyLoadManagerArguments.LazyLoadSettingDao = _lazyLoadManagerArguments.LazyLoadSettingDao ?? new DefaultLazyLoadSettingDao();
            _lazyLoadManagerArguments.SaveNewSettingDao = _lazyLoadManagerArguments.SaveNewSettingDao ?? new DefaultSaveNewSettingDao();

            if (SettingCacheInstance.SettingCacheProvider.IsInitalized)
                return this;

            ISettingCacheProvider settingCacheProvider = _settingCacheProviderFactory.GetSettingCacheProvider(lazyLoadManagerArguments);

            SettingCacheInstance.InitializeCache(settingCacheProvider);

            return this;
        }

        /// <summary>
        /// Basic way of intializing the underlying caches by providing just the data access layer needed to lazy load the settings.
        /// </summary>
        /// <param name="lazyLoadSettingDao">Implementation of the lazy load interface. Cannot be null.</param>
        /// <returns></returns>
        public static LazyLoadDbAppSettingManager CreateAndIntialize(ILazyLoadSettingDao lazyLoadSettingDao)
        {
            if (lazyLoadSettingDao == null)
                throw new NullReferenceException("lazyLoadSettingDao cannot be null");

            return new LazyLoadDbAppSettingManager().Create(new LazyLoadManagerArguments { LazyLoadSettingDao = lazyLoadSettingDao });
        }

        /// <summary>
        /// Provides a more advanced way of intializing the underlying caches by providing overrides and additional data access methods.
        /// </summary>
        /// <param name="lazyLoadManagerArguments">Arguments cannot be null.</param>
        /// <returns></returns>
        public static LazyLoadDbAppSettingManager CreateAndIntialize(LazyLoadManagerArguments lazyLoadManagerArguments)
        {
            if (lazyLoadManagerArguments == null)
                throw new NullReferenceException("lazyLoadManagerArguments cannot be null");

            return new LazyLoadDbAppSettingManager().Create(lazyLoadManagerArguments);
        }
    }
}
