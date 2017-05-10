using DbAppSettings.Model.Service.SettingCacheProvider.Interfaces;

namespace DbAppSettings.Model.Service.Interfaces
{
    internal interface ISettingCacheV2
    {
        ISettingCacheProvider SettingCacheProvider { get; }

        void InitializeCache(ISettingCacheProvider settingCacheProvider);
    }
}
