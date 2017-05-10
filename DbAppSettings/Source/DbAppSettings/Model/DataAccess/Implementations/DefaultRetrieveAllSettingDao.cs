using System;
using System.Collections.Generic;
using DbAppSettings.Model.DataAccess.Interfaces;
using DbAppSettings.Model.DataTransfer;

namespace DbAppSettings.Model.DataAccess.Implementations
{
    /// <summary>
    /// Default implementation if no implementation is provided by the caller
    /// </summary>
    internal class DefaultRetrieveAllSettingDao : IRetrieveAllSettingDao
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
