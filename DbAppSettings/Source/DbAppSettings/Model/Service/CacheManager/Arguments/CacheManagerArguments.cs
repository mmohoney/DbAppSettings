using System;
using DbAppSettings.Model.DataAccess.Interfaces;

namespace DbAppSettings.Model.Service.CacheManager.Arguments
{
    /// <summary>
    /// Base class of the manager arguments
    /// </summary>
    public abstract class CacheManagerArguments
    {
        /// <summary>
        /// Protected constructor
        /// </summary>
        protected CacheManagerArguments()
        {
            
        }

        /// <summary>
        /// *Optional Property*
        /// Default of 5 seconds
        /// Function that returns a timespan how long to wait before checking the data access layer for new values
        /// </summary>
        public Func<TimeSpan> CacheRefreshTimeout { get; set; } = () => TimeSpan.FromSeconds(5);

        /// <summary>
        /// Implementation of the data access layer to save new settings that are not currently in the data access
        /// </summary>
        public ISaveNewSettingDao SaveNewSettingDao { get; set; }
    }
}
