using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using DbAppSettings.Model.DataTransfer;
using DbAppSettings.Model.Domain;
using NUnit.Framework;

namespace DbAppSettings.Test.Model.Domain
{
    [TestFixture]
    public class DbAppSettingTest
    {
        public class DbAppSettingTestSetting : DbAppSetting<DbAppSettingTestSetting, int> { public override int InitialValue => 1; }

        private DbAppSettingTestSetting GetSetting()
        {
            DbAppSettingTestSetting setting = new DbAppSettingTestSetting();
            Assert.IsNotNull(setting);

            return setting;
        }

        [Test]
        public void DbAppSetting_InstantiateTest()
        {
            DbAppSettingTestSetting test = new DbAppSettingTestSetting();
            Assert.IsNotNull(test);
        }

        [Test]
        public void DbAppSetting_GenericInstantiateTest()
        {
            DbAppSettingTestSetting setting = GetSetting();
            Assert.IsNotNull(setting);
        }

        [Test]
        public void DbAppSetting_AssemblyName()
        {
            DbAppSettingTestSetting setting = GetSetting();
            Assert.IsNotNull(setting);
            Assert.IsNotNull(setting.AssemblyName);
            Assert.AreEqual(setting.AssemblyName, Assembly.GetExecutingAssembly().GetName().Name);
        }

        [Test]
        public void DbAppSetting_SettingName()
        {
            DbAppSettingTestSetting setting = GetSetting();
            Assert.IsNotNull(setting);
            Assert.IsNotNull(setting.SettingName);
            Assert.AreEqual(setting.SettingName, "DbAppSettingTestSetting");
        }

        [Test]
        public void DbAppSetting_FullSettingName()
        {
            DbAppSettingTestSetting setting = GetSetting();
            Assert.IsNotNull(setting);
            Assert.IsNotNull(setting.FullSettingName);
            Assert.AreEqual(setting.FullSettingName, $"{Assembly.GetExecutingAssembly().GetName().Name}.DbAppSettingTestSetting");
        }

        [Test]
        public void DbAppSetting_InitialValue()
        {
            DbAppSettingTestSetting setting = GetSetting();
            Assert.IsNotNull(setting);
            Assert.IsNotNull(setting.InitialValue);
            Assert.IsTrue(setting.InitialValue == 1);
        }

        [Test]
        public void DbAppSetting_Assembly()
        {
            DbAppSettingTestSetting setting = GetSetting();
            Assert.IsNotNull(setting);
            Assert.IsNull(setting.ApplicationKey);
        }

        [Test]
        public void DbAppSetting_InternalValue()
        {
            DbAppSettingTestSetting setting = GetSetting();
            Assert.IsNotNull(setting);
            Assert.IsNotNull(setting.InternalValue);
            Assert.IsTrue(setting.InitialValue == 1);
        }

        [Test]
        public void DbAppSetting_TypeString()
        {
            DbAppSettingTestSetting setting = GetSetting();
            Assert.IsNotNull(setting);
            Assert.IsNotNull(setting.TypeString);
            Assert.IsTrue(setting.TypeString == typeof(int).FullName);
        }

        [Test]
        public void DbAppSetting_From()
        {
            DbAppSettingTestSetting setting = GetSetting();

            Assert.IsNotNull(setting.InitialValue);
            Assert.IsTrue(setting.InitialValue == 1);

            Assert.IsNotNull(setting.InternalValue);
            Assert.IsTrue(setting.InitialValue == 1);

            DbAppSettingDto settingDto = new DbAppSettingDto() {Key = "DbAppSettingTest1", Value = "2", Type = typeof(int).FullName };
            setting.From(settingDto);

            Assert.IsNotNull(setting.InitialValue);
            Assert.IsTrue(setting.InitialValue == 1);

            Assert.IsNotNull(setting.InternalValue);
            Assert.IsTrue(setting.InternalValue == 2);
        }

        [Test]
        public void ConvertStringCollectionToJson()
        {
            Random randNum = new Random();
            int[] randomInts = Enumerable
                .Repeat(0, 1000)
                .Select(i => randNum.Next(0, int.MaxValue))
                .ToArray();

            StringCollection inputStringCollection = new StringCollection();
            inputStringCollection.AddRange(randomInts.Select(t => t.ToString()).ToArray());
            Assert.IsTrue(inputStringCollection.Count == randomInts.Length);

            string result = InternalDbAppSettingBase.ConvertStringCollectionToJson(inputStringCollection);
            Assert.IsNotNull(result);
        }

        [Test]
        public void ConvertJsonToStringCollection()
        {
            Random randNum = new Random();
            int[] randomInts = Enumerable
                .Repeat(0, 1000)
                .Select(i => randNum.Next(0, int.MaxValue))
                .ToArray();

            StringCollection inputStringCollection = new StringCollection();
            inputStringCollection.AddRange(randomInts.Select(t => t.ToString()).ToArray());
            Assert.IsTrue(inputStringCollection.Count == randomInts.Length);

            string result = InternalDbAppSettingBase.ConvertStringCollectionToJson(inputStringCollection);
            Assert.IsNotNull(result);

            StringCollection resultStringCollection = InternalDbAppSettingBase.ConvertJsonToStringCollection(result);
            Assert.IsNotNull(resultStringCollection);

            List<string> inputToList = inputStringCollection.Cast<string>().ToList();
            List<string> resultToList = resultStringCollection.Cast<string>().ToList();

            Assert.IsTrue(inputToList.SequenceEqual(resultToList));
        }
    }
}
