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
        public IEnumerable<DbAppSettingDto> GetAllDbAppSettings()
        {
            return new List<DbAppSettingDto>();
        }

        public IEnumerable<DbAppSettingDto> GetChangedDbAppSettings(DateTime? latestDbAppSettingChangedDate)
        {
            return new List<DbAppSettingDto>();
        }
    }
}
