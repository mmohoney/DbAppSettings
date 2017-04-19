using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using DbAppSettings.Model.DataTransfer;
using DbAppSettings.Model.Domain;
using DbAppSettings.Model.Service;
using NUnit.Framework;

namespace DbAppSettings.Test.Model.Domain
{
    [TestFixture]
    public class DbAppSettingTest
    {
        public class DbAppSettingTestSetting : DbAppSetting<DbAppSettingTestSetting, int> { public override int DefaultValue => 1; }

        [Test]
        public void DbAppSetting_InstantiateTest()
        {
            DbAppSettingTestSetting test = new DbAppSettingTestSetting();
            Assert.IsNotNull(test);
        }

        [Test]
        public void DbAppSetting_GenericInstantiateTest()
        {
            List<Type> results = DbAppSettingAssemblySearcher.GetGenericDbAppSettings();

            Type myType = results.Find(r => r.Name.Contains("DbAppSettingTestSetting"));
            Assert.IsNotNull(myType);

            object newObject = Activator.CreateInstance(myType);
            Assert.IsNotNull(newObject);
        }

        [Test]
        public void DbAppSetting_AssemblyName()
        {
            List<Type> results = DbAppSettingAssemblySearcher.GetGenericDbAppSettings();

            Type myType = results.Find(r => r.Name.Contains("DbAppSettingTestSetting"));
            Assert.IsNotNull(myType);

            object newObject = Activator.CreateInstance(myType);
            Assert.IsNotNull(newObject);

            object assemblyName = myType.GetProperty("AssemblyName").GetValue(newObject, null);
            Assert.IsNotNull(assemblyName);
            Assert.IsTrue(assemblyName is string);
            Assert.AreEqual((string)assemblyName, Assembly.GetExecutingAssembly().GetName().Name);
        }

        [Test]
        public void DbAppSetting_SettingName()
        {
            List<Type> results = DbAppSettingAssemblySearcher.GetGenericDbAppSettings();

            Type myType = results.Find(r => r.Name.Contains("DbAppSettingTestSetting"));
            Assert.IsNotNull(myType);

            object newObject = Activator.CreateInstance(myType);
            Assert.IsNotNull(newObject);

            object settingName = myType.GetProperty("SettingName").GetValue(newObject, null);
            Assert.IsNotNull(settingName);
            Assert.IsTrue(settingName is string);
            Assert.AreEqual((string)settingName, "DbAppSettingTestSetting");
        }

        [Test]
        public void DbAppSetting_FullSettingName()
        {
            List<Type> results = DbAppSettingAssemblySearcher.GetGenericDbAppSettings();

            Type myType = results.Find(r => r.Name.Contains("DbAppSettingTestSetting"));
            Assert.IsNotNull(myType);

            object newObject = Activator.CreateInstance(myType);
            Assert.IsNotNull(newObject);

            object fullSettingName = myType.GetProperty("FullSettingName").GetValue(newObject, null);
            Assert.IsNotNull(fullSettingName);
            Assert.IsTrue(fullSettingName is string);
            Assert.AreEqual((string)fullSettingName, $"{Assembly.GetExecutingAssembly().GetName().Name}.DbAppSettingTestSetting");
        }

        [Test]
        public void DbAppSetting_GenericInstantiateTest_Property_DefaultValue_ByReflection()
        {
            List<Type> results = DbAppSettingAssemblySearcher.GetGenericDbAppSettings();

            Type myType = results.Find(r => r.Name.Contains("DbAppSettingTestSetting"));
            Assert.IsNotNull(myType);

            object newObject = Activator.CreateInstance(myType);
            Assert.IsNotNull(newObject);

            object defaultValue = myType.GetProperty("DefaultValue").GetValue(newObject, null);
            Assert.IsNotNull(defaultValue);
            Assert.IsTrue(defaultValue is int);
            Assert.IsTrue((int)defaultValue == 1);
        }

        [Test]
        public void DbAppSetting_GenericInstantiateTest_Property_Value_ByReflection()
        {
            List<Type> results = DbAppSettingAssemblySearcher.GetGenericDbAppSettings();

            Type myType = results.Find(r => r.Name.Contains("DbAppSettingTestSetting"));
            Assert.IsNotNull(myType);

            object newObject = Activator.CreateInstance(myType);
            Assert.IsNotNull(newObject);

            object value = myType.GetProperty("InternalValue", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(newObject, null);
            Assert.IsNotNull(value);
            Assert.IsTrue(value is int);
            Assert.IsTrue((int)value == default(int));
        }

        [Test]
        public void DbAppSetting_GenericInstantiateTest_Property_ValueTypeString_ByReflection()
        {
            List<Type> results = DbAppSettingAssemblySearcher.GetGenericDbAppSettings();

            Type myType = results.Find(r => r.Name.Contains("DbAppSettingTestSetting"));
            Assert.IsNotNull(myType);

            object newObject = Activator.CreateInstance(myType);
            Assert.IsNotNull(newObject);

            object value = myType.GetProperty("TypeString", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(newObject, null);
            Assert.IsNotNull(value);
            Assert.IsTrue(value is string);
            Assert.IsTrue((string)value == typeof(int).FullName);
        }

        [Test]
        public void DbAppSetting_GenericInstantiateTest_Method_From_ByReflection()
        {
            List<Type> results = DbAppSettingAssemblySearcher.GetGenericDbAppSettings();

            Type myType = results.Find(r => r.Name.Contains("DbAppSettingTestSetting"));
            Assert.IsNotNull(myType);

            object newObject = Activator.CreateInstance(myType);
            Assert.IsNotNull(newObject);

            object defaultValue = myType.GetProperty("DefaultValue").GetValue(newObject, null);
            Assert.IsNotNull(defaultValue);
            Assert.IsTrue(defaultValue is int);
            Assert.IsTrue((int)defaultValue == 1);

            object value = myType.GetProperty("InternalValue", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(newObject, null);
            Assert.IsNotNull(value);
            Assert.IsTrue(value is int);
            Assert.IsTrue((int)value == default(int));

            DbAppSettingDto settingDto = new DbAppSettingDto() {Key = "DbAppSettingTest1", Value = "2", Type = typeof(int).FullName };
            MethodInfo fromMethod = newObject.GetType().GetMethod("From", BindingFlags.Instance | BindingFlags.NonPublic);
            fromMethod.Invoke(newObject, new object[] { settingDto });

            defaultValue = myType.GetProperty("DefaultValue").GetValue(newObject, null);
            Assert.IsNotNull(defaultValue);
            Assert.IsTrue(defaultValue is int);
            Assert.IsTrue((int)defaultValue == 1);

            value = myType.GetProperty("InternalValue", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(newObject, null);
            Assert.IsNotNull(value);
            Assert.IsTrue(value is int);
            Assert.IsTrue((int)value == 2);
        }

        [Test]
        public void ConvertStringCollectionToXml()
        {
            Random randNum = new Random();
            int[] randomInts = Enumerable
                .Repeat(0, 1000)
                .Select(i => randNum.Next(0, int.MaxValue))
                .ToArray();

            StringCollection inputStringCollection = new StringCollection();
            inputStringCollection.AddRange(randomInts.Select(t => t.ToString()).ToArray());
            Assert.IsTrue(inputStringCollection.Count == randomInts.Length);

            string resultXml = InternalDbAppSettingBase.ConvertStringCollectionToXml(inputStringCollection);
            Assert.IsNotNull(resultXml);

            try
            {
                XDocument xd1 = XDocument.Parse(resultXml);
                Assert.IsNotNull(xd1);
            }
            catch (XmlException)
            {
                Assert.IsTrue(false);
            }
        }

        [Test]
        public void ConvertXmlToStringCollection()
        {
            Random randNum = new Random();
            int[] randomInts = Enumerable
                .Repeat(0, 1000)
                .Select(i => randNum.Next(0, int.MaxValue))
                .ToArray();

            StringCollection inputStringCollection = new StringCollection();
            inputStringCollection.AddRange(randomInts.Select(t => t.ToString()).ToArray());
            Assert.IsTrue(inputStringCollection.Count == randomInts.Length);

            string resultXml = InternalDbAppSettingBase.ConvertStringCollectionToXml(inputStringCollection);
            Assert.IsNotNull(resultXml);

            StringCollection resultStringCollection = InternalDbAppSettingBase.ConvertXmlToStringCollection(resultXml);
            Assert.IsNotNull(resultStringCollection);

            List<string> inputToList = inputStringCollection.Cast<string>().ToList();
            List<string> resultToList = resultStringCollection.Cast<string>().ToList();

            Assert.IsTrue(inputToList.SequenceEqual(resultToList));
        }
    }
}
