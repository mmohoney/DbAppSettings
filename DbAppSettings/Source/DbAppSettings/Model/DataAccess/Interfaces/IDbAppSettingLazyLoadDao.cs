using System;
using System.Collections.Generic;
using DbAppSettings.Model.DataTransfer;

namespace DbAppSettings.Model.DataAccess.Interfaces
{
    /// <summary>
    /// Represents the data access portion which implements the lazy load features. 
    /// Provides the cache with a way to load only specific settings on demand rather than all during intialization.
    /// </summary>
    public interface IDbAppSettingLazyLoadDao
    {
        /// <summary>
        /// Returns the up to date representation of the argument supplied
        /// </summary>
        /// <param name="dbAppSettingDto">the value needing retrieval</param>
        /// <returns>the up to date representation</returns>
        IEnumerable<DbAppSettingDto> GetDbAppSetting(DbAppSettingDto dbAppSettingDto);

        /// <summary>
        /// Returns all settings from the data access that have changed since the last time a value was retrieved.
        /// </summary>
        /// <param name="latestDbAppSettingChangedDate">the latest time any setting was changed from the data access layer</param>
        /// <returns>all changed settings</returns>
        IEnumerable<DbAppSettingDto> GetChangedDbAppSettings(DateTime? latestDbAppSettingChangedDate);
    }
}
