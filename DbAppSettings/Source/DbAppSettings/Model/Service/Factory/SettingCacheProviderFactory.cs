using System;
using DbAppSettings.Model.Service.Factory.Interfaces;
using DbAppSettings.Model.Service.Interfaces;
using DbAppSettings.Model.Service.SettingCacheProvider;
using DbAppSettings.Model.Service.SettingCacheProvider.Interfaces;

namespace DbAppSettings.Model.Service.Factory
{
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
