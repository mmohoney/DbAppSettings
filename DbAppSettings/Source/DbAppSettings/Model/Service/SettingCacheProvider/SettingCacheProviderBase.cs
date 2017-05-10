using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DbAppSettings.Model.DataTransfer;
using DbAppSettings.Model.Domain;
using DbAppSettings.Model.Service.CacheManager.Arguments;
using DbAppSettings.Model.Service.SettingCacheProvider.Interfaces;

namespace DbAppSettings.Model.Service.SettingCacheProvider
{
    internal abstract class SettingCacheProviderBase : ISettingCacheProvider
    {
        protected static readonly object Lock = new object();
        protected static readonly ConcurrentDictionary<string, DbAppSettingDto> SettingDtosByKey = new ConcurrentDictionary<string, DbAppSettingDto>();
        protected static DateTime? LastRefreshedTime;


        protected abstract CacheManagerArguments ManagerArguments { get; }

        protected abstract void InitializeSettingCacheProvider();
        protected abstract void SettingWatchTaskAction();

        /// <summary>
        /// Returns whether or not the class is intialized
        /// </summary>
        internal static bool IsInitalized { get; set; }
        /// <summary>
        /// Reference to the watch task
        /// </summary>
        internal static Task SettingWatchTask { get; set; }
        /// <summary>
        /// Returns the count of the dtos in its internal cache
        /// </summary>
        internal static int SettingDtosByKeyCount => SettingDtosByKey.Count;

        bool ISettingCacheProvider.IsInitalized => IsInitalized;

        public void InitalizeSettingCacheProvider()
        {
            if (!IsInitalized)
            {
                lock (Lock)
                {
                    if (!IsInitalized)
                    {
                        //Call each implementation
                        InitializeSettingCacheProvider();

                        //If we were unable to get a refresh time, provide now as a fallback
                        if (!LastRefreshedTime.HasValue)
                            LastRefreshedTime = DateTime.Now;

                        //Invoke the watch thread to watch for changes in settings
                        InitalizeSettingWatchTask();

                        //Set the cache to intialized to allow settings to be fetched
                        IsInitalized = true;
                    }
                }
            }
        }

        public abstract DbAppSetting<T, TValueType> GetDbAppSetting<T, TValueType>() where T : DbAppSetting<T, TValueType>, new();

        /// <summary>
        /// Background thread to watch for settings that change in the data access layer
        /// </summary>
        protected void InitalizeSettingWatchTask()
        {
            //Task.Factory.StartNew pattern
            SettingWatchTask = Task.Factory.StartNew(() =>
            {
                //Run this indefinitly
                while (true)
                {
                    //Initially sleep for the refresh time since settings were just retrieved from the data access layer
                    Thread.Sleep(ManagerArguments.CacheRefreshTimeout());

                    try
                    {
                        SettingWatchTaskAction();
                    }
                    catch (Exception e)
                    {
                        //TODO: Log manager
                        //cacheManager.NotifyOfException(e);
                    }
                }
            });
        }

        /// <summary>
        /// Adds or updates settings in the internal dictionary of settings
        /// </summary>
        /// <param name="settingDtos"></param>
        protected void SetSettingValues(List<DbAppSettingDto> settingDtos)
        {
            //If any applications are provided, filter the results to only those settings
            if (ManagerArguments.Applications.Any())
                settingDtos = settingDtos.Where(d => ManagerArguments.Applications.Contains(d.ApplicationKey)).ToList();

            //Always store the dtos by key. This allows hydration without hitting the data access layer
            foreach (DbAppSettingDto settingDto in settingDtos)
                SettingDtosByKey.AddOrUpdate(settingDto.Key, settingDto, (key, oldValue) => settingDto);
        }

        /// <summary>
        /// Check the if the cache contains the value from the dictionary and return the up to date value if so
        /// </summary>
        /// <param name="dbAppSetting"></param>
        protected void HydrateSettingFromDto(InternalDbAppSettingBase dbAppSetting)
        {
            //If we have loaded the dto, hydrate it from the cache
            if (SettingDtosByKey.ContainsKey(dbAppSetting.FullSettingName))
            {
                DbAppSettingDto settingDto = SettingDtosByKey[dbAppSetting.FullSettingName];
                dbAppSetting.From(settingDto);
                return;
            }

            //If we have not yet loaded an assembly of a type, ignore this setting
            if (!SettingDtosByKey.ContainsKey(dbAppSetting.FullSettingName))
            {
                lock (Lock)
                {
                    if (!SettingDtosByKey.ContainsKey(dbAppSetting.FullSettingName))
                    {
                        try
                        {
                            //Convert to a dto to save
                            DbAppSettingDto newSettingDto = dbAppSetting.ToDto();

                            //If the setting is no found in the cache, save the setting
                            ManagerArguments.SaveNewSettingDao.SaveNewSettingIfNotExists(dbAppSetting.ToDto());

                            //If the setting was just added to the data access layer, add it to the dto cache as well
                            SettingDtosByKey.AddOrUpdate(newSettingDto.Key, newSettingDto, (key, oldValue) => newSettingDto);

                            //Hydrate it from itself as it is now present in the cache
                            dbAppSetting.From(newSettingDto);
                        }
                        catch (Exception e)
                        {
                            //TODO: Log manager
                            //cacheManager.NotifyOfException(e);
                        }
                    }
                }
            }
        }
    }
}
