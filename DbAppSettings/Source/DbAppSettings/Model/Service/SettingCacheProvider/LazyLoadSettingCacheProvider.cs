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

        internal override List<DbAppSettingDto> GetChangedSettings()
        {
            //Return all settings that have changed since the last time a setting was refreshed
            return _managerArguments.LazyLoadSettingDao.GetChangedDbAppSettings(LastRefreshedTime).ToList();
        }

        public override DbAppSetting<T, TValueType> GetDbAppSetting<T, TValueType>()
        {
            IntializationCheck();

            T newSetting = new T();

            //Check if setting exists without locking
            DbAppSettingDto outDto;
            if (SettingDtosByKey.TryGetValue(newSetting.FullSettingName, out outDto))
            {
                newSetting.From(outDto);
                return newSetting;
            }

            lock (Lock)
            {
                //Check if setting exists with locking
                if (SettingDtosByKey.ContainsKey(newSetting.FullSettingName))
                {
                    DbAppSettingDto settingDto = SettingDtosByKey[newSetting.FullSettingName];
                    newSetting.From(settingDto);
                    return newSetting;
                }

                //If setting does not exist yet, go get it from the database
                DbAppSettingDto updatedSettingDto = _managerArguments.LazyLoadSettingDao.GetDbAppSetting(newSetting.ToDto());
                if (updatedSettingDto != null)
                    SetSettingValues(new List<DbAppSettingDto>{ updatedSettingDto });

                HydrateSettingFromDto(newSetting);

                return newSetting;
            }
        }
    }
}
