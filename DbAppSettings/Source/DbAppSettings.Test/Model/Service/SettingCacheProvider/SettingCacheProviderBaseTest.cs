using System;
using System.Threading;
using DbAppSettings.Model.Service.SettingCacheProvider;
using DbAppSettings.Test.Mock;
using NUnit.Framework;

namespace DbAppSettings.Test.Model.Service.SettingCacheProvider
{
    [TestFixture]
    public class SettingCacheProviderBaseTest : ProviderTestBase
    {
        [Test]
        public void InitalizeSettingCacheProvider()
        {
            DummySettingCacheProvider provider = new DummySettingCacheProvider(new DummyCacheManagerArguments() { CacheRefreshTimeout = () => TimeSpan.FromMilliseconds(0) });
            Assert.IsNull(SettingCacheProviderBase.LastRefreshedTime);

            provider.InitalizeSettingCacheProvider();

            Assert.IsTrue(provider.InitializeSettingCacheProviderHitCount == 1);
            Assert.IsNotNull(SettingCacheProviderBase.LastRefreshedTime);
            Assert.IsTrue(SettingCacheProviderBase.LastRefreshedTime > DateTime.MinValue);
            Assert.IsTrue(SettingCacheProviderBase.Initalized);
        }

        [Test]
        public void InitalizeSettingWatchTask_InvokeCancel()
        {
            DummySettingCacheProvider2 provider = new DummySettingCacheProvider2(new DummyCacheManagerArguments() { CacheRefreshTimeout = () => TimeSpan.FromMilliseconds(0) });
            Assert.IsNull(SettingCacheProviderBase.LastRefreshedTime);

            provider.InitalizeSettingCacheProvider();

            Assert.IsNotNull(SettingCacheProviderBase.LastRefreshedTime);
            Assert.IsTrue(SettingCacheProviderBase.LastRefreshedTime > DateTime.MinValue);
            Assert.IsTrue(SettingCacheProviderBase.Initalized);

            SettingCacheProviderBase.CancelTask();
            SpinWait.SpinUntil(() =>
            {
                if (SettingCacheProviderBase.SettingWatchTask == null)
                    return true;

                return SettingCacheProviderBase.SettingWatchTask.IsCompleted;
            });

            Assert.IsTrue(SettingCacheProviderBase.SettingWatchTask.IsCompleted);
        }

        [Test]
        public void InitalizeSettingWatchTask_GetChangedSettingsNone()
        {
            DummySettingCacheProvider2 provider = new DummySettingCacheProvider2(new DummyCacheManagerArguments() { CacheRefreshTimeout = () => TimeSpan.FromMilliseconds(0) });
            Assert.IsNull(SettingCacheProviderBase.LastRefreshedTime);

            provider.InitalizeSettingCacheProvider();

            Assert.IsNotNull(SettingCacheProviderBase.LastRefreshedTime);
            Assert.IsTrue(SettingCacheProviderBase.LastRefreshedTime > DateTime.MinValue);
            Assert.IsTrue(SettingCacheProviderBase.Initalized);

            SpinWait.SpinUntil(() => provider.GetChangedSettingsHitCount > 0);

            Assert.IsTrue(SettingCacheProviderBase.SettingDtosByKey.Count == 0);
        }

        [Test]
        public void InitalizeSettingWatchTask_GetChangedSettingsOne()
        {
            DummySettingCacheProvider3 provider = new DummySettingCacheProvider3(new DummyCacheManagerArguments() { CacheRefreshTimeout = () => TimeSpan.FromMilliseconds(0) });
            Assert.IsNull(SettingCacheProviderBase.LastRefreshedTime);

            provider.InitalizeSettingCacheProvider();

            Assert.IsNotNull(SettingCacheProviderBase.LastRefreshedTime);
            Assert.IsTrue(SettingCacheProviderBase.LastRefreshedTime > DateTime.MinValue);
            Assert.IsTrue(SettingCacheProviderBase.Initalized);

            SpinWait.SpinUntil(() => SettingCacheProviderBase.SettingDtosByKey.Count > 0 && SettingCacheProviderBase.LastRefreshedTime == DateTime.Today.AddDays(1));

            Assert.IsTrue(SettingCacheProviderBase.SettingDtosByKey.Count == 1);

            Assert.IsTrue(SettingCacheProviderBase.LastRefreshedTime == DateTime.Today.AddDays(1));
        }
    }
}
