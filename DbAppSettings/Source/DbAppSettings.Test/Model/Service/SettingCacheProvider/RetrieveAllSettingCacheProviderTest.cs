using System;
using System.Collections.Generic;
using System.Threading;
using DbAppSettings.Model.DataAccess.Interfaces;
using DbAppSettings.Model.DataTransfer;
using DbAppSettings.Model.Domain;
using DbAppSettings.Model.Service.CacheManager.Arguments;
using DbAppSettings.Model.Service.SettingCacheProvider;
using NUnit.Framework;

namespace DbAppSettings.Test.Model.Service.SettingCacheProvider
{
    [TestFixture]
    public class RetrieveAllSettingCacheProviderTest : ProviderTestBase
    {
        class DummyDbAppSettingDaoTestSetting1 : DbAppSetting<DummyDbAppSettingDaoTestSetting1, int> { public override int InitialValue => 1; }

        class DummyRetrieveAllSettingDao : IRetrieveAllSettingDao
        {
            public int GetAllDbAppSettingsHitCount { get; set; }
            public IEnumerable<DbAppSettingDto> GetAllDbAppSettings()
            {
                GetAllDbAppSettingsHitCount++;
                return new List<DbAppSettingDto>();
            }

            public int GetChangedDbAppSettingsHitCount { get; set; }
            public IEnumerable<DbAppSettingDto> GetChangedDbAppSettings(DateTime? latestDbAppSettingChangedDate)
            {
                GetChangedDbAppSettingsHitCount++;
                return new List<DbAppSettingDto>();
            }
        }

        class DummyReturnOneRetrieveAllSettingDao : IRetrieveAllSettingDao
        {
            public IEnumerable<DbAppSettingDto> GetAllDbAppSettings()
            {
                return new List<DbAppSettingDto>();
            }

            public IEnumerable<DbAppSettingDto> GetChangedDbAppSettings(DateTime? latestDbAppSettingChangedDate)
            {
                return new List<DbAppSettingDto>
                {
                    new DbAppSettingDto() { Key = new DummyDbAppSettingDaoTestSetting1().FullSettingName, Value = "2", Type = typeof(int).FullName, ApplicationKey = "DbAppSettingApp" },
                };
            }
        }

        [Test]
        public void ManagerArguments()
        {
            RetrieveAllSettingCacheProvider provider = new RetrieveAllSettingCacheProvider(new RetrieveAllManagerArguments());
            Assert.IsNotNull(provider.ManagerArguments);
            Assert.IsTrue(provider.ManagerArguments.GetType() == typeof(RetrieveAllManagerArguments));
        }

        [Test]
        public void InitializeSettingCacheProvider()
        {
            RetrieveAllSettingCacheProvider provider = new RetrieveAllSettingCacheProvider(new RetrieveAllManagerArguments());
            provider.InitializeSettingCacheProvider();
            Assert.IsFalse(provider.IsInitalized);
        }

        [Test]
        public void SettingWatchTaskAction_NoResults()
        {
            DummyRetrieveAllSettingDao dao = new DummyRetrieveAllSettingDao();
            RetrieveAllSettingCacheProvider provider = new RetrieveAllSettingCacheProvider(new RetrieveAllManagerArguments() { RetrieveAllSettingDao = dao, CacheRefreshTimeout = () => TimeSpan.FromMilliseconds(0) });
            provider.InitalizeSettingWatchTask();

            SpinWait.SpinUntil(() => dao.GetChangedDbAppSettingsHitCount > 0);

            Assert.IsTrue(dao.GetChangedDbAppSettingsHitCount > 1);
        }

        [Test]
        public void SettingWatchTaskAction_Results()
        {
            DummyReturnOneRetrieveAllSettingDao dao = new DummyReturnOneRetrieveAllSettingDao();
            RetrieveAllSettingCacheProvider provider = new RetrieveAllSettingCacheProvider(new RetrieveAllManagerArguments() { RetrieveAllSettingDao = dao, CacheRefreshTimeout = () => TimeSpan.FromMilliseconds(0) });
            provider.InitalizeSettingWatchTask();

            SpinWait.SpinUntil(() => SettingCacheProviderBase.SettingDtosByKey.Count > 0);

            Assert.IsTrue(SettingCacheProviderBase.SettingDtosByKey.Count == 1);
        }

        [Test]
        public void GetDbAppSetting_NotInitialized()
        {
            Assert.Throws(typeof(Exception), () =>
            {
                DummyReturnOneRetrieveAllSettingDao dao = new DummyReturnOneRetrieveAllSettingDao();
                RetrieveAllSettingCacheProvider provider = new RetrieveAllSettingCacheProvider(new RetrieveAllManagerArguments() { RetrieveAllSettingDao = dao, CacheRefreshTimeout = () => TimeSpan.FromMilliseconds(0) });

                provider.GetDbAppSetting<DbAppSettingTestSetting, int>();
            });
        }

        [Test]
        public void GetDbAppSettingInitialized()
        {
            DummyReturnOneRetrieveAllSettingDao dao = new DummyReturnOneRetrieveAllSettingDao();
            RetrieveAllSettingCacheProvider provider = new RetrieveAllSettingCacheProvider(new RetrieveAllManagerArguments() { RetrieveAllSettingDao = dao, CacheRefreshTimeout = () => TimeSpan.FromMilliseconds(0) });
            SettingCacheProviderBase.Initalized = true;

            DbAppSettingDto dto = new DbAppSettingTestSetting().ToDto();
            dto.Value = "100";

            SettingCacheProviderBase.SettingDtosByKey.AddOrUpdate(dto.Key, dto, (key, oldValue) => dto);

            Assert.IsTrue(SettingCacheProviderBase.SettingDtosByKey.Count > 0);
            DbAppSetting<DbAppSettingTestSetting, int> result = provider.GetDbAppSetting<DbAppSettingTestSetting, int>();
            Assert.IsTrue(result.InternalValue == 100);
        }
    }
}
