using DbAppSettings.Model.DataAccess.Interfaces;

namespace DbAppSettings.Model.Service.CacheManager.Arguments
{
    /// <summary>
    /// Allows passing in of initialization parameters into the DbAppSettingCacheManager
    /// </summary>
    public class RetrieveAllManagerArguments : CacheManagerArguments
    {
        /// <summary>
        /// Implementation of the data access layer which will load all settings into memory at start up
        /// </summary>
        public IRetrieveAllSettingDao RetrieveAllSettingDao { get; set; }
    }
}
