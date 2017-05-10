using DbAppSettings.Model.DataAccess.Interfaces;

namespace DbAppSettings
{
    /// <summary>
    /// Allows passing in of initialization parameters into the DbAppSettingCacheManager
    /// </summary>
    public class RetrieveAllManagerArguments : ManagerArguments
    {
        /// <summary>
        /// Implementation of the data access layer which will load all settings into memory at start up
        /// </summary>
        public IDbAppSettingDao DbAppSettingDao { get; set; }

        /// <summary>
        /// Implementation of the data access layer to save new settings that are not currently in the data access
        /// </summary>
        public IDbAppSettingSaveNewSettingDao DbAppSettingSaveNewSettingDao { get; set; }
    }
}
