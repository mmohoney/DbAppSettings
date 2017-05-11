using System.Collections.Concurrent;
using System.Reflection;
using DbAppSettings.Model.DataTransfer;
using DbAppSettings.Model.Domain;
using DbAppSettings.Model.Service.SettingCacheProvider;
using NUnit.Framework;

namespace DbAppSettings.Test.Model.Service.SettingCacheProvider
{
    public class ProviderTestBase
    {
        public class DbAppSettingTestSetting : DbAppSetting<DbAppSettingTestSetting, int> { public override int InitialValue => 1; }

        public static class SingletonHelper
        {
            public static void CleanUpAfterTest()
            {
                typeof(SettingCacheProviderBase)
                    .GetField("SettingDtosByKey", BindingFlags.Static | BindingFlags.NonPublic)
                    .SetValue(null, new ConcurrentDictionary<string, DbAppSettingDto>());

                typeof(SettingCacheProviderBase)
                    .GetField("LastRefreshedTime", BindingFlags.Static | BindingFlags.NonPublic)
                    .SetValue(null, null);

                typeof(SettingCacheProviderBase)
                    .GetProperty("Initalized", BindingFlags.Static | BindingFlags.NonPublic)
                    .SetValue(null, false);
            }
        }

        [SetUp]
        public void Setup()
        {
            SingletonHelper.CleanUpAfterTest();
        }

        [TearDown]
        public void TearDown()
        {
            SingletonHelper.CleanUpAfterTest();
        }
    }
}
