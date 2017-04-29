using System;
using System.Collections.Generic;
using System.Threading;
using DbAppSettings.Model.DataAccess.Interfaces;
using DbAppSettings.Model.DataTransfer;
using DbAppSettings.Model.Service;
using DbAppSettings.Test.Mock;
using NUnit.Framework;

namespace DbAppSettings.Test.Model.Service
{
    [TestFixture()]
    public class SettingCacheTest
    {
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

        private readonly DummySettingCacheTestDbAppSettingDao _dao = new DummySettingCacheTestDbAppSettingDao();

        [Test]
        public void Instance()
        {
            SettingCache cache = SettingCache.Instance;
            Assert.IsNotNull(cache);
        }

        [Test]
        public void InitializeCache()
        {
            SettingCache cache = SettingCache.Instance;
            Assert.IsNotNull(cache);

            cache.InitializeCache(new DummySettingInitialization(_dao, 200, "DbAppSettingApp"));
            SpinWait.SpinUntil(() =>
            {
                return _dao.HitCount > 5;
            });

            Assert.IsTrue(_dao.HitCount > 5);
            Assert.IsTrue(SettingCache.IsInitalized);
        }

        [Test]
        public void InitalizeSettingWatchTask()
        {
            SettingCache cache = SettingCache.Instance;
            Assert.IsNotNull(cache);

            cache.InitializeCache(new DummySettingInitialization(_dao, 200, "DbAppSettingApp"));
            SpinWait.SpinUntil(() =>
            {
                return _dao.HitCount > 5;
            });

            Assert.IsTrue(_dao.HitCount > 5);
            Assert.IsTrue(SettingCache.IsInitalized);
            Assert.IsNotNull(SettingCache.SettingWatchTask);
        }

        [Test, Ignore("run expliclity")]
        public void SetSettingValue_AppUnspecific()
        {
            Assert.IsTrue(new DummyDbAppSettings.DummyDbAppSettingDaoTestSetting1().InitialValue == 1);
            Assert.IsTrue(new DummyDbAppSettings.DummyDbAppSettingDaoTestSetting2().InitialValue == "TEST");
            Assert.IsTrue(new DummyDbAppSettings.DummyDbAppSettingDaoTestSetting3().InitialValue == false);

            Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting1.DefaultValue == 1);
            Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting2.DefaultValue == "TEST");
            Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting3.DefaultValue == false);

            SettingCache cache = SettingCache.Instance;
            Assert.IsNotNull(cache);

            cache.InitializeCache(new DummySettingInitialization(_dao, 200));
            SpinWait.SpinUntil(() =>
            {
                return _dao.HitCount > 5;
            });

            Assert.IsTrue(_dao.HitCount > 5);
            Assert.IsTrue(SettingCache.IsInitalized);
            Assert.IsNotNull(SettingCache.SettingWatchTask);
            Assert.IsTrue(SettingCache.SettingDtosByKeyCount == 6);

            Assert.IsTrue(new DummyDbAppSettings.DummyDbAppSettingDaoTestSetting1().InitialValue == 1);
            Assert.IsTrue(new DummyDbAppSettings.DummyDbAppSettingDaoTestSetting2().InitialValue == "TEST");
            Assert.IsTrue(new DummyDbAppSettings.DummyDbAppSettingDaoTestSetting3().InitialValue == false);

            Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting1.DefaultValue == 1);
            Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting2.DefaultValue == "TEST");
            Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting3.DefaultValue == false);

            Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting1.Value == 2);
            Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting2.Value == "NEW TEST");
            Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting3.Value == true);
        }

        [Test]
        public void SetSettingValue_AppSpecific()
        {
            Assert.IsTrue(new DummyDbAppSettings.DummyDbAppSettingDaoTestSetting1().InitialValue == 1);
            Assert.IsTrue(new DummyDbAppSettings.DummyDbAppSettingDaoTestSetting2().InitialValue == "TEST");
            Assert.IsTrue(new DummyDbAppSettings.DummyDbAppSettingDaoTestSetting3().InitialValue == false);

            Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting1.DefaultValue == 1);
            Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting2.DefaultValue == "TEST");
            Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting3.DefaultValue == false);

            SettingCache cache = SettingCache.Instance;
            Assert.IsNotNull(cache);

            cache.InitializeCache(new DummySettingInitialization(_dao, 200, "DbAppSettingApp"));
            SpinWait.SpinUntil(() =>
            {
                return _dao.HitCount > 5;
            });

            Assert.IsTrue(_dao.HitCount > 5);
            Assert.IsTrue(SettingCache.IsInitalized);
            Assert.IsNotNull(SettingCache.SettingWatchTask);
            Assert.IsTrue(SettingCache.SettingDtosByKeyCount == 3);

            Assert.IsTrue(new DummyDbAppSettings.DummyDbAppSettingDaoTestSetting1().InitialValue == 1);
            Assert.IsTrue(new DummyDbAppSettings.DummyDbAppSettingDaoTestSetting2().InitialValue == "TEST");
            Assert.IsTrue(new DummyDbAppSettings.DummyDbAppSettingDaoTestSetting3().InitialValue == false);

            Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting1.DefaultValue == 1);
            Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting2.DefaultValue == "TEST");
            Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting3.DefaultValue == false);

            Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting1.Value == 2);
            Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting2.Value == "NEW TEST");
            Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting3.Value == true);
        }


        [Test]
        public void SetSettingValue_RefreshedSettings()
        {
            Assert.IsTrue(new DummyDbAppSettings.DummyDbAppSettingDaoTestSetting1().InitialValue == 1);
            Assert.IsTrue(new DummyDbAppSettings.DummyDbAppSettingDaoTestSetting2().InitialValue == "TEST");
            Assert.IsTrue(new DummyDbAppSettings.DummyDbAppSettingDaoTestSetting3().InitialValue == false);

            Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting1.DefaultValue == 1);
            Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting2.DefaultValue == "TEST");
            Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting3.DefaultValue == false);

            SettingCache cache = SettingCache.Instance;
            Assert.IsNotNull(cache);

            cache.InitializeCache(new DummySettingInitialization(_dao, 200, "DbAppSettingApp"));
            SpinWait.SpinUntil(() =>
            {
                return _dao.HitCount > 5;
            });

            Assert.IsTrue(_dao.HitCount > 5);
            Assert.IsTrue(SettingCache.IsInitalized);
            Assert.IsNotNull(SettingCache.SettingWatchTask);

            Assert.IsTrue(new DummyDbAppSettings.DummyDbAppSettingDaoTestSetting1().InitialValue == 1);
            Assert.IsTrue(new DummyDbAppSettings.DummyDbAppSettingDaoTestSetting2().InitialValue == "TEST");
            Assert.IsTrue(new DummyDbAppSettings.DummyDbAppSettingDaoTestSetting3().InitialValue == false);

            Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting1.DefaultValue == 1);
            Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting2.DefaultValue == "TEST");
            Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting3.DefaultValue == false);

            Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting1.Value == 2);
            Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting2.Value == "NEW TEST");
            Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting3.Value == true);

            Assert.IsTrue(_dao.HitCount > 5);
            Assert.IsTrue(SettingCache.IsInitalized);
            Assert.IsNotNull(SettingCache.SettingWatchTask);

            Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting1.DefaultValue == 1);
            Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting2.DefaultValue == "TEST");
            Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting3.DefaultValue == false);

            Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting1.Value == 2);
            Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting2.Value == "NEW TEST");
            Assert.IsTrue(DummyDbAppSettings.DummyDbAppSettingDaoTestSetting3.Value == true);
        }
    }
}
