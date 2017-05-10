using DbAppSettings.Model.DataTransfer;

namespace DbAppSettings.Model.DataAccess.Interfaces
{
    /// <summary>
    /// Represents the data access portion of the settings cache which will insert new settings into the data access layer
    /// </summary>
    public interface ISaveNewSettingDao
    {
        /// <summary>
        /// Saves new settings into the data access layer. Invoked when a new setting is invoked that does not exist in the data access layer.
        /// </summary>
        /// <param name="dbAppSettingDto">dto representation of the setting needing to be saved</param>
        void SaveNewSettingIfNotExists(DbAppSettingDto dbAppSettingDto);
    }
}
