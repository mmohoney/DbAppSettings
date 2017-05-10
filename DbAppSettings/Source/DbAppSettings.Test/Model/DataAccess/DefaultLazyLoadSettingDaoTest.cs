using System;
using System.Collections.Generic;
using System.Linq;
using DbAppSettings.Model.DataAccess.Implementations;
using DbAppSettings.Model.DataTransfer;
using NUnit.Framework;

namespace DbAppSettings.Test.Model.DataAccess
{
    [TestFixture]
    public class DefaultLazyLoadSettingDaoTest
    {
        [Test]
        public void GetDbAppSetting()
        {
            DefaultLazyLoadSettingDao dao = new DefaultLazyLoadSettingDao();
            Assert.IsNotNull(dao);

            IEnumerable<DbAppSettingDto> results = dao.GetDbAppSetting(new DbAppSettingDto());
            Assert.IsNotNull(results);
            Assert.IsTrue(!results.Any());
        }

        [Test]
        public void GetChangedDbAppSettings()
        {
            DefaultLazyLoadSettingDao dao = new DefaultLazyLoadSettingDao();
            Assert.IsNotNull(dao);

            IEnumerable<DbAppSettingDto> results = dao.GetChangedDbAppSettings(DateTime.Now);
            Assert.IsNotNull(results);
            Assert.IsTrue(!results.Any());
        }
    }
}
