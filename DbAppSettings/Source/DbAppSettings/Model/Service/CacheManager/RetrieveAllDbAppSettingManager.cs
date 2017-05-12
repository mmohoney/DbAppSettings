using DbAppSettings.Model.DataAccess.Implementations;
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
    internal class RetrieveAllDbAppSettingManager
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
    }
}
