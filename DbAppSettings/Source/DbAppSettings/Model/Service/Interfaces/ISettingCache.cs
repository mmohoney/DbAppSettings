namespace DbAppSettings.Model.Service.Interfaces
{
    /// <summary>
    /// Represents a cache of DbAppSettings
    /// </summary>
    internal interface ISettingCache
    {
        /// <summary>
        /// Initialize the cache
        /// </summary>
        /// <param name="cacheManager"></param>
        void InitializeCache(ISettingInitialization cacheManager);
    }
}