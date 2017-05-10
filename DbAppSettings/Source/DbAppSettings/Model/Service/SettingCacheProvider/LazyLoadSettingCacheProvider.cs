using System;
using System.Collections.Generic;
using System.Linq;
using DbAppSettings.Model.DataTransfer;
using DbAppSettings.Model.Domain;
using DbAppSettings.Model.Service.CacheManager.Arguments;

namespace DbAppSettings.Model.Service.SettingCacheProvider
{
    /// <summary>
    /// Lazy load implementation of the setting cache provider. Will only retrieve settings on demand.
    /// </summary>
    internal class LazyLoadSettingCacheProvider : SettingCacheProviderBase
    {
        private readonly LazyLoadManagerArguments _managerArguments;

        internal LazyLoadSettingCacheProvider(LazyLoadManagerArguments managerArguments)
        {
            _managerArguments = managerArguments;
        }

        internal override CacheManagerArguments ManagerArguments => _managerArguments;

        internal override void InitializeSettingCacheProvider()
        {
            //Nothing to intially do in the lazy implementation
            return;
        }

        internal override void SettingWatchTaskAction()
        {
            try
            {
                //Return all settings that have changed since the last time a setting was refreshed
                List<DbAppSettingDto> settingDtos = _managerArguments.LazyLoadSettingDao.GetChangedDbAppSettings(LastRefreshedTime).ToList();
                if (!settingDtos.Any())
                    return;

                //Update the settings
                SetSettingValues(settingDtos);

                //Store a reference to the latest changed time
                LastRefreshedTime = settingDtos.Max(d => d.ModifiedDate);
            }
            catch (Exception e)
            {
                //TODO: Log manager
                //cacheManager.NotifyOfException(e);
            }
        }

        public override DbAppSetting<T, TValueType> GetDbAppSetting<T, TValueType>()
        {
            if (!Initalized)
            {
                lock (Lock)
                {
                    if (!Initalized)
                        throw new Exception("SettingCache uninitialized. Initalize by invoking DbAppSettingCacheManager.InitalizeSettingCacheProvider.");
                }
            }

            return null;
        }
    }
}
