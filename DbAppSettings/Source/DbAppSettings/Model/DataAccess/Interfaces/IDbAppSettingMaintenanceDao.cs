using System.Collections.Generic;
using DbAppSettings.Model.DataTransfer;

namespace DbAppSettings.Model.DataAccess.Interfaces
{
    /// <summary>
    /// Represents the porition of the data access which manages CRUD operations with the DAL
    /// </summary>
    public interface IDbAppSettingMaintenanceDao
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
    }
}