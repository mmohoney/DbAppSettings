using System;
using System.Collections.Generic;
using System.Linq;
using DbAppSettings.Model.DataTransfer;
using DbAppSettings.Model.Domain;
using DbAppSettings.Model.Service.CacheManager.Arguments;

namespace DbAppSettings.Model.Service.SettingCacheProvider
{
    /// <summary>
    /// Retrieve all implementation of the setting cache provider. Will intially get all settings and load them into memory.
    /// </summary>
    internal class RetrieveAllSettingCacheProvider : SettingCacheProviderBase
    {
        private readonly RetrieveAllManagerArguments _managerArguments;

        internal RetrieveAllSettingCacheProvider(RetrieveAllManagerArguments managerArguments)
        {
            _managerArguments = managerArguments;
        }

        protected override CacheManagerArguments ManagerArguments => _managerArguments;

        protected override void InitializeSettingCacheProvider()
        {
            try
            {
                //Get all settings from the data access layer
                List<DbAppSettingDto> settingDtos = _managerArguments.RetrieveAllSettingDao.GetAllDbAppSettings().ToList();
                if (!settingDtos.Any())
                    return;

                SetSettingValues(settingDtos);

                //Store the latest changed timestamp
                LastRefreshedTime = settingDtos.Max(d => d.ModifiedDate);
            }
            catch (Exception e)
            {
                //TODO: Log manager
                //cacheManager.NotifyOfException(e);
            }
        }

        protected override void SettingWatchTaskAction()
        {
            try
            {
                //Return all settings that have changed since the last time a setting was refreshed
                List<DbAppSettingDto> settingDtos = _managerArguments.RetrieveAllSettingDao.GetChangedDbAppSettings(LastRefreshedTime).ToList();
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
            if (!IsInitalized)
            {
                lock (Lock)
                {
                    if (!IsInitalized)
                        throw new Exception("SettingCache uninitialized. Initalize by invoking DbAppSettingCacheManager.InitalizeSettingCacheProvider.");
                }
            }

            T newSetting = new T();

            HydrateSettingFromDto(newSetting);

            return newSetting;
        }
    }
}
