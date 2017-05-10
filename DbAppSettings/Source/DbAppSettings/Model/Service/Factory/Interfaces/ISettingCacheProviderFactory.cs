using DbAppSettings.Model.Service.CacheManager.Arguments;
using DbAppSettings.Model.Service.SettingCacheProvider.Interfaces;

namespace DbAppSettings.Model.Service.Factory.Interfaces
{
    /// <summary>
    /// Factory to get the underlying cache provider
    /// </summary>
    internal interface ISettingCacheProviderFactory
    {
        ISettingCacheProvider GetSettingCacheProvider(CacheManagerArguments cacheManagerArguments);
    }
}
