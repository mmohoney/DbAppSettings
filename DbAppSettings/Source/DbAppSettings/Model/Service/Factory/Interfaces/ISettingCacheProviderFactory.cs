using DbAppSettings.Model.Service.CacheManager.Arguments;
using DbAppSettings.Model.Service.SettingCacheProvider.Interfaces;

namespace DbAppSettings.Model.Service.Factory.Interfaces
{
    internal interface ISettingCacheProviderFactory
    {
        ISettingCacheProvider GetSettingCacheProvider(CacheManagerArguments cacheManagerArguments);
    }
}
