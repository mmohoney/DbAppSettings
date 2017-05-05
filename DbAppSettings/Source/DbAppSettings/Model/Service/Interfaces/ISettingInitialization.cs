using System;
using System.Collections.Generic;
using DbAppSettings.Model.DataAccess.Interfaces;

namespace DbAppSettings.Model.Service.Interfaces
{
    /// <summary>
    /// Represents a container of values for the SettingCache
    /// </summary>
    internal interface ISettingInitialization
    {
        /// <summary>
        /// Implementation of the data access layer
        /// </summary>
        IDbAppSettingDao DbAppSettingDao { get; }
        /// <summary>
        /// Optional - If any applications are passed in. The SettingCache will only pull up to date
        /// values for the specified applications. Generally this will not be needed unless specific security is needed
        /// </summary>
        List<string> Applications { get; }
        /// <summary>
        /// Static function that returns in milliseconds how long to wait before checking the data access layer for new values
        /// </summary>
        TimeSpan CacheRefreshTimeout { get; }
    }
}
