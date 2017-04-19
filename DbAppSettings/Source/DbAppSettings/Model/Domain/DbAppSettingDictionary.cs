using System.Collections.Generic;

namespace DbAppSettings.Model.Domain
{
    /// <summary>
    /// Internal dictionary representing string keys and DbAppSetting
    /// </summary>
    internal class DbAppSettingDictionary
    {
        private readonly Dictionary<string, InternalDbAppSettingBase> _dict = new Dictionary<string, InternalDbAppSettingBase>();

        private DbAppSettingDictionary()
        {
            
        }

        public static DbAppSettingDictionary Create()
        {
            return new DbAppSettingDictionary();
        }

        public bool ContainsKey(string key)
        {
            return _dict.ContainsKey(key);
        }

        public bool ContainsKey<T, TValueType>() where T : DbAppSetting<T, TValueType>, new()
        {
            T setting = new T();
            return _dict.ContainsKey(setting.FullSettingName);
        }

        public void Add(string key, InternalDbAppSettingBase obj)
        {
            _dict.Add(key, obj);
        }

        public void Add<T, TValueType>(DbAppSetting<T, TValueType> dbAppSetting) where T : DbAppSetting<T, TValueType>, new()
        {
            T setting = new T();
            _dict.Add(setting.FullSettingName, dbAppSetting);
        }

        public InternalDbAppSettingBase Get(string key)
        {
            return _dict[key];
        }

        public DbAppSetting<T, TValueType> Get<T, TValueType>() where T : DbAppSetting<T, TValueType>, new()
        {
            T setting = new T();
            return _dict[setting.FullSettingName] as DbAppSetting<T, TValueType>;
        }
    }
}
