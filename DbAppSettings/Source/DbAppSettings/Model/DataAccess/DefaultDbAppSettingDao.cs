using System;
using System.Collections.Generic;
using DbAppSettings.Model.DataAccess.Interfaces;
using DbAppSettings.Model.DataTransfer;

namespace DbAppSettings.Model.DataAccess
{
    /// <summary>
    /// Default data access layer to use if no Dao is provided. In this case, the default values will always be returned
    /// </summary>
    internal class DefaultDbAppSettingDao : IDbAppSettingDao
    {
        /// <summary>
        /// Returns all settings from the database. Initially used to load all settings into the cache
        /// </summary>
        /// <returns>all settings</returns>
        public IEnumerable<DbAppSettingDto> GetAllDbAppSettings()
        {
            return new List<DbAppSettingDto>();
        }

        /// <summary>
        /// Returns all settings from the database that have changed since the last time a value was retrieved
        /// </summary>
        /// <param name="latestDbAppSettingChangedDate"></param>
        /// <returns>all settings the have changed</returns>
        public IEnumerable<DbAppSettingDto> GetChangedDbAppSettings(DateTime? latestDbAppSettingChangedDate)
        {
            return new List<DbAppSettingDto>();
        }
    }
}
