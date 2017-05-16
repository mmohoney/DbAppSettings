using System;
using DbAppSettings.Model.Domain;
using DbAppSettings.Model.Service.Interfaces;
using DbAppSettings.Model.Service.SettingCacheProvider.Interfaces;

namespace DbAppSettings.Test.Model.Service.CacheManager
{
    public abstract class SettingManagerTestBase
    {
        internal class DummySettingCache : ISettingCache
        {
            public ISettingCacheProvider SettingCacheProvider { get; }

            public int HitCount = 0;

            public void InitializeCache(ISettingCacheProvider settingCacheProvider)
            {
                HitCount++;
            }
        }

        internal class DummySettingCacheProvider : ISettingCacheProvider
        {
            public bool IsInitalized => true;

            public void InitalizeSettingCacheProvider()
            {
                throw new NotImplementedException();
            }

            public DbAppSetting<T, TValueType> GetDbAppSetting<T, TValueType>() where T : DbAppSetting<T, TValueType>, new()
            {
                throw new NotImplementedException();
            }

            public TValueType GetDbAppSettingValue<TValueType>(string fullSettingName)
            {
                throw new NotImplementedException();
            }
        }

        internal class DummySettingCacheIntailized : ISettingCache
        {
            public DummySettingCacheIntailized()
            {
                SettingCacheProvider = new DummySettingCacheProvider();
            }
            public ISettingCacheProvider SettingCacheProvider { get; }

            public void InitializeCache(ISettingCacheProvider settingCacheProvider)
            {
                return;
            }
        }
    }
}
