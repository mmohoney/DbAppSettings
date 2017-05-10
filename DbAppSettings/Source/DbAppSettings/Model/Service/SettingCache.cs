using System;
using DbAppSettings.Model.Domain;
using DbAppSettings.Model.Service.Interfaces;
using DbAppSettings.Model.Service.SettingCacheProvider.Interfaces;

namespace DbAppSettings.Model.Service
{
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
            if (!Instance.SettingCacheProvider.IsInitalized)
            {
                lock (Lock)
                {
                    if (!Instance.SettingCacheProvider.IsInitalized)
                        throw new Exception("Cache is uninitialized. Initalize by invoking DbAppSettingCacheManager.InitalizeSettingCacheProvider.");
                }
            }

            return Instance.SettingCacheProvider.GetDbAppSetting<T, TValueType>();
        }

        public void InitializeCache(ISettingCacheProvider settingCacheProvider)
        {
            _settingCacheProvider = settingCacheProvider;

            _settingCacheProvider.InitalizeSettingCacheProvider();
        }
    }
}
