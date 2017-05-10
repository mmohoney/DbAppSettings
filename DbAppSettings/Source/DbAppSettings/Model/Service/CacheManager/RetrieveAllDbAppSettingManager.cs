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
    /// Initalization class that needs to be implemented to wire up the setting caches with retrieve all logic.
    /// </summary>
    public class RetrieveAllDbAppSettingManager
    {
        private readonly ISettingCacheProviderFactory _settingCacheProviderFactory;
        private RetrieveAllManagerArguments _retrieveAllManagerArguments;

        internal RetrieveAllDbAppSettingManager(ISettingCacheProviderFactory settingCacheProviderFactory, ISettingCache settingCache)
        {
            _settingCacheProviderFactory = settingCacheProviderFactory;
            SettingCacheInstance = settingCache;
        }

        internal RetrieveAllDbAppSettingManager()
            :this(new SettingCacheProviderFactory(), SettingCache.Instance)
        {

        }

        internal ISettingCache SettingCacheInstance { get; }

        internal RetrieveAllDbAppSettingManager Create(RetrieveAllManagerArguments retrieveAllManagerArguments)
        {
            if (SettingCacheInstance.SettingCacheProvider?.IsInitalized ?? false)
                return this;

            _retrieveAllManagerArguments = retrieveAllManagerArguments;
            _retrieveAllManagerArguments.RetrieveAllSettingDao = _retrieveAllManagerArguments.RetrieveAllSettingDao ?? new DefaultRetrieveAllSettingDao();
            _retrieveAllManagerArguments.SaveNewSettingDao = _retrieveAllManagerArguments.SaveNewSettingDao ?? new DefaultSaveNewSettingDao();

            ISettingCacheProvider settingCacheProvider = _settingCacheProviderFactory.GetSettingCacheProvider(retrieveAllManagerArguments);

            SettingCacheInstance.InitializeCache(settingCacheProvider);

            return this;
        }

        /// <summary>
        /// Basic way of intializing the underlying caches by providing just the data access layer needed to get all settings.
        /// </summary>
        /// <param name="retrieveAllSettingDao">Implementation of the get all interface. Cannot be null.</param>
        /// <returns></returns>
        public static RetrieveAllDbAppSettingManager CreateAndIntialize(IRetrieveAllSettingDao retrieveAllSettingDao)
        {
            if (retrieveAllSettingDao == null)
                throw new NullReferenceException("retrieveAllSettingDao cannot be null");

            return new RetrieveAllDbAppSettingManager().Create(new RetrieveAllManagerArguments { RetrieveAllSettingDao = retrieveAllSettingDao });
        }

        /// <summary>
        /// Provides a more advanced way of intializing the underlying caches by providing overrides and additional data access methods.
        /// </summary>
        /// <param name="retrieveAllManagerArguments">Arguments cannot be null.</param>
        /// <returns></returns>
        public static RetrieveAllDbAppSettingManager CreateAndIntialize(RetrieveAllManagerArguments retrieveAllManagerArguments)
        {
            if (retrieveAllManagerArguments == null)
                throw new NullReferenceException("retrieveAllManagerArguments cannot be null");

            return new RetrieveAllDbAppSettingManager().Create(retrieveAllManagerArguments);
        }
    }
}
