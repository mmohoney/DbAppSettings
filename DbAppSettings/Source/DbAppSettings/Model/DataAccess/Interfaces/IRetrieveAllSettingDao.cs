using System;
using System.Collections.Generic;
using DbAppSettings.Model.DataTransfer;

namespace DbAppSettings.Model.DataAccess.Interfaces
{
    /// <summary>
    /// Represents the data access portion of the settings cache. Provides the cache with all settings from the data access at intialization.
    /// </summary>
    public interface IRetrieveAllSettingDao
    {
        /// <summary>
        /// Returns all settings from the data access.
        /// </summary>
        /// <returns>all settings</returns>
        IEnumerable<DbAppSettingDto> GetAllDbAppSettings();

        /// <summary>
        /// Returns all settings from the data access that have changed since the last time a value was retrieved.
        /// </summary>
        /// <param name="latestDbAppSettingChangedDate">the latest time any setting was changed from the data access layer</param>
        /// <returns>all changed settings</returns>
        IEnumerable<DbAppSettingDto> GetChangedDbAppSettings(DateTime? latestDbAppSettingChangedDate);
    }
}
