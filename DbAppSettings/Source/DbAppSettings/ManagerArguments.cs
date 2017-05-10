using System;
using System.Collections.Generic;

namespace DbAppSettings
{
    /// <summary>
    /// Base class of the manager arguments
    /// </summary>
    public abstract class ManagerArguments
    {
        /// <summary>
        /// Protected constructor
        /// </summary>
        protected ManagerArguments()
        {
            
        }

        /// <summary>
        /// *Optional Property*
        /// If any applications are passed in. The SettingCache will only pull up to date values for the specified applications. 
        /// Generally this will not be needed unless specific security is needed
        /// </summary>
        public List<string> Applications { get; set; } = new List<string>();

        /// <summary>
        /// *Optional Property*
        /// Default of 5 seconds
        /// Function that returns a timespan how long to wait before checking the data access layer for new values
        /// </summary>
        public Func<TimeSpan> CacheRefreshTimeout { get; set; } = () => TimeSpan.FromSeconds(5);
    }
}
