using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        /// <summary>
        /// Singleton pattern
        /// </summary>
        private static readonly SettingCache Singleton = new SettingCache();
        private static readonly object Lock = new object();
        private static readonly Dictionary<string, DbAppSettingDto> SettingDtosByKey = new Dictionary<string, DbAppSettingDto>();
        private static ISettingInitialization _settingInitialization;
        private static bool _isInitalized;
        private static DbAppSettingDictionary _dbAppSettingDictionary;
        private static DateTime? _lastRefreshedTime;
        private static Task _settingWatchTask;

        /// <summary>
        /// Singleton pattern
        /// </summary>
        private SettingCache()
        {
            
        }

        /// <summary>
        /// Singleton pattern
        /// </summary>
        public static SettingCache Instance => Singleton;
        /// <summary>
        /// Returns whether or not the class is intialized
        /// </summary>
        internal static bool IsInitalized => _isInitalized;
        /// <summary>
        /// Reference to the watch task
        /// </summary>
        internal static Task SettingWatchTask => _settingWatchTask;
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
            T newSetting = new T();

            lock (Lock)
            {
                if (!_isInitalized)
                    throw new Exception("SettingCache uninitialized. Initalize by invoking DbAppSettingCacheManager.InitializeCache.");

                if (_dbAppSettingDictionary.ContainsKey<T, TValueType>())
                    return _dbAppSettingDictionary.Get<T, TValueType>();

                //Attempt to hydrate the setting from the dto if we have not done so already
                HydrateSettingFromDto(newSetting);
            }
            
            return newSetting;
        }

        /// <summary>
        /// Must be invoked to intialize the setting cache. Runs in sync mode
        /// </summary>
        /// <param name="settingInitialization"></param>
        public void InitializeCache(ISettingInitialization settingInitialization)
        {
            if (!_isInitalized)
            {
                lock (Lock)
                {
                    if (!_isInitalized)
                    {
                        _settingInitialization = settingInitialization;

                        //Create the dictionary of settings
                        _dbAppSettingDictionary = DbAppSettingDictionary.Create();

                        //Get all settings from the data access layer
                        List<DbAppSettingDto> settingDtos = _settingInitialization.AppSettingDao.GetAllDbAppSettings().ToList();
                        if (settingDtos.Any())
                            SetSettingValues(settingDtos);

                        //Store a reference to the latest changed time
                        _lastRefreshedTime = settingDtos.Max(d => d.ModifiedDate);

                        //Invoke the watch thread to watch for changes in settings
                        InitalizeSettingWatchTask();

                        //Set the cache to intialized to allow settings to be fetched
                        _isInitalized = true;
                    }
                }
            }
        }

        /// <summary>
        /// Background thread to watch for settings that change in the data access layer
        /// </summary>
        internal void InitalizeSettingWatchTask()
        {
            lock (Lock)
            {
                //Task.Factory.StartNew pattern
                _settingWatchTask = Task.Factory.StartNew(() =>
                {
                    //Initially sleep for the refresh time since settings were just retrieved from the data access layer
                    Thread.Sleep(_settingInitialization.CacheRefreshTimeoutMs);

                    //Run this indefinitly
                    while (true)
                    {
                        try
                        {
                            //Return all settings that have changed since the last time a setting was refreshed
                            List<DbAppSettingDto> settingDtos = _settingInitialization.AppSettingDao.GetChangedDbAppSettings(_lastRefreshedTime).ToList();
                            if (settingDtos.Any())
                            {
                                //Run in sync mode
                                lock (Lock)
                                {
                                    //Update the settings
                                    SetSettingValues(settingDtos);

                                    //Store a reference to the latest changed time
                                    _lastRefreshedTime = settingDtos.Max(d => d.ModifiedDate);
                                }
                            }

                            //Sleep before checking again
                            Thread.Sleep(_settingInitialization.CacheRefreshTimeoutMs);
                        }
                        catch (Exception e)
                        {
                            //cacheManager.NotifyOfException(e);
                        }
                    }
                });
            }
        }

        /// <summary>
        /// Adds or updates settings in the internal dictionary of settings
        /// </summary>
        /// <param name="settingDtos"></param>
        internal void SetSettingValues(List<DbAppSettingDto> settingDtos)
        {
            lock (Lock)
            {
                //If any applications are provided, filter the results to only those settings
                if (_settingInitialization.Applications.Any())
                    settingDtos = settingDtos.Where(d => _settingInitialization.Applications.Contains(d.ApplicationKey)).ToList();

                //Always store the dtos by key in case we do not have a loaded assembly when searching for a type. 
                //  This allows hydration without hitting the data access layer
                foreach (DbAppSettingDto settingDto in settingDtos)
                {
                    if (!SettingDtosByKey.ContainsKey(settingDto.Key))
                        SettingDtosByKey.Add(settingDto.Key, settingDto);
                    else
                        SettingDtosByKey[settingDto.Key] = settingDto;
                }

                //Get all settings in the loaded assemblies
                List<Type> genericSettingTypes = DbAppSettingAssemblySearcher.GetGenericDbAppSettings();

                //Iterate through the types
                foreach (Type type in genericSettingTypes)
                {
                    //Create an instance of each type
                    object activatedType = Activator.CreateInstance(type);

                    //Cast to the lowest base class of a DbAppSetting
                    InternalDbAppSettingBase dbAppSetting = activatedType as InternalDbAppSettingBase;
                    if (dbAppSetting == null)
                        continue;

                    //Attempt to hydrate the setting from the dto if we have not done so already
                    HydrateSettingFromDto(dbAppSetting);
                }
            }
        }

        private static void HydrateSettingFromDto(InternalDbAppSettingBase dbAppSetting)
        {
            //If we have not yet loaded an assembly of a type, ignore this setting
            if (!SettingDtosByKey.ContainsKey(dbAppSetting.FullSettingName))
                return;

            DbAppSettingDto settingDto = SettingDtosByKey[dbAppSetting.FullSettingName];

            //Add or update the internal dictionary of the DbAppSetting after hydration from the dto
            if (_dbAppSettingDictionary.ContainsKey(dbAppSetting.FullSettingName))
            {
                dbAppSetting = _dbAppSettingDictionary.Get(dbAppSetting.FullSettingName);
                dbAppSetting.From(settingDto);
            }
            else
            {
                dbAppSetting.From(settingDto);
                _dbAppSettingDictionary.Add(dbAppSetting.FullSettingName, dbAppSetting);
            }
        }
    }
}