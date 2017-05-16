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

        internal override CacheManagerArguments ManagerArguments => _managerArguments;

        internal override void InitializeSettingCacheProvider()
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

        internal override List<DbAppSettingDto> GetChangedSettings()
        {
            //Return all settings that have changed since the last time a setting was refreshed
            return _managerArguments.RetrieveAllSettingDao.GetChangedDbAppSettings(LastRefreshedTime).ToList();
        }

        public override DbAppSetting<T, TValueType> GetDbAppSetting<T, TValueType>()
        {
            //Throw exception if we have not initialized the cache
            if (!Initalized)
            {
                lock (Lock)
                {
                    if (!Initalized)
                        throw new Exception("SettingCache uninitialized. Initalize by invoking DbAppSettingCacheManager.InitalizeSettingCacheProvider.");
                }
            }

            T newSetting = new T();

            //Get the setting from the cache
            HydrateSettingFromDto(newSetting);

            return newSetting;
        }

        public override TValueType GetDbAppSettingValue<TValueType>(string fullSettingName)
        {
            if (string.IsNullOrWhiteSpace(fullSettingName))
                return default(TValueType);

            DbAppSettingDto placeHolderDto = new DbAppSettingDto
            {
                Key = fullSettingName,
                Type = typeof(TValueType).ToString(),
                Value = "test",
            };

            return default(TValueType);
        }
    }
}
