using System;
using DbAppSettings.Model.Service.Interfaces;
using DbAppSettings.Model.Service.SettingCacheProvider;

namespace DbAppSettings.Model.Service.Factory
{
    internal class SettingCacheProviderFactory
    {
        ISettingCacheProvider GetSettingCacheProvider(CacheManagerArguments cacheManagerArguments)
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
