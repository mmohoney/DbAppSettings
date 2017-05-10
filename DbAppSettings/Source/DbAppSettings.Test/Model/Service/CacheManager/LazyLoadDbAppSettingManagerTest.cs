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
    public class LazyLoadDbAppSettingManagerTest : SettingManagerTestBase
    {
        class LazyLoadProviderFactory : ISettingCacheProviderFactory
        {
            public ISettingCacheProvider GetSettingCacheProvider(CacheManagerArguments cacheManagerArguments)
            {
                return new LazyLoadSettingCacheProvider(cacheManagerArguments as LazyLoadManagerArguments);
            }
        }

        [Test]
        public void LazyLoadDbAppSettingManager()
        {
            LazyLoadDbAppSettingManager cacheManager = new LazyLoadDbAppSettingManager(new LazyLoadProviderFactory(), new DummySettingCache());
            Assert.IsNotNull(cacheManager);
        }

        [Test]
        public void LazyLoadDbAppSettingManager_UseDefaults()
        {
            LazyLoadDbAppSettingManager cacheManager = new LazyLoadDbAppSettingManager(new LazyLoadProviderFactory(), new DummySettingCache());
            Assert.IsNotNull(cacheManager);

            LazyLoadManagerArguments arguments = new LazyLoadManagerArguments();
            cacheManager.Create(arguments);

            Assert.IsNotNull(arguments.LazyLoadSettingDao);
            Assert.IsTrue(arguments.LazyLoadSettingDao.GetType() == typeof(DefaultLazyLoadSettingDao));
            Assert.IsNotNull(arguments.SaveNewSettingDao);
            Assert.IsTrue(arguments.SaveNewSettingDao.GetType() == typeof(DefaultSaveNewSettingDao));
        }

        [Test]
        public void LazyLoadDbAppSettingManager_CacheAlreadyIntailized()
        {
            LazyLoadDbAppSettingManager cacheManager = new LazyLoadDbAppSettingManager(new LazyLoadProviderFactory(), new DummySettingCacheIntailized());
            Assert.IsNotNull(cacheManager);

            LazyLoadManagerArguments arguments = new LazyLoadManagerArguments();
            cacheManager.Create(arguments);

            Assert.IsNull(arguments.LazyLoadSettingDao);
            Assert.IsNull(arguments.SaveNewSettingDao);
        }

        [Test]
        public void LazyLoadDbAppSettingManager_IntailizeCache()
        {
            var cache = new DummySettingCache();
            LazyLoadDbAppSettingManager cacheManager = new LazyLoadDbAppSettingManager(new LazyLoadProviderFactory(), cache);
            Assert.IsNotNull(cacheManager);

            LazyLoadManagerArguments arguments = new LazyLoadManagerArguments();
            cacheManager.Create(arguments);

            Assert.IsTrue(cache.HitCount == 1);
        }
    }
}
