using DbAppSettings.Model.DataTransfer;

namespace DbAppSettings.Model.DataAccess.Interfaces
{
    /// <summary>
    /// Represents the data access portion of the settings cache. Needs to be implemented for data access.
    /// Provides additional functionality for the settings cache
    /// </summary>
    public interface IDbAppSettingAdvancedDao : IDbAppSettingDao
    {
        /// <summary>
        /// If a setting is not found in the initial retrieve or in the changed retrieval, call this method to save the setting.
        /// Implementaiton should fill in the four audit fields on the DbAppSettingDto
        /// </summary>
        /// <param name="dbAppSettingDto"></param>
        void SaveNewSettingIfNotExists(DbAppSettingDto dbAppSettingDto);
    }
}
