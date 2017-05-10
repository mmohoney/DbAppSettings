using DbAppSettings.Model.Service.Interfaces;
using DbAppSettings.Model.Service.SettingCacheProvider.Interfaces;

namespace DbAppSettings.Model.Service.Factory.Interfaces
{
    internal interface ISettingCacheProviderFactory
    {
        ISettingCacheProvider GetSettingCacheProvider(CacheManagerArguments cacheManagerArguments);
    }
}
