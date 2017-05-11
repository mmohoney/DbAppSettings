using DbAppSettings.Model.DataAccess.Implementations;
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
    internal class LazyLoadDbAppSettingManager
    {
        private readonly ISettingCacheProviderFactory _settingCacheProviderFactory;
        private LazyLoadManagerArguments _lazyLoadManagerArguments;

        internal LazyLoadDbAppSettingManager(ISettingCacheProviderFactory settingCacheProviderFactory, ISettingCache settingCache)
        {
            _settingCacheProviderFactory = settingCacheProviderFactory;
            SettingCacheInstance = settingCache;
        }

        internal LazyLoadDbAppSettingManager()
            :this(new SettingCacheProviderFactory(), SettingCache.Instance)
        {

        }

        internal ISettingCache SettingCacheInstance { get; }

        internal LazyLoadDbAppSettingManager Create(LazyLoadManagerArguments lazyLoadManagerArguments)
        {
            if (SettingCacheInstance.SettingCacheProvider?.IsInitalized ?? false)
                return this;

            _lazyLoadManagerArguments = lazyLoadManagerArguments;
            _lazyLoadManagerArguments.LazyLoadSettingDao = _lazyLoadManagerArguments.LazyLoadSettingDao ?? new DefaultLazyLoadSettingDao();
            _lazyLoadManagerArguments.SaveNewSettingDao = _lazyLoadManagerArguments.SaveNewSettingDao ?? new DefaultSaveNewSettingDao();

            ISettingCacheProvider settingCacheProvider = _settingCacheProviderFactory.GetSettingCacheProvider(lazyLoadManagerArguments);

            SettingCacheInstance.InitializeCache(settingCacheProvider);

            return this;
        }
    }
}
