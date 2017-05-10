using DbAppSettings.Model.Domain;

namespace DbAppSettings.Model.Service.Interfaces
{
    internal interface ISettingCacheProvider
    {
        void InitalizeSettingCacheProvider();

        DbAppSetting<T, TValueType> GetDbAppSetting<T, TValueType>() where T : DbAppSetting<T, TValueType>, new();
    }
}
