using System;
using System.Collections.Generic;
using System.Linq;
using DbAppSettings.Model.DataAccess.Implementations;
using DbAppSettings.Model.DataTransfer;
using NUnit.Framework;

namespace DbAppSettings.Test.Model.DataAccess
{
    [TestFixture]
    public class DefaultRetrieveAllSettingDaoTest
    {
        [Test]
        public void GetAllDbAppSettings()
        {
            DefaultRetrieveAllSettingDao dao = new DefaultRetrieveAllSettingDao();
            Assert.IsNotNull(dao);

            IEnumerable<DbAppSettingDto> results = dao.GetAllDbAppSettings();
            Assert.IsNotNull(results);
            Assert.IsTrue(!results.Any());
        }

        [Test]
        public void GetChangedDbAppSettings()
        {
            DefaultRetrieveAllSettingDao dao = new DefaultRetrieveAllSettingDao();
            Assert.IsNotNull(dao);

            IEnumerable<DbAppSettingDto> results = dao.GetChangedDbAppSettings(DateTime.Now);
            Assert.IsNotNull(results);
            Assert.IsTrue(!results.Any());
        }
    }
}
