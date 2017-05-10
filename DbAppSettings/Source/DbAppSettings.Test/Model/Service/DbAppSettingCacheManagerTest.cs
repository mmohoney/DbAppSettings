using DbAppSettings.Model.DataAccess;
using DbAppSettings.Model.DataAccess.Implementations;
using DbAppSettings.Test.Mock;
using NUnit.Framework;

namespace DbAppSettings.Test.Model.Service
{
    [TestFixture()]
    public class DbAppSettingCacheManagerTest
    {
        private readonly RetrieveAllManagerArguments _arguments = new RetrieveAllManagerArguments() { DbAppSettingDao = new DummyDbAppSettingDao() };

        [Test]
        public void DbAppSettingCacheManager_Create()
        {
            DbAppSettingCacheManager cacheManager = new DbAppSettingCacheManager(_arguments);
            Assert.IsNotNull(cacheManager);
            Assert.IsNotNull(cacheManager.AppSettingDao);
            Assert.IsFalse(cacheManager.IsCacheInitalized);
        }

        [Test]
        public void DbAppSettingCacheManager_Create_NoDao_UseDefault()
        {
            DbAppSettingCacheManager cacheManager = new DbAppSettingCacheManager(new RetrieveAllManagerArguments());
            Assert.IsNotNull(cacheManager);
            Assert.IsNotNull(cacheManager.AppSettingDao);
            Assert.IsTrue(cacheManager.AppSettingDao.GetType() == typeof(DefaultRetrieveAllSettingDao));
        }

        //[Test]
        //public void CacheRefreshTimeoutMs()
        //{
        //    int initialValue = _arguments.CacheRefreshTimeoutMs();

        //    _arguments.CacheRefreshTimeoutMs = () => initialValue + 10;

        //    DbAppSettingCacheManager cacheManager = new DbAppSettingCacheManager(_arguments);
        //    Assert.IsNotNull(cacheManager);
        //    Assert.IsFalse(cacheManager.IsCacheInitalized);

        //    Assert.IsTrue(_arguments.CacheRefreshTimeoutMs() == initialValue + 10);
        //}

        [Test]
        public void AppSettingDao()
        {
            DbAppSettingCacheManager cacheManager = new DbAppSettingCacheManager(_arguments);
            Assert.IsNotNull(cacheManager.AppSettingDao);
        }

        [Test]
        public void InitializeCache_IsCacheInitalized()
        {
            DbAppSettingCacheManager cacheManager = new DbAppSettingCacheManager(new DummyDbAppSettingDao(), new DummySettingCache());
            Assert.IsNotNull(cacheManager);
            Assert.IsFalse(cacheManager.IsCacheInitalized);

            cacheManager.InitializeCache();

            Assert.IsTrue(cacheManager.IsCacheInitalized);
        }

        [Test]
        public void DbAppSettingCacheManager_CreateAndIntialize()
        {
            DbAppSettingCacheManager cacheManager = DbAppSettingCacheManager.CreateAndIntialize(new DummyDbAppSettingDao());
            Assert.IsNotNull(cacheManager);
            Assert.IsNotNull(cacheManager.AppSettingDao);
            Assert.IsTrue(cacheManager.IsCacheInitalized);
        }

        [Test]
        public void DbAppSettingCacheManager_CreateAndIntialize_Arguments()
        {
            DbAppSettingCacheManager cacheManager = DbAppSettingCacheManager.CreateAndIntialize(_arguments);
            Assert.IsNotNull(cacheManager);
            Assert.IsNotNull(cacheManager.AppSettingDao);
            Assert.IsTrue(cacheManager.IsCacheInitalized);
        }
    }
}
