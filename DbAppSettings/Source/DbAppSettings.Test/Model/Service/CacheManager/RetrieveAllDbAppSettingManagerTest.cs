using DbAppSettings.Model.DataAccess.Implementations;
using DbAppSettings.Model.Service.CacheManager;
using DbAppSettings.Model.Service.CacheManager.Arguments;
using DbAppSettings.Model.Service.Factory.Interfaces;
using DbAppSettings.Model.Service.SettingCacheProvider;
using DbAppSettings.Model.Service.SettingCacheProvider.Interfaces;
using NUnit.Framework;

namespace DbAppSettings.Test.Model.Service.CacheManager
{
    [TestFixture]
    public class RetrieveAllDbAppSettingManagerTest : SettingManagerTestBase
    {
        class RetrieveAllProviderFactory : ISettingCacheProviderFactory
        {
            public ISettingCacheProvider GetSettingCacheProvider(CacheManagerArguments cacheManagerArguments)
            {
                return new RetrieveAllSettingCacheProvider(cacheManagerArguments as RetrieveAllManagerArguments);
            }
        }

        [Test]
        public void RetrieveAllDbAppSettingManager()
        {
            RetrieveAllDbAppSettingManager cacheManager = new RetrieveAllDbAppSettingManager(new RetrieveAllProviderFactory(), new DummySettingCache());
            Assert.IsNotNull(cacheManager);
        }

        [Test]
        public void LazyLoadDbAppSettingManager_UseDefaults()
        {
            RetrieveAllDbAppSettingManager cacheManager = new RetrieveAllDbAppSettingManager(new RetrieveAllProviderFactory(), new DummySettingCache());
            Assert.IsNotNull(cacheManager);

            RetrieveAllManagerArguments arguments = new RetrieveAllManagerArguments();
            cacheManager.Create(arguments);

            Assert.IsNotNull(arguments.RetrieveAllSettingDao);
            Assert.IsTrue(arguments.RetrieveAllSettingDao.GetType() == typeof(DefaultRetrieveAllSettingDao));
            Assert.IsNotNull(arguments.SaveNewSettingDao);
            Assert.IsTrue(arguments.SaveNewSettingDao.GetType() == typeof(DefaultSaveNewSettingDao));
        }

        [Test]
        public void LazyLoadDbAppSettingManager_CacheAlreadyIntailized()
        {
            RetrieveAllDbAppSettingManager cacheManager = new RetrieveAllDbAppSettingManager(new RetrieveAllProviderFactory(), new DummySettingCacheIntailized());
            Assert.IsNotNull(cacheManager);

            RetrieveAllManagerArguments arguments = new RetrieveAllManagerArguments();
            cacheManager.Create(arguments);

            Assert.IsNull(arguments.RetrieveAllSettingDao);
            Assert.IsNull(arguments.SaveNewSettingDao);
        }

        [Test]
        public void LazyLoadDbAppSettingManager_IntailizeCache()
        {
            DummySettingCache cache = new DummySettingCache();
            RetrieveAllDbAppSettingManager cacheManager = new RetrieveAllDbAppSettingManager(new RetrieveAllProviderFactory(), cache);
            Assert.IsNotNull(cacheManager);

            RetrieveAllManagerArguments arguments = new RetrieveAllManagerArguments();
            cacheManager.Create(arguments);

            Assert.IsTrue(cache.HitCount == 1);
        }
    }
}
