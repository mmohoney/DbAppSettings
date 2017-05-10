namespace DbAppSettings.Model.Service.Interfaces
{
    internal interface ISettingCache
    {
        void InitializeCache(ISettingInitialization cacheManager);
    }
}