using System.Collections.Generic;
using DbAppSettings.Model.DataTransfer;

namespace DbAppSettings.Model.Service.Maintenance.Interfaces
{
    /// <summary>
    /// Manages all interactions between the DAL and the caller
    /// </summary>
    public interface IDbAppSettingMaintenanceService
    {
        /// <summary>
        /// Returns all settings from the DAL
        /// </summary>
        /// <returns></returns>
        List<DbAppSettingDto> GetAll();

        /// <summary>
        /// Inserts or updates the setting in the DAL
        /// </summary>
        /// <param name="dto"></param>
        void SaveDbAppSetting(DbAppSettingDto dto);

        /// <summary>
        /// Removes the setting from the DAL
        /// </summary>
        /// <param name="dto"></param>
        void DeleteDbAppSetting(DbAppSettingDto dto);

        /// <summary>
        /// Returns whether or not the value passed in is a valid value for the type
        /// </summary>
        /// <param name="value"></param>
        /// <param name="valueType"></param>
        /// <returns></returns>
        bool ValidateValueForType(object value, string valueType);
    }
}