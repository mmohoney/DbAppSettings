using System;
using System.Collections.Generic;
using DbAppSettings.Model.DataTransfer;

namespace DbAppSettings.Model.DataAccess.Interfaces
{
    /// <summary>
    /// Represents the data access portion of the settings cache. Needs to be implemented for data access
    /// </summary>
    public interface IDbAppSettingDao
    {
        /// <summary>
        /// Returns all settings from the data access. Initially used to load all settings into the cache
        /// </summary>
        /// <returns>all settings</returns>
        IEnumerable<DbAppSettingDto> GetAllDbAppSettings();

        /// <summary>
        /// Returns all settings from the data access that have changed since the last time a value was retrieved
        /// </summary>
        /// <param name="latestDbAppSettingChangedDate"></param>
        /// <returns></returns>
        IEnumerable<DbAppSettingDto> GetChangedDbAppSettings(DateTime? latestDbAppSettingChangedDate);
    }
}
