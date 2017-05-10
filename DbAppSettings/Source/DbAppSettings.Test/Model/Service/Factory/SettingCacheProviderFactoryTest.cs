using DbAppSettings.Model.Service.CacheManager.Arguments;
using DbAppSettings.Model.Service.Factory;
using DbAppSettings.Model.Service.SettingCacheProvider;
using DbAppSettings.Model.Service.SettingCacheProvider.Interfaces;
using NUnit.Framework;

namespace DbAppSettings.Test.Model.Service.Factory
{
    [TestFixture]
    public class SettingCacheProviderFactoryTest
    {
        [Test]
        public void GetSettingCacheProvider_RetrieveAllManagerArguments()
        {
            ISettingCacheProvider result = new SettingCacheProviderFactory().GetSettingCacheProvider(new RetrieveAllManagerArguments());
            Assert.IsTrue(result.GetType() == typeof(RetrieveAllSettingCacheProvider));
        }

        [Test]
        public void GetSettingCacheProvider_LazyLoadManagerArguments()
        {
            ISettingCacheProvider result = new SettingCacheProviderFactory().GetSettingCacheProvider(new LazyLoadManagerArguments());
            Assert.IsTrue(result.GetType() == typeof(LazyLoadSettingCacheProvider));
        }
    }
}
