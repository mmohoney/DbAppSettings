using System.Collections.Concurrent;
using System.Reflection;
using System.Threading;
using DbAppSettings.Model.DataTransfer;
using DbAppSettings.Model.Domain;
using DbAppSettings.Model.Service.SettingCacheProvider;
using NUnit.Framework;

namespace DbAppSettings.Test.Model.Service.SettingCacheProvider
{
    public class ProviderTestBase
    {
        private static readonly object Lock = new object();
        private static bool _isRunning = false;
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

                SettingCacheProviderBase.CancelTask();
                SpinWait.SpinUntil(() =>
                {
                    if (SettingCacheProviderBase.SettingWatchTask == null)
                        return true;

                    return SettingCacheProviderBase.SettingWatchTask.IsCompleted;
                });

                typeof(SettingCacheProviderBase)
                    .GetProperty("SettingWatchTask", BindingFlags.Static | BindingFlags.NonPublic)
                    .SetValue(null, null);
            }
        }

        [SetUp]
        public void Setup()
        {
            while (true)
            {
                if (!_isRunning)
                {
                    lock (Lock)
                    {
                        if (!_isRunning)
                        {
                            _isRunning = true;
                            break;
                        }
                    }
                }
                Thread.Sleep(10);
            }

            lock (Lock)
            {
                SingletonHelper.CleanUpAfterTest();
            }
        }

        [TearDown]
        public void TearDown()
        {
            lock (Lock)
            {
                SingletonHelper.CleanUpAfterTest();
                _isRunning = false;
            }
        }
    }
}
