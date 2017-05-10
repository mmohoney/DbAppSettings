using System;
using DbAppSettings.Model.DataAccess.Implementations;
using DbAppSettings.Model.Service.CacheManager.Arguments;
using DbAppSettings.Model.Service.Factory;
using DbAppSettings.Model.Service.Factory.Interfaces;
using DbAppSettings.Model.Service.Interfaces;
using DbAppSettings.Model.Service.SettingCacheProvider.Interfaces;

namespace DbAppSettings.Model.Service.CacheManager
{
    public class LazyLoadDbAppSettingManager
    {
        private readonly ISettingCacheProviderFactory _settingCacheProviderFactory;
        private LazyLoadManagerArguments _lazyLoadManagerArguments;

        internal LazyLoadDbAppSettingManager(ISettingCacheProviderFactory settingCacheProviderFactory)
        {
            _settingCacheProviderFactory = settingCacheProviderFactory;
        }

        internal ISettingCacheV2 SettingCacheInstance => SettingCacheV2.Instance;

        private void Create(LazyLoadManagerArguments lazyLoadManagerArguments)
        {
            _lazyLoadManagerArguments = lazyLoadManagerArguments;
            _lazyLoadManagerArguments.LazyLoadSettingDao = _lazyLoadManagerArguments.LazyLoadSettingDao ?? new DefaultLazyLoadSettingDao();
            _lazyLoadManagerArguments.SaveNewSettingDao = _lazyLoadManagerArguments.SaveNewSettingDao ?? new DefaultSaveNewSettingDao();

            if (SettingCacheInstance.SettingCacheProvider.IsInitalized)
                return;

            ISettingCacheProvider settingCacheProvider = _settingCacheProviderFactory.GetSettingCacheProvider(lazyLoadManagerArguments);

            SettingCacheInstance.InitializeCache(settingCacheProvider);
        }

        public static LazyLoadDbAppSettingManager CreateAndIntialize(LazyLoadManagerArguments lazyLoadManagerArguments)
        {
            if (lazyLoadManagerArguments == null)
                throw new NullReferenceException("lazyLoadManagerArguments cannot be null");

            LazyLoadDbAppSettingManager settingManager = new LazyLoadDbAppSettingManager(new SettingCacheProviderFactory());
            settingManager.Create(lazyLoadManagerArguments);

            return settingManager;
        }
    }
}
