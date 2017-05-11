//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using DbAppSettings.Model.Service.CacheManager.Arguments;
//using DbAppSettings.Model.Service.SettingCacheProvider;
//using DbAppSettings.Test.Mock;
//using NUnit.Framework;

//namespace DbAppSettings.Test.Model.Service.SettingCacheProvider
//{
//    [TestFixture]
//    public class SettingCacheProviderBaseTest
//    {
//        [Test]
//        public void SettingWatchTaskAction_NoResults()
//        {
//            DummyLazyLoadSettingDao dao = new DummyLazyLoadSettingDao();
//            LazyLoadSettingCacheProvider provider = new LazyLoadSettingCacheProvider(new LazyLoadManagerArguments() { LazyLoadSettingDao = dao });
//            provider.InitalizeSettingWatchTask();

//            SpinWait.SpinUntil(() => dao.GetChangedDbAppSettingsHitCount > 0);

//            Assert.IsTrue(dao.GetChangedDbAppSettingsHitCount == 1);
//        }

//        [Test]
//        public void SettingWatchTaskAction_Results()
//        {
//            DummyReturnOneLazyLoadSettingDao dao = new DummyReturnOneLazyLoadSettingDao();
//            LazyLoadSettingCacheProvider provider = new LazyLoadSettingCacheProvider(new LazyLoadManagerArguments() { LazyLoadSettingDao = dao });
//            provider.SettingWatchTaskAction();

//            SpinWait.SpinUntil(() => SettingCacheProviderBase.SettingDtosByKey.Count > 0);

//            Assert.IsTrue(SettingCacheProviderBase.SettingDtosByKey.Count == 1);
//        }

//        [Test]
//        public void SettingWatchTaskAction_LastRefreshedTime()
//        {
//            DummyReturnOneLazyLoadSettingDao dao = new DummyReturnOneLazyLoadSettingDao();
//            LazyLoadSettingCacheProvider provider = new LazyLoadSettingCacheProvider(new LazyLoadManagerArguments() { LazyLoadSettingDao = dao });
//            Assert.IsNull(SettingCacheProviderBase.LastRefreshedTime);

//            provider.SettingWatchTaskAction();

//            SpinWait.SpinUntil(() => SettingCacheProviderBase.SettingDtosByKey.Count > 0);

//            Assert.IsNotNull(SettingCacheProviderBase.LastRefreshedTime);
//        }
//    }
//}
