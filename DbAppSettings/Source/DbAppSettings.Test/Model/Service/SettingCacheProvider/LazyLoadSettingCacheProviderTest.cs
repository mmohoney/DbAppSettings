using System;
using System.Collections.Generic;
using System.Threading;
using DbAppSettings.Model.DataAccess.Interfaces;
using DbAppSettings.Model.DataTransfer;
using DbAppSettings.Model.Service.CacheManager.Arguments;
using DbAppSettings.Model.Service.SettingCacheProvider;
using DbAppSettings.Test.Mock;
using NUnit.Framework;

namespace DbAppSettings.Test.Model.Service.SettingCacheProvider
{
    [TestFixture]
    public class LazyLoadSettingCacheProviderTest
    {
        class DummyLazyLoadSettingDao : ILazyLoadSettingDao
        {
            public int GetDbAppSettingHitCount { get; set; }
            public IEnumerable<DbAppSettingDto> GetDbAppSetting(DbAppSettingDto dbAppSettingDto)
            {
                GetDbAppSettingHitCount++;
                return new List<DbAppSettingDto>();
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
            public IEnumerable<DbAppSettingDto> GetDbAppSetting(DbAppSettingDto dbAppSettingDto)
            {
                return new List<DbAppSettingDto>();
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
            LazyLoadSettingCacheProvider provider = new LazyLoadSettingCacheProvider(new LazyLoadManagerArguments() {LazyLoadSettingDao = dao });
            provider.SettingWatchTaskAction();

            SpinWait.SpinUntil(() => dao.GetChangedDbAppSettingsHitCount > 0);

            Assert.IsTrue(dao.GetChangedDbAppSettingsHitCount == 1);
        }

        [Test]
        public void SettingWatchTaskAction_Results()
        {
            DummyReturnOneLazyLoadSettingDao dao = new DummyReturnOneLazyLoadSettingDao();
            LazyLoadSettingCacheProvider provider = new LazyLoadSettingCacheProvider(new LazyLoadManagerArguments() { LazyLoadSettingDao = dao });
            provider.SettingWatchTaskAction();

            SpinWait.SpinUntil(() => SettingCacheProviderBase.SettingDtosByKey.Count > 0);

            Assert.IsTrue(SettingCacheProviderBase.SettingDtosByKey.Count == 1);
        }

        [Test]
        public void SettingWatchTaskAction_LastRefreshedTime()
        {
            DummyReturnOneLazyLoadSettingDao dao = new DummyReturnOneLazyLoadSettingDao();
            LazyLoadSettingCacheProvider provider = new LazyLoadSettingCacheProvider(new LazyLoadManagerArguments() { LazyLoadSettingDao = dao });
            Assert.IsNull(SettingCacheProviderBase.LastRefreshedTime);

            provider.SettingWatchTaskAction();

            SpinWait.SpinUntil(() => SettingCacheProviderBase.SettingDtosByKey.Count > 0);

            Assert.IsNotNull(SettingCacheProviderBase.LastRefreshedTime);
        }
    }
}
