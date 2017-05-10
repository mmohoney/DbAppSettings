using DbAppSettings.Model.Domain;

namespace DbAppSettings.Model.Service.SettingCacheProvider.Interfaces
{
    internal interface ISettingCacheProvider
    {
        bool IsInitalized { get; }

        void InitalizeSettingCacheProvider();

        DbAppSetting<T, TValueType> GetDbAppSetting<T, TValueType>() where T : DbAppSetting<T, TValueType>, new();
    }
}
