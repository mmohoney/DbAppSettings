using DbAppSettings.Model.DataAccess.Interfaces;

namespace DbAppSettings.Model.Service.CacheManager.Arguments
{
    /// <summary>
    /// Allows passing in of initialization parameters into the DbAppSettingCacheManager
    /// </summary>
    public class LazyLoadManagerArguments : CacheManagerArguments
    {
        /// <summary>
        /// Implementation of the data access layer which will lazy load settings as they are hit as opposed to all settings at startup
        /// </summary>
        public ILazyLoadSettingDao LazyLoadSettingDao { get; set; }
    }
}
