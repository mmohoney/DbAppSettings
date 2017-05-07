using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Script.Serialization;
using DbAppSettings.Model.DataTransfer;
using DbAppSettings.Model.Domain;
using NUnit.Framework;

namespace DbAppSettings.Test.Model.Domain
{
    [TestFixture]
    public class DbAppSettingTypeTest
    {
        public class MyTestClass
        {
            public int SomeProperty { get; set; } = 1;
            public string SomeOtherProperty { get; set; } = "Test";
        }


        public class MyTestClassSetting : DbAppSetting<MyTestClassSetting, MyTestClass> { public override MyTestClass InitialValue => new MyTestClass(); }
        public class StringCollectionSetting : DbAppSetting<StringCollectionSetting, StringCollection> { public override StringCollection InitialValue => new StringCollection { "One", "Two", "Three" }; }

        [Test]
        public void DbAppSetting_InstantiateTest()
        {
            MyTestClassSetting test = new MyTestClassSetting();
            Assert.IsNotNull(test);
        }

        [Test]
        public void DbAppSetting_InternalValue()
        {
            MyTestClassSetting setting = new MyTestClassSetting();
            Assert.IsNotNull(setting);
            Assert.IsNotNull(setting.InternalValue);
            Assert.IsTrue(setting.InitialValue.SomeProperty == 1);
            Assert.IsTrue(setting.InitialValue.SomeOtherProperty == "Test");
        }

        [Test]
        public void DbAppSetting_From()
        {
            MyTestClassSetting setting = new MyTestClassSetting();

            Assert.IsNotNull(setting.InitialValue);
            Assert.IsTrue(setting.InitialValue.SomeProperty == 1);
            Assert.IsTrue(setting.InitialValue.SomeOtherProperty == "Test");

            Assert.IsNotNull(setting.InternalValue);
            Assert.IsTrue(setting.InternalValue.SomeProperty == 1);
            Assert.IsTrue(setting.InternalValue.SomeOtherProperty == "Test");

            MyTestClass testClass = new MyTestClass();
            testClass.SomeProperty = 2;
            testClass.SomeOtherProperty = "Test2";

            string jsonTestClass = new JavaScriptSerializer().Serialize(testClass);

            DbAppSettingDto settingDto = new DbAppSettingDto() { Key = "MyTestClass", Value = jsonTestClass, Type = typeof(object).FullName };
            setting.From(settingDto);

            Assert.IsNotNull(setting.InitialValue);
            Assert.IsTrue(setting.InitialValue.SomeProperty == 1);
            Assert.IsTrue(setting.InitialValue.SomeOtherProperty == "Test");

            Assert.IsNotNull(setting.InternalValue);
            Assert.IsTrue(setting.InternalValue.SomeProperty == 2);
            Assert.IsTrue(setting.InternalValue.SomeOtherProperty == "Test2");
        }

        [Test]
        public void StringCollectionSetting_From()
        {
            StringCollectionSetting setting = new StringCollectionSetting();
            List<string> initialStringList = new List<string> { "One", "Two", "Three" };

            Assert.IsNotNull(setting.InitialValue);
            List<string> initialValueList = setting.InitialValue.Cast<string>().ToList();
            Assert.IsTrue(initialStringList.SequenceEqual(initialValueList));

            Assert.IsNotNull(setting.InternalValue);
            List<string> internalValueList = setting.InternalValue.Cast<string>().ToList();
            Assert.IsTrue(initialStringList.SequenceEqual(internalValueList));

            StringCollection newCollection = new StringCollection { "t", "3", "2" };
            string jsonNewCollection = InternalDbAppSettingBase.ConvertStringCollectionToJson(newCollection);

            DbAppSettingDto settingDto = new DbAppSettingDto() { Key = "StringCollectionSetting", Value = jsonNewCollection, Type = typeof(StringCollection).FullName };
            setting.From(settingDto);

            Assert.IsNotNull(setting.InitialValue);
            initialValueList = setting.InitialValue.Cast<string>().ToList();
            Assert.IsTrue(initialStringList.SequenceEqual(initialValueList));

            List<string> newStringList = new List<string> { "t", "3", "2" };

            Assert.IsNotNull(setting.InternalValue);
            internalValueList = setting.InternalValue.Cast<string>().ToList();
            Assert.IsTrue(newStringList.SequenceEqual(internalValueList));
        }
    }
}
