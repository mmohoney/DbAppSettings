using System;
using DbAppSettings.Model.Service.CacheManager.Arguments;
using DbAppSettings.Model.Service.Factory.Interfaces;
using DbAppSettings.Model.Service.SettingCacheProvider;
using DbAppSettings.Model.Service.SettingCacheProvider.Interfaces;

namespace DbAppSettings.Model.Service.Factory
{
    /// <summary>
    /// Factory to get the underlying cache provider
    /// </summary>
    internal class SettingCacheProviderFactory : ISettingCacheProviderFactory
    {
        public ISettingCacheProvider GetSettingCacheProvider(CacheManagerArguments cacheManagerArguments)
        {
            if (cacheManagerArguments is RetrieveAllManagerArguments)
            {
                return new RetrieveAllSettingCacheProvider((RetrieveAllManagerArguments) cacheManagerArguments);
            }

            if (cacheManagerArguments is LazyLoadManagerArguments)
            {
                return new LazyLoadSettingCacheProvider((LazyLoadManagerArguments)cacheManagerArguments);
            }

            throw new NotSupportedException("Unsupported cacheManagerArguments type");
        }
    }
}
