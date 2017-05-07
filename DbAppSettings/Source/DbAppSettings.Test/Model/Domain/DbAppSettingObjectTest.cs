using System.Web.Script.Serialization;
using DbAppSettings.Model.DataTransfer;
using DbAppSettings.Model.Domain;
using NUnit.Framework;

namespace DbAppSettings.Test.Model.Domain
{
    [TestFixture]
    public class DbAppSettingObjectTest
    {
        public class MyTestClass
        {
            public int SomeProperty { get; set; } = 1;
            public string SomeOtherProperty { get; set; } = "Test";
        }

        public class MyTestClassSetting : DbAppSetting<MyTestClassSetting, MyTestClass> { public override MyTestClass InitialValue => new MyTestClass(); }

        private MyTestClassSetting GetSetting()
        {
            MyTestClassSetting setting = new MyTestClassSetting();
            Assert.IsNotNull(setting);

            return setting;
        }

        [Test]
        public void DbAppSetting_InstantiateTest()
        {
            MyTestClassSetting test = new MyTestClassSetting();
            Assert.IsNotNull(test);
        }

        [Test]
        public void DbAppSetting_InternalValue()
        {
            MyTestClassSetting setting = GetSetting();
            Assert.IsNotNull(setting);
            Assert.IsNotNull(setting.InternalValue);
            Assert.IsTrue(setting.InitialValue.SomeProperty == 1);
            Assert.IsTrue(setting.InitialValue.SomeOtherProperty == "Test");
        }

        [Test]
        public void DbAppSetting_From()
        {
            MyTestClassSetting setting = GetSetting();

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
    }
}
