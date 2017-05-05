using System;
using System.Collections.Generic;
using DbAppSettings.Model.DataAccess.Interfaces;

namespace DbAppSettings
{
    /// <summary>
    /// Allows passing in of initialization parameters into the DbAppSettingCacheManager
    /// </summary>
    public class DbAppSettingManagerArguments
    {
        /// <summary>
        /// Implementation of the data access layer
        /// </summary>
        public IDbAppSettingDao DbAppSettingDao { get; set; }
        /// <summary>
        /// Optional - If any applications are passed in. The SettingCache will only pull up to date values for the specified applications. 
        /// Generally this will not be needed unless specific security is needed
        /// </summary>
        public List<string> Applications { get; set; } = new List<string>();
        /// <summary>
        /// Optional - Function that returns a timespan how long to wait before checking the data access layer for new values
        /// Defauls to 5 seconds
        /// </summary>
        public Func<TimeSpan> CacheRefreshTimeout { get; set; } = () => TimeSpan.FromSeconds(5);
    }
}
