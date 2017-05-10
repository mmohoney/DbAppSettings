using DbAppSettings.Model.Domain;
using DbAppSettings.Model.Service.Interfaces;

namespace DbAppSettings.Model.Service
{
    /// <summary>
    /// Represents a cache of DbAppSettings
    /// </summary>
    internal class SettingCacheV2
    {
        private static readonly object Lock = new object();
        private static SettingCacheV2 _singleton;
        private ISettingCacheProvider _settingProvider;

        private SettingCacheV2()
        {
            
        }

        public static SettingCacheV2 Instance
        {
            get
            {
                if (_singleton == null)
                {
                    lock (Lock)
                    {
                        if (_singleton == null)
                            _singleton = new SettingCacheV2();
                    }
                }
                return _singleton;
            }
        }
      
        internal static DbAppSetting<T, TValueType> GetDbAppSetting<T, TValueType>() where T : DbAppSetting<T, TValueType>, new()
        {
            return Instance._settingProvider.GetDbAppSetting<T, TValueType>();
        }

        public void InitializeCache(ISettingCacheProvider settingCacheProvider)
        {
            _settingProvider = settingCacheProvider;

            _settingProvider.InitalizeSettingCacheProvider();
        }
    }
}
