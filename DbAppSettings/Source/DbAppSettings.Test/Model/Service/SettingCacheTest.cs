using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using DbAppSettings.Model.DataAccess.Interfaces;
using DbAppSettings.Model.DataTransfer;
using DbAppSettings.Model.Service;
using DbAppSettings.Test.Mock;
using NUnit.Framework;

namespace DbAppSettings.Test.Model.Service
{
    [TestFixture]
    public class SettingCacheTest
    {
        private static readonly object Lock = new object();
        private readonly DummySettingCacheTestDbAppSettingDao _dao = new DummySettingCacheTestDbAppSettingDao();

        private void VerifyDefaultValues()
        {
            Assert.IsTrue(new DummyDbAppSettings.DummyDbAppSettingDaoTestSetting1().InitialValue == 1);
            Assert.IsTrue(new DummyDbAppSettings.DummyDbAppSettingDaoTestSetting2().InitialValue == "TEST");
            Assert.IsTrue(new DummyDbAppSettings.DummyDbAppSettingDaoTestSetting3().InitialValue == false);

            Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting1.DefaultValue == 1);
            Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting2.DefaultValue == "TEST");
            Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting3.DefaultValue == false);
        }

        private void WithCleanup(Action action)
        {
            lock (Lock)
            {
                SettingCache cache = SettingCache.Instance;
                SingletonHelper.CleanUpAfterTest();
                action();
                SingletonHelper.CleanUpAfterTest();
            }
        }

        [Test]
        public void Instance()
        {
            WithCleanup(() =>
            {
                SettingCache cache = SettingCache.Instance;
                Assert.IsNotNull(cache);
            });
        }

        [Test]
        public void InitializeCache()
        {
            WithCleanup(() =>
            {
                SettingCache cache = SettingCache.Instance;
                cache.InitializeCache(new DummySettingInitialization(_dao, TimeSpan.FromMilliseconds(200), "DbAppSettingApp"));
                Assert.IsTrue(SettingCache.IsInitalized);
            });
        }

        [Test]
        public void InitalizeSettingWatchTask()
        {
            WithCleanup(() =>
            {
                SettingCache cache = SettingCache.Instance;

                cache.InitializeCache(new DummySettingInitialization(_dao, TimeSpan.FromMilliseconds(200), "DbAppSettingApp"));
                Assert.IsTrue(SettingCache.IsInitalized);
                Assert.IsNotNull(SettingCache.SettingWatchTask);
            });
        }

        [Test]
        public void SetSettingValue_AppUnspecific()
        {
            WithCleanup(() =>
            {
                VerifyDefaultValues();

                SettingCache cache = SettingCache.Instance;
                cache.InitializeCache(new DummySettingInitialization(_dao, TimeSpan.FromMilliseconds(200)));
                SpinWait.SpinUntil(() => _dao.HitCount > 5);

                Assert.IsTrue(SettingCache.IsInitalized);
                Assert.IsNotNull(SettingCache.SettingWatchTask);

                Assert.IsTrue(_dao.HitCount > 5);
                Assert.IsTrue(SettingCache.SettingDtosByKeyCount == 6);

                VerifyDefaultValues();

                Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting1.Value == 2);
                Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting2.Value == "NEW TEST");
                Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting3.Value == true);
            });
        }

        [Test]
        public void SetSettingValue_AppSpecific()
        {
            WithCleanup(() =>
            {
                VerifyDefaultValues();

                SettingCache cache = SettingCache.Instance;
                cache.InitializeCache(new DummySettingInitialization(_dao, TimeSpan.FromMilliseconds(200), "DbAppSettingApp"));
                SpinWait.SpinUntil(() => _dao.HitCount > 5);

                Assert.IsTrue(SettingCache.IsInitalized);
                Assert.IsNotNull(SettingCache.SettingWatchTask);

                Assert.IsTrue(_dao.HitCount > 5);
                Assert.IsTrue(SettingCache.SettingDtosByKeyCount == 3);

                VerifyDefaultValues();

                Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting1.Value == 2);
                Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting2.Value == "NEW TEST");
                Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting3.Value == true);
            });
        }

        [Test]
        public void DbException_Intialization()
        {
            WithCleanup(() =>
            {
                VerifyDefaultValues();

                SettingCache cache = SettingCache.Instance;

                ExceptionDummySettingCacheTestDbAppSettingDao dao = new ExceptionDummySettingCacheTestDbAppSettingDao();
                cache.InitializeCache(new DummySettingInitialization(dao, TimeSpan.FromMilliseconds(200), "DbAppSettingApp"));
                SpinWait.SpinUntil(() => dao.HitCountGetAllDbAppSettings > 0);

                VerifyDefaultValues();
            });
        }

        [Test]
        public void DbException_WatchTask()
        {

            WithCleanup(() =>
            {
                VerifyDefaultValues();

                SettingCache cache = SettingCache.Instance;

                ExceptionDummySettingCacheTestDbAppSettingDao dao = new ExceptionDummySettingCacheTestDbAppSettingDao();
                cache.InitializeCache(new DummySettingInitialization(dao, TimeSpan.FromMilliseconds(200), "DbAppSettingApp"));
                SpinWait.SpinUntil(() => dao.HitCountGetChangedDbAppSettings > 5);

                VerifyDefaultValues();
            });
        }

        [Test]
        public void NoSettings_Intialization()
        {
            WithCleanup(() =>
            {
                VerifyDefaultValues();

                SettingCache cache = SettingCache.Instance;

                NoSettingsSettingCacheTestDbAppSettingDao dao = new NoSettingsSettingCacheTestDbAppSettingDao();
                cache.InitializeCache(new DummySettingInitialization(dao, TimeSpan.FromMilliseconds(200), "DbAppSettingApp"));
                SpinWait.SpinUntil(() => dao.HitCountGetAllDbAppSettings > 0);

                VerifyDefaultValues();
            });
        }

        [Test]
        public void NoSettings_WatchTask()
        {

            WithCleanup(() =>
            {
                VerifyDefaultValues();

                SettingCache cache = SettingCache.Instance;

                NoSettingsSettingCacheTestDbAppSettingDao dao = new NoSettingsSettingCacheTestDbAppSettingDao();
                cache.InitializeCache(new DummySettingInitialization(dao, TimeSpan.FromMilliseconds(200), "DbAppSettingApp"));
                SpinWait.SpinUntil(() => dao.HitCountGetChangedDbAppSettings > 5);

                VerifyDefaultValues();
            });
        }
    }

    public class NoSettingsSettingCacheTestDbAppSettingDao : IDbAppSettingDao
    {
        public int HitCountGetAllDbAppSettings { get; set; }
        public int HitCountGetChangedDbAppSettings { get; set; }

        public IEnumerable<DbAppSettingDto> GetAllDbAppSettings()
        {
            HitCountGetAllDbAppSettings += 1;
            return new List<DbAppSettingDto>();
        }

        public IEnumerable<DbAppSettingDto> GetChangedDbAppSettings(DateTime? latestDbAppSettingChangedDate)
        {
            HitCountGetChangedDbAppSettings += 1;
            return new List<DbAppSettingDto>();
        }
    }


    public class ExceptionDummySettingCacheTestDbAppSettingDao : IDbAppSettingDao
    {
        public int HitCountGetAllDbAppSettings { get; set; }
        public int HitCountGetChangedDbAppSettings { get; set; }

        public IEnumerable<DbAppSettingDto> GetAllDbAppSettings()
        {
            HitCountGetAllDbAppSettings += 1;
            throw new Exception();
        }

        public IEnumerable<DbAppSettingDto> GetChangedDbAppSettings(DateTime? latestDbAppSettingChangedDate)
        {
            HitCountGetChangedDbAppSettings += 1;
            throw new Exception();
        }
    }

    public class DummySettingCacheTestDbAppSettingDao : IDbAppSettingDao
    {
        public int HitCount { get; set; }

        public IEnumerable<DbAppSettingDto> GetAllDbAppSettings()
        {
            return new DummyDbAppSettingsDtos().GetAllDbAppSettings();
        }

        public IEnumerable<DbAppSettingDto> GetChangedDbAppSettings(DateTime? latestDbAppSettingChangedDate)
        {
            HitCount += 1;
            return new List<DbAppSettingDto>();
        }
    }

    public static class SingletonHelper
    {
        public static void CleanUpAfterTest()
        {
            typeof(SettingCache)
                .GetField("_singleton", BindingFlags.Static | BindingFlags.NonPublic)
                .SetValue(null, null);

            typeof(SettingCache)
                .GetField("_isInitalized", BindingFlags.Static | BindingFlags.NonPublic)
                .SetValue(null, false);

            typeof(SettingCache)
                .GetField("SettingDtosByKey", BindingFlags.Static | BindingFlags.NonPublic)
                .SetValue(null, new Dictionary<string, DbAppSettingDto>());
        }
    }
}
