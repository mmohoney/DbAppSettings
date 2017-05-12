using System;
using DbAppSettings.Model.DataAccess.Interfaces;
using DbAppSettings.Model.Service.CacheManager;
using DbAppSettings.Model.Service.CacheManager.Arguments;

namespace DbAppSettings
{
    /// <summary>
    /// Class used to instantiate the setting cachs. Wraps all necessary methods to invoke the caches.
    /// </summary>
    public class DbAppSettingCacheManager
    {
        /// <summary>
        /// Basic way of intializing the underlying caches by providing just the data access layer needed to get all settings.
        /// </summary>
        /// <param name="retrieveAllSettingDao">Implementation of the get all interface. Cannot be null.</param>
        /// <returns></returns>
        public static void CreateAndIntialize(IRetrieveAllSettingDao retrieveAllSettingDao)
        {
            new RetrieveAllDbAppSettingManager().Create(new RetrieveAllManagerArguments { RetrieveAllSettingDao = retrieveAllSettingDao });
        }

        /// <summary>
        /// Provides a more advanced way of intializing the underlying caches by providing overrides and additional data access methods.
        /// </summary>
        /// <param name="retrieveAllManagerArguments">Arguments cannot be null.</param>
        /// <returns></returns>
        public static void CreateAndIntialize(RetrieveAllManagerArguments retrieveAllManagerArguments)
        {
            if (retrieveAllManagerArguments == null)
                throw new NullReferenceException("retrieveAllManagerArguments cannot be null");

            new RetrieveAllDbAppSettingManager().Create(retrieveAllManagerArguments);
        }

        /// <summary>
        /// Basic way of intializing the underlying caches by providing just the data access layer needed to lazy load the settings.
        /// </summary>
        /// <param name="lazyLoadSettingDao">Implementation of the lazy load interface. Cannot be null.</param>
        /// <returns></returns>
        public static void CreateAndIntialize(ILazyLoadSettingDao lazyLoadSettingDao)
        {
            new LazyLoadDbAppSettingManager().Create(new LazyLoadManagerArguments { LazyLoadSettingDao = lazyLoadSettingDao });
        }

        /// <summary>
        /// Provides a more advanced way of intializing the underlying caches by providing overrides and additional data access methods.
        /// </summary>
        /// <param name="lazyLoadManagerArguments">Arguments cannot be null.</param>
        /// <returns></returns>
        public static void CreateAndIntialize(LazyLoadManagerArguments lazyLoadManagerArguments)
        {
            if (lazyLoadManagerArguments == null)
                throw new NullReferenceException("lazyLoadManagerArguments cannot be null");

            new LazyLoadDbAppSettingManager().Create(lazyLoadManagerArguments);
        }
    }
}
