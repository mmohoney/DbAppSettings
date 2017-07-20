using DbAppSettings.Model.DataTransfer;
using DbAppSettings.Model.Domain;
using DbAppSettings.Model.Service.Interfaces;
using DbAppSettings.Model.Service.SettingCacheProvider.Interfaces;

namespace DbAppSettings.Model.Service
{
    /// <summary>
    /// Singleton instance exposed for DbAppSetting to get it's value
    /// </summary>
    internal class SettingCache : ISettingCache
    {
        private static readonly object Lock = new object();
        private static SettingCache _singleton;
        private static ISettingCacheProvider _settingCacheProvider;

        private SettingCache()
        {
            
        }

        public static ISettingCache Instance
        {
            get
            {
                if (_singleton == null)
                {
                    lock (Lock)
                    {
                        if (_singleton == null)
                            _singleton = new SettingCache();
                    }
                }
                return _singleton;
            }
        }

        public ISettingCacheProvider SettingCacheProvider => _settingCacheProvider;

        internal static DbAppSetting<T, TValueType> GetDbAppSetting<T, TValueType>() where T : DbAppSetting<T, TValueType>, new()
        {
            Instance.SettingCacheProvider.IntializationCheck();

            return Instance.SettingCacheProvider.GetDbAppSetting<T, TValueType>();
        }

        internal static TValueType GetDbAppSettingValue<TValueType>(DbAppSettingDto dbAppSettingDto)
        {
            if (dbAppSettingDto == null)
                return default(TValueType);

            Instance.SettingCacheProvider.IntializationCheck();

            return Instance.SettingCacheProvider.GetDbAppSettingValue<TValueType>(dbAppSettingDto);
        }

        public void InitializeCache(ISettingCacheProvider settingCacheProvider)
        {
            _settingCacheProvider = settingCacheProvider;

            _settingCacheProvider.InitalizeSettingCacheProvider();
        }
    }
}
