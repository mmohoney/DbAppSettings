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
    /// <summary>
    /// Cache Provider Base
    /// </summary>
    internal abstract class SettingCacheProviderBase : ISettingCacheProvider
    {
        internal static readonly object Lock = new object();
        internal static readonly ConcurrentDictionary<string, DbAppSettingDto> SettingDtosByKey = new ConcurrentDictionary<string, DbAppSettingDto>();
        internal static DateTime? LastRefreshedTime;

        internal abstract CacheManagerArguments ManagerArguments { get; }

        internal abstract void InitializeSettingCacheProvider();
        internal abstract List<DbAppSettingDto> GetChangedSettings();
        public abstract DbAppSetting<T, TValueType> GetDbAppSetting<T, TValueType>() where T : DbAppSetting<T, TValueType>, new();

        internal static bool Initalized { get; set; }
        internal static CancellationTokenSource CancellationTokenSource { get; set; }
        internal static Task SettingWatchTask { get; set; }
        internal static int SettingDtosByKeyCount => SettingDtosByKey.Count;

        public bool IsInitalized => Initalized;

        /// <summary>
        /// Intialize the setting cache provider
        /// </summary>
        public void InitalizeSettingCacheProvider()
        {
            if (!Initalized)
            {
                lock (Lock)
                {
                    if (!Initalized)
                    {
                        //Call each implementation
                        InitializeSettingCacheProvider();

                        //If we were unable to get a refresh time, provide now as a fallback
                        LastRefreshedTime = LastRefreshedTime ?? DateTime.Now;

                        //Invoke the watch thread to watch for changes in settings
                        InitalizeSettingWatchTask();

                        //Set the cache to intialized to allow settings to be fetched
                        Initalized = true;
                    }
                }
            }
        }

        /// <summary>
        /// Background thread to watch for settings that change in the data access layer
        /// </summary>
        internal virtual void InitalizeSettingWatchTask()
        {
            lock (Lock)
            {
                //Create the cancel token
                CancellationTokenSource = new CancellationTokenSource();

                //Task.Factory.StartNew pattern
                SettingWatchTask = Task.Factory.StartNew(() =>
                {
                    //Run this indefinitly
                    while (true)
                    {
                        //If we have been requested to cancel, stop the infinite loop
                        if (CancellationTokenSource.Token.IsCancellationRequested)
                            break;

                        //Initially sleep for the refresh time since settings were just retrieved from the data access layer
                        Thread.Sleep(ManagerArguments.CacheRefreshTimeout());

                        try
                        {
                            //Return all settings that have changed since the last time a setting was refreshed
                            List<DbAppSettingDto> settingDtos = GetChangedSettings().ToList();
                            if (!settingDtos.Any())
                                return;

                            //Update the settings
                            SetSettingValues(settingDtos);

                            //Store a reference to the latest changed time
                            DateTime newMax = settingDtos.Max(d => d.ModifiedDate);
                            if (LastRefreshedTime < newMax)
                                LastRefreshedTime = newMax;

                            //If we were unable to get a refresh time, provide now as a fallback
                            LastRefreshedTime = LastRefreshedTime > DateTime.MinValue
                                ? LastRefreshedTime
                                : DateTime.Now;
                        }
                        catch (Exception e)
                        {
                            //TODO: Log manager
                            //cacheManager.NotifyOfException(e);
                        }
                    }
                }, CancellationTokenSource.Token);
            }
        }

        /// <summary>
        /// Internal use to cancel the setting watch task
        /// </summary>
        internal static void CancelTask()
        {
            if (CancellationTokenSource != null)
                CancellationTokenSource.Cancel();
        }

        /// <summary>
        /// Adds or updates settings in the internal dictionary of settings
        /// </summary>
        /// <param name="settingDtos"></param>
        internal void SetSettingValues(List<DbAppSettingDto> settingDtos)
        {
            //Always store the dtos by key. This allows hydration without hitting the data access layer
            foreach (DbAppSettingDto settingDto in settingDtos)
                SettingDtosByKey.AddOrUpdate(settingDto.Key, settingDto, (key, oldValue) => settingDto);
        }

        /// <summary>
        /// Check the if the cache contains the value from the dictionary and return the up to date value if so
        /// </summary>
        /// <param name="dbAppSetting"></param>
        internal void HydrateSettingFromDto(InternalDbAppSettingBase dbAppSetting)
        {
            //Check if setting exists without locking
            DbAppSettingDto outDto;
            if (SettingDtosByKey.TryGetValue(dbAppSetting.FullSettingName, out outDto))
            {
                dbAppSetting.From(outDto);
                return;
            }

            //Check if setting exists with locking
            lock (Lock)
            {
                if (SettingDtosByKey.ContainsKey(dbAppSetting.FullSettingName))
                {
                    DbAppSettingDto settingDto = SettingDtosByKey[dbAppSetting.FullSettingName];
                    dbAppSetting.From(settingDto);
                    return;
                }

                //If the setting was not found in the cache, we need to save it to the data access layer
                SaveNewSettingIfNotExists(dbAppSetting);
            }
        }

        /// <summary>
        /// Save the settign to the data access layer
        /// </summary>
        /// <param name="dbAppSetting"></param>
        internal virtual void SaveNewSettingIfNotExists(InternalDbAppSettingBase dbAppSetting)
        {
            lock (Lock)
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
