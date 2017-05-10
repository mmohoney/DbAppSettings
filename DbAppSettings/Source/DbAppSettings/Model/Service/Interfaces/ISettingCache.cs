using DbAppSettings.Model.Service.SettingCacheProvider.Interfaces;

namespace DbAppSettings.Model.Service.Interfaces
{
    /// <summary>
    /// Setting cache 
    /// </summary>
    internal interface ISettingCache
    {
        ISettingCacheProvider SettingCacheProvider { get; }

        void InitializeCache(ISettingCacheProvider settingCacheProvider);
    }
}
