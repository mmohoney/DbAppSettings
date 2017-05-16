using DbAppSettings.Model.Domain;

namespace DbAppSettings.Model.Service.SettingCacheProvider.Interfaces
{
    /// <summary>
    /// Cache Provider
    /// </summary>
    internal interface ISettingCacheProvider
    {
        bool IsInitalized{ get; }

        void IntializationCheck();

        void InitalizeSettingCacheProvider();

        DbAppSetting<T, TValueType> GetDbAppSetting<T, TValueType>() where T : DbAppSetting<T, TValueType>, new();

        TValueType GetDbAppSettingValue<TValueType>(string fullSettingName);
    }
}
