using System;
using DbAppSettings.Model.DataAccess.Implementations;
using DbAppSettings.Model.Service.CacheManager.Arguments;
using DbAppSettings.Model.Service.Factory;
using DbAppSettings.Model.Service.Factory.Interfaces;
using DbAppSettings.Model.Service.Interfaces;
using DbAppSettings.Model.Service.SettingCacheProvider.Interfaces;

namespace DbAppSettings.Model.Service.CacheManager
{
    public class RetrieveAllDbAppSettingManager
    {
        private readonly ISettingCacheProviderFactory _settingCacheProviderFactory;
        private RetrieveAllManagerArguments _retrieveAllManagerArguments;

        internal RetrieveAllDbAppSettingManager(ISettingCacheProviderFactory settingCacheProviderFactory)
        {
            _settingCacheProviderFactory = settingCacheProviderFactory;
        }

        internal ISettingCacheV2 SettingCacheInstance => SettingCacheV2.Instance;

        private void Create(RetrieveAllManagerArguments retrieveAllManagerArguments)
        {
            _retrieveAllManagerArguments = retrieveAllManagerArguments;
            _retrieveAllManagerArguments.DbAppSettingDao = _retrieveAllManagerArguments.DbAppSettingDao ?? new DefaultRetrieveAllSettingDao();
            _retrieveAllManagerArguments.SaveNewSettingDao = _retrieveAllManagerArguments.SaveNewSettingDao ?? new DefaultSaveNewSettingDao();

            if (SettingCacheInstance.SettingCacheProvider.IsInitalized)
                return;

            ISettingCacheProvider settingCacheProvider = _settingCacheProviderFactory.GetSettingCacheProvider(retrieveAllManagerArguments);

            SettingCacheInstance.InitializeCache(settingCacheProvider);
        }

        public static RetrieveAllDbAppSettingManager CreateAndIntialize(RetrieveAllManagerArguments retrieveAllManagerArguments)
        {
            if (retrieveAllManagerArguments == null)
                throw new NullReferenceException("retrieveAllManagerArguments cannot be null");

            RetrieveAllDbAppSettingManager settingManager = new RetrieveAllDbAppSettingManager(new SettingCacheProviderFactory());
            settingManager.Create(retrieveAllManagerArguments);

            return settingManager;
        }
    }
}
