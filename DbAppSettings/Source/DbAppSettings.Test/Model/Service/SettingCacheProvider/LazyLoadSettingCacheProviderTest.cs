using System;
using System.Collections.Generic;
using System.Threading;
using DbAppSettings.Model.DataAccess.Interfaces;
using DbAppSettings.Model.DataTransfer;
using DbAppSettings.Model.Domain;
using DbAppSettings.Model.Service.CacheManager.Arguments;
using DbAppSettings.Model.Service.SettingCacheProvider;
using DbAppSettings.Test.Mock;
using NUnit.Framework;

namespace DbAppSettings.Test.Model.Service.SettingCacheProvider
{
    [TestFixture]
    public class LazyLoadSettingCacheProviderTest : ProviderTestBase
    {
        class DummyLazyLoadSettingDao : ILazyLoadSettingDao
        {
            public int GetDbAppSettingHitCount { get; set; }
            public DbAppSettingDto GetDbAppSetting(DbAppSettingDto dbAppSettingDto)
            {
                GetDbAppSettingHitCount++;
                return new DbAppSettingDto();
            }

            public int GetChangedDbAppSettingsHitCount { get; set; }
            public IEnumerable<DbAppSettingDto> GetChangedDbAppSettings(DateTime? latestDbAppSettingChangedDate)
            {
                GetChangedDbAppSettingsHitCount++;
                return new List<DbAppSettingDto>();
            }
        }

        class DummyReturnOneLazyLoadSettingDao : ILazyLoadSettingDao
        {
            public DbAppSettingDto GetDbAppSetting(DbAppSettingDto dbAppSettingDto)
            {
                return new DbAppSettingDto();
            }

            public IEnumerable<DbAppSettingDto> GetChangedDbAppSettings(DateTime? latestDbAppSettingChangedDate)
            {
                return new List<DbAppSettingDto>
                {
                    new DbAppSettingDto() { Key = new DummyDbAppSettings.DummyDbAppSettingDaoTestSetting1().FullSettingName, Value = "2", Type = typeof(int).FullName, ApplicationKey = "DbAppSettingApp" },
                };
            }
        }

        class DummyReturnDbAppSettingTestSettingLazyLoadSettingDao : ILazyLoadSettingDao
        {
            public int GetDbAppSettingHitCount { get; set; }
            public DbAppSettingDto GetDbAppSetting(DbAppSettingDto dbAppSettingDto)
            {
                GetDbAppSettingHitCount++;

                DbAppSettingDto dto = new DbAppSettingTestSetting().ToDto();
                dto.Value = "100";
                return dto;
            }

            public IEnumerable<DbAppSettingDto> GetChangedDbAppSettings(DateTime? latestDbAppSettingChangedDate)
            {
                return new List<DbAppSettingDto>
                {
                    new DbAppSettingDto() { Key = new DummyDbAppSettings.DummyDbAppSettingDaoTestSetting1().FullSettingName, Value = "2", Type = typeof(int).FullName, ApplicationKey = "DbAppSettingApp" },
                };
            }
        }

        [Test]
        public void ManagerArguments()
        {
            LazyLoadSettingCacheProvider provider = new LazyLoadSettingCacheProvider(new LazyLoadManagerArguments());
            Assert.IsNotNull(provider.ManagerArguments);
            Assert.IsTrue(provider.ManagerArguments.GetType() == typeof(LazyLoadManagerArguments));
        }

        [Test]
        public void InitializeSettingCacheProvider()
        {
            LazyLoadSettingCacheProvider provider = new LazyLoadSettingCacheProvider(new LazyLoadManagerArguments());
            provider.InitializeSettingCacheProvider();
            Assert.IsFalse(provider.IsInitalized);
        }

        [Test]
        public void SettingWatchTaskAction_NoResults()
        {
            DummyLazyLoadSettingDao dao = new DummyLazyLoadSettingDao();
            LazyLoadSettingCacheProvider provider = new LazyLoadSettingCacheProvider(new LazyLoadManagerArguments() {LazyLoadSettingDao = dao, CacheRefreshTimeout = () => TimeSpan.FromMilliseconds(0) });
            provider.InitalizeSettingWatchTask();

            SpinWait.SpinUntil(() => dao.GetChangedDbAppSettingsHitCount > 0);

            Assert.IsTrue(dao.GetChangedDbAppSettingsHitCount == 1);
        }

        [Test]
        public void SettingWatchTaskAction_Results()
        {
            DummyReturnOneLazyLoadSettingDao dao = new DummyReturnOneLazyLoadSettingDao();
            LazyLoadSettingCacheProvider provider = new LazyLoadSettingCacheProvider(new LazyLoadManagerArguments() { LazyLoadSettingDao = dao, CacheRefreshTimeout = () => TimeSpan.FromMilliseconds(0) });
            provider.InitalizeSettingWatchTask();

            SpinWait.SpinUntil(() => SettingCacheProviderBase.SettingDtosByKey.Count > 0);

            Assert.IsTrue(SettingCacheProviderBase.SettingDtosByKey.Count == 1);
        }

        //[Test]
        //public void SettingWatchTaskAction_LastRefreshedTime()
        //{
        //    DummyReturnOneLazyLoadSettingDao dao = new DummyReturnOneLazyLoadSettingDao();
        //    LazyLoadSettingCacheProvider provider = new LazyLoadSettingCacheProvider(new LazyLoadManagerArguments() { LazyLoadSettingDao = dao, CacheRefreshTimeout = () => TimeSpan.FromMilliseconds(0)});
        //    Assert.IsNull(SettingCacheProviderBase.LastRefreshedTime);

        //    provider.InitalizeSettingWatchTask();

        //    SpinWait.SpinUntil(() => SettingCacheProviderBase.LastRefreshedTime.HasValue && SettingCacheProviderBase.LastRefreshedTime.Value > DateTime.MinValue);

        //    Assert.IsNotNull(SettingCacheProviderBase.LastRefreshedTime);
        //}

        [Test]
        public void GetDbAppSetting_NotInitialized()
        {
            Assert.Throws(typeof(Exception), () =>
            {
                DummyReturnOneLazyLoadSettingDao dao = new DummyReturnOneLazyLoadSettingDao();
                LazyLoadSettingCacheProvider provider = new LazyLoadSettingCacheProvider(new LazyLoadManagerArguments() { LazyLoadSettingDao = dao });

                provider.GetDbAppSetting<DbAppSettingTestSetting, int>();
            });
        }

        [Test]
        public void GetDbAppSettingInitialized()
        {
            DummyReturnDbAppSettingTestSettingLazyLoadSettingDao dao = new DummyReturnDbAppSettingTestSettingLazyLoadSettingDao();
            LazyLoadSettingCacheProvider provider = new LazyLoadSettingCacheProvider(new LazyLoadManagerArguments() { LazyLoadSettingDao = dao });
            SettingCacheProviderBase.Initalized = true;

            Assert.IsTrue(SettingCacheProviderBase.SettingDtosByKey.Count == 0);
            DbAppSetting<DbAppSettingTestSetting, int> result = provider.GetDbAppSetting<DbAppSettingTestSetting, int>();
            Assert.IsTrue(SettingCacheProviderBase.SettingDtosByKey.Count == 1);
            Assert.IsTrue(result.InternalValue == 100);
            Assert.IsTrue(dao.GetDbAppSettingHitCount == 1);
        }

        [Test]
        public void GetDbAppSettingInitialized_SecondHit()
        {
            DummyReturnDbAppSettingTestSettingLazyLoadSettingDao dao = new DummyReturnDbAppSettingTestSettingLazyLoadSettingDao();
            LazyLoadSettingCacheProvider provider = new LazyLoadSettingCacheProvider(new LazyLoadManagerArguments() { LazyLoadSettingDao = dao });
            SettingCacheProviderBase.Initalized = true;

            Assert.IsTrue(SettingCacheProviderBase.SettingDtosByKey.Count == 0);
            DbAppSetting<DbAppSettingTestSetting, int> result = provider.GetDbAppSetting<DbAppSettingTestSetting, int>();
            Assert.IsTrue(SettingCacheProviderBase.SettingDtosByKey.Count == 1);
            Assert.IsTrue(result.InternalValue == 100);
            Assert.IsTrue(dao.GetDbAppSettingHitCount == 1);

            DbAppSetting<DbAppSettingTestSetting, int> result2 = provider.GetDbAppSetting<DbAppSettingTestSetting, int>();
            Assert.IsTrue(SettingCacheProviderBase.SettingDtosByKey.Count == 1);
            Assert.IsTrue(result.InternalValue == 100);
            Assert.IsTrue(dao.GetDbAppSettingHitCount == 1);
        }
    }
}
