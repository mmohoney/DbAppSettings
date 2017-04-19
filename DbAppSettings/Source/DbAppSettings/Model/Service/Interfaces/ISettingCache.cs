namespace DbAppSettings.Model.Service.Interfaces
{
    /// <summary>
    /// Represents a cache of DbAppSettings
    /// </summary>
    internal interface ISettingCache
    {
        void InitializeCache(ISettingInitialization cacheManager);
    }
}