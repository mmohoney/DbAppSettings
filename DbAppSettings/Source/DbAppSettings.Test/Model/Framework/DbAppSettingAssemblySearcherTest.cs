using System;
using System.Collections.Generic;
using System.Linq;
using DbAppSettings.Model.Domain;
using DbAppSettings.Model.Service;
using NUnit.Framework;

namespace DbAppSettings.Test.Model.Framework
{
    [TestFixture]
    public class DbAppSettingAssemblySearcherTest
    {
        public class DbAppSettingAssemblySearcherTestSetting1 : DbAppSetting<DbAppSettingAssemblySearcherTestSetting1, int> { public override int InitialValue => 1; }
        public class DbAppSettingAssemblySearcherTestSetting2 : DbAppSetting<DbAppSettingAssemblySearcherTestSetting2, bool> { public override bool InitialValue => true; }

        [Test]
        public void GetGenericDbAppSettingsTest()
        {
            List<Type> results = DbAppSettingAssemblySearcher.GetGenericDbAppSettings();

            Assert.IsTrue(results.Count > 0);
            Assert.IsTrue(results.Any(r => r.Name.Contains("DbAppSettingAssemblySearcherTestSetting1")));
            Assert.IsTrue(results.Any(r => r.Name.Contains("DbAppSettingAssemblySearcherTestSetting2")));
        }
    }
}
