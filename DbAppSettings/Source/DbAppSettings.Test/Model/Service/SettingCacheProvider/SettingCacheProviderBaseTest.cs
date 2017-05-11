using System;
using DbAppSettings.Model.Service.SettingCacheProvider;
using DbAppSettings.Test.Mock;
using NUnit.Framework;

namespace DbAppSettings.Test.Model.Service.SettingCacheProvider
{
    [TestFixture]
    public class SettingCacheProviderBaseTest : ProviderTestBase
    {
        [Test]
        public void InitalizeSettingCacheProvider_LastRefreshedTime()
        {
            DummySettingCacheProvider provider = new DummySettingCacheProvider(new DummyCacheManagerArguments() { CacheRefreshTimeout = () => TimeSpan.FromMilliseconds(0) });
            Assert.IsNull(SettingCacheProviderBase.LastRefreshedTime);

            provider.InitalizeSettingCacheProvider();

            Assert.IsTrue(provider.InitializeSettingCacheProviderHitCount == 1);
            Assert.IsNotNull(SettingCacheProviderBase.LastRefreshedTime);
            Assert.IsTrue(SettingCacheProviderBase.LastRefreshedTime > DateTime.MinValue);
            Assert.IsTrue(SettingCacheProviderBase.Initalized);

        }
    }
}
