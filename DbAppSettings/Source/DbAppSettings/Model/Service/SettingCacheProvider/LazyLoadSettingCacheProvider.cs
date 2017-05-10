using System;
using System.Collections.Generic;
using System.Linq;
using DbAppSettings.Model.DataTransfer;
using DbAppSettings.Model.Domain;

namespace DbAppSettings.Model.Service.SettingCacheProvider
{
    internal class LazyLoadSettingCacheProvider : SettingCacheProviderBase
    {
        private readonly LazyLoadManagerArguments _managerArguments;

        internal LazyLoadSettingCacheProvider(LazyLoadManagerArguments managerArguments)
        {
            _managerArguments = managerArguments;
        }

        protected override CacheManagerArguments ManagerArguments => _managerArguments;

        protected override void InitializeSettingCacheProvider()
        {
            //Run in sync mode
            lock (Lock)
            {
                return;
            }
        }

        protected override void SettingWatchTaskAction()
        {
            try
            {
                //Return all settings that have changed since the last time a setting was refreshed
                List<DbAppSettingDto> settingDtos = _managerArguments.DbAppSettingLazyLoadDao.GetChangedDbAppSettings(LastRefreshedTime).ToList();
                if (settingDtos.Any())
                {
                    //Update the settings
                    SetSettingValues(settingDtos);

                    //Store a reference to the latest changed time
                    LastRefreshedTime = settingDtos.Max(d => d.ModifiedDate);
                }
            }
            catch (Exception e)
            {
                //TODO: Log manager
                //cacheManager.NotifyOfException(e);
            }
        }

        public override DbAppSetting<T, TValueType> GetDbAppSetting<T, TValueType>()
        {
            throw new NotImplementedException();
        }
    }
}
