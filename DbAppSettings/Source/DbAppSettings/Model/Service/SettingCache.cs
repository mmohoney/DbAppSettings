using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DbAppSettings.Model.DataAccess.Interfaces;
using DbAppSettings.Model.DataTransfer;
using DbAppSettings.Model.Domain;
using DbAppSettings.Model.Service.Interfaces;

namespace DbAppSettings.Model.Service
{
    /// <summary>
    /// Represents a cache of DbAppSettings
    /// </summary>
    internal class SettingCache : ISettingCache
    {
        private static readonly object Lock = new object();
        private static readonly Dictionary<string, DbAppSettingDto> SettingDtosByKey = new Dictionary<string, DbAppSettingDto>();
        private static readonly ConcurrentDictionary<string, object> ImmutableSettingsByKey = new ConcurrentDictionary<string, object>();
        private static SettingCache _singleton;
        private static ISettingInitialization _settingInitialization;
        private static DateTime? _lastRefreshedTime;

        private SettingCache()
        {
            
        }

        public static SettingCache Instance
        {
            get
            {
                if (_singleton == null)
                {
                    lock (Lock)
                    {
                        if (_singleton == null)
                            _singleton = new SettingCache();
                    }
                }
                return _singleton;
            }
        }
        /// <summary>
        /// Returns whether or not the class is intialized
        /// </summary>
        internal static bool IsInitalized { get; private set; }
        /// <summary>
        /// Reference to the watch task
        /// </summary>
        internal static Task SettingWatchTask { get; private set; }
        /// <summary>
        /// Returns the count of the dtos in its internal cache
        /// </summary>
        internal static int SettingDtosByKeyCount => SettingDtosByKey.Count;

        /// <summary>
        /// Returns either a hydrated setting from the data access or the default setting. Runs in sync mode
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TValueType"></typeparam>
        /// <returns></returns>
        internal static DbAppSetting<T, TValueType> GetDbAppSetting<T, TValueType>() where T : DbAppSetting<T, TValueType>, new()
        {
            if(!IsInitalized)
            {
                lock (Lock)
                {
                    if (!IsInitalized)
                        throw new Exception("SettingCache uninitialized. Initalize by invoking DbAppSettingCacheManager.InitializeCache.");
                }
            }

            T newSetting = new T();

            //Case where we have the setting cached
            if (ImmutableSettingsByKey.ContainsKey(newSetting.FullSettingName))
                return ImmutableSettingsByKey[newSetting.FullSettingName] as T;

            HydrateSettingFromDto(newSetting);

            if (newSetting is ImmutableDbAppSetting<T, TValueType>)
            {
                ImmutableSettingsByKey.AddOrUpdate(newSetting.FullSettingName, newSetting, (key, oldValue) => newSetting);
            }
            
            return newSetting;
        }

        /// <summary>
        /// Must be invoked to intialize the setting cache. Runs in sync mode
        /// </summary>
        /// <param name="settingInitialization"></param>
        public void InitializeCache(ISettingInitialization settingInitialization)
        {
            if (!IsInitalized)
            {
                lock (Lock)
                {
                    if (!IsInitalized)
                    {
                        _settingInitialization = settingInitialization;

                        //Get all settings from the data access layer
                        try
                        {
                            List<DbAppSettingDto> settingDtos = _settingInitialization.DbAppSettingDao.GetAllDbAppSettings().ToList();
                            if (settingDtos.Any())
                            {
                                SetSettingValues(settingDtos);

                                //Store a reference to the latest changed time
                                _lastRefreshedTime = settingDtos.Max(d => d.ModifiedDate);
                            }
                        }
                        catch (Exception e)
                        {
                            //TODO: Log manager
                            //cacheManager.NotifyOfException(e);
                        }

                        //If we were unable to get a refresh time, provide now as a fallback
                        if (!_lastRefreshedTime.HasValue)
                            _lastRefreshedTime = DateTime.Now;
                        
                        //Invoke the watch thread to watch for changes in settings
                        InitalizeSettingWatchTask();

                        //Set the cache to intialized to allow settings to be fetched
                        IsInitalized = true;
                    }
                }
            }
        }

        /// <summary>
        /// Background thread to watch for settings that change in the data access layer
        /// </summary>
        private void InitalizeSettingWatchTask()
        {
            //Task.Factory.StartNew pattern
            SettingWatchTask = Task.Factory.StartNew(() =>
            {
                //Run this indefinitly
                while (true)
                {
                    //Initially sleep for the refresh time since settings were just retrieved from the data access layer
                    Thread.Sleep(_settingInitialization.CacheRefreshTimeout);

                    try
                    {
                        //Return all settings that have changed since the last time a setting was refreshed
                        List<DbAppSettingDto> settingDtos = _settingInitialization.DbAppSettingDao.GetChangedDbAppSettings(_lastRefreshedTime).ToList();
                        if (settingDtos.Any())
                        {
                            //Update the settings
                            SetSettingValues(settingDtos);

                            //Store a reference to the latest changed time
                            _lastRefreshedTime = settingDtos.Max(d => d.ModifiedDate);
                        }
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
        private void SetSettingValues(List<DbAppSettingDto> settingDtos)
        {
            //If any applications are provided, filter the results to only those settings
            if (_settingInitialization.Applications.Any())
                settingDtos = settingDtos.Where(d => _settingInitialization.Applications.Contains(d.ApplicationKey)).ToList();

            //Always store the dtos by key in case we do not have a loaded assembly when searching for a type. 
            //  This allows hydration without hitting the data access layer
            lock (Lock)
            {
                foreach (DbAppSettingDto settingDto in settingDtos)
                {
                    if (!SettingDtosByKey.ContainsKey(settingDto.Key))
                        SettingDtosByKey.Add(settingDto.Key, settingDto);
                    else
                        SettingDtosByKey[settingDto.Key] = settingDto;
                }
            }
        }

        /// <summary>
        /// Check the if the cache contains the value from the dictionary and return the up to date value if so
        /// </summary>
        /// <param name="dbAppSetting"></param>
        private static void HydrateSettingFromDto(InternalDbAppSettingBase dbAppSetting)
        {
            //If we have not yet loaded an assembly of a type, ignore this setting
            if (!SettingDtosByKey.ContainsKey(dbAppSetting.FullSettingName))
            {
                //If advanced implementation is given, use downcasting to access methods
                if (!(_settingInitialization.DbAppSettingDao is IDbAppSettingAdvancedDao))
                    return;

                DbAppSettingDto newSettingDto = dbAppSetting.ToDto();

                try
                {
                    //If the setting is no found in the cache, save the setting
                    ((IDbAppSettingAdvancedDao)_settingInitialization.DbAppSettingDao).SaveNewSettingIfNotExists(dbAppSetting.ToDto());

                    //If the setting was just added to the data access layer, add it to the dto cache as well
                    lock (Lock)
                    {
                        if (!SettingDtosByKey.ContainsKey(newSettingDto.Key))
                            SettingDtosByKey.Add(newSettingDto.Key, newSettingDto);
                    }
                }
                catch (Exception e)
                {
                    //TODO: Log manager
                    //cacheManager.NotifyOfException(e);
                }
            }

            DbAppSettingDto settingDto = SettingDtosByKey[dbAppSetting.FullSettingName];

            dbAppSetting.From(settingDto);
        }
    }
}
