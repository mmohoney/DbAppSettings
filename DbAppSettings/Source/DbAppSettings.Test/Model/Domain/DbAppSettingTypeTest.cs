using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
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

        public class boolSetting : DbAppSetting<boolSetting, bool> { public override bool InitialValue => true; }
        [Test]
        public void DbAppSetting_bool()
        {
            boolSetting setting = new boolSetting();

            Assert.IsTrue(setting.InitialValue == true);
            Assert.IsTrue(setting.InternalValue == true);

            DbAppSettingDto settingDto = new DbAppSettingDto() { Key = setting.FullSettingName, Value = false.ToString(), Type = setting.TypeString };
            setting.From(settingDto);

            Assert.IsTrue(setting.InitialValue == true);
            Assert.IsTrue(setting.InternalValue == false);

            DbAppSettingDto toDto = setting.ToDto();
            Assert.IsTrue(toDto.ApplicationKey == settingDto.ApplicationKey);
            Assert.IsTrue(toDto.Key == settingDto.Key);
            Assert.IsTrue(toDto.Type == settingDto.Type);
            Assert.IsTrue(toDto.Value.Equals(settingDto.Value, StringComparison.InvariantCultureIgnoreCase));
        }

        public class byteSetting : DbAppSetting<byteSetting, byte> { public override byte InitialValue => Encoding.ASCII.GetBytes("test")[0]; }
        [Test]
        public void DbAppSetting_byte()
        {
            byteSetting setting = new byteSetting();

            Assert.IsTrue(setting.InitialValue == Encoding.ASCII.GetBytes("test")[0]);
            Assert.IsTrue(setting.InternalValue == Encoding.ASCII.GetBytes("test")[0]);

            DbAppSettingDto settingDto = new DbAppSettingDto() { Key = setting.FullSettingName, Value = Encoding.ASCII.GetBytes("test")[1].ToString(), Type = setting.TypeString };
            setting.From(settingDto);

            Assert.IsTrue(setting.InitialValue == Encoding.ASCII.GetBytes("test")[0]);
            Assert.IsTrue(setting.InternalValue == Encoding.ASCII.GetBytes("test")[1]);

            DbAppSettingDto toDto = setting.ToDto();
            Assert.IsTrue(toDto.ApplicationKey == settingDto.ApplicationKey);
            Assert.IsTrue(toDto.Key == settingDto.Key);
            Assert.IsTrue(toDto.Type == settingDto.Type);
            Assert.IsTrue(toDto.Value.Equals(settingDto.Value, StringComparison.InvariantCultureIgnoreCase));
        }

        public class charSetting : DbAppSetting<charSetting, char> { public override char InitialValue => 'a'; }
        [Test]
        public void DbAppSetting_char()
        {
            charSetting setting = new charSetting();

            Assert.IsTrue(setting.InitialValue == 'a');
            Assert.IsTrue(setting.InternalValue == 'a');

            DbAppSettingDto settingDto = new DbAppSettingDto() { Key = setting.FullSettingName, Value = 'b'.ToString(), Type = setting.TypeString };
            setting.From(settingDto);

            Assert.IsTrue(setting.InitialValue == 'a');
            Assert.IsTrue(setting.InternalValue == 'b');

            DbAppSettingDto toDto = setting.ToDto();
            Assert.IsTrue(toDto.ApplicationKey == settingDto.ApplicationKey);
            Assert.IsTrue(toDto.Key == settingDto.Key);
            Assert.IsTrue(toDto.Type == settingDto.Type);
            Assert.IsTrue(toDto.Value.Equals(settingDto.Value, StringComparison.InvariantCultureIgnoreCase));
        }






        public class StringCollectionSetting : DbAppSetting<StringCollectionSetting, StringCollection> { public override StringCollection InitialValue => new StringCollection { "One", "Two", "Three" }; }
        [Test]
        public void DbAppSetting_StringCollection()
        {
            List<string> initialStringList = new List<string> { "One", "Two", "Three" };
            StringCollectionSetting setting = new StringCollectionSetting();

            List<string> initialValueList = setting.InitialValue.Cast<string>().ToList();
            Assert.IsTrue(initialStringList.SequenceEqual(initialValueList));
            List<string> internalValueList = setting.InternalValue.Cast<string>().ToList();
            Assert.IsTrue(initialStringList.SequenceEqual(internalValueList));

            StringCollection newCollection = new StringCollection { "t", "3", "2" };
            List<string> newStringList = new List<string> { "t", "3", "2" };
            string jsonNewCollection = InternalDbAppSettingBase.ConvertStringCollectionToJson(newCollection);

            DbAppSettingDto settingDto = new DbAppSettingDto() { Key = setting.FullSettingName, Value = jsonNewCollection, Type = setting.TypeString };
            setting.From(settingDto);

            initialValueList = setting.InitialValue.Cast<string>().ToList();
            Assert.IsTrue(initialStringList.SequenceEqual(initialValueList));
            internalValueList = setting.InternalValue.Cast<string>().ToList();
            Assert.IsTrue(newStringList.SequenceEqual(internalValueList));
        }

        public class MyTestClassSetting : DbAppSetting<MyTestClassSetting, MyTestClass> { public override MyTestClass InitialValue => new MyTestClass(); }
        [Test]
        public void DbAppSetting_customObect()
        {
            MyTestClassSetting setting = new MyTestClassSetting();

            Assert.IsTrue(setting.InitialValue.SomeProperty == 1);
            Assert.IsTrue(setting.InitialValue.SomeOtherProperty == "Test");
            Assert.IsTrue(setting.InternalValue.SomeProperty == 1);
            Assert.IsTrue(setting.InternalValue.SomeOtherProperty == "Test");

            MyTestClass testClass = new MyTestClass();
            testClass.SomeProperty = 2;
            testClass.SomeOtherProperty = "Test2";

            string jsonTestClass = new JavaScriptSerializer().Serialize(testClass);

            DbAppSettingDto settingDto = new DbAppSettingDto() { Key = setting.FullSettingName, Value = jsonTestClass, Type = setting.TypeString };
            setting.From(settingDto);

            Assert.IsTrue(setting.InitialValue.SomeProperty == 1);
            Assert.IsTrue(setting.InitialValue.SomeOtherProperty == "Test");
            Assert.IsTrue(setting.InternalValue.SomeProperty == 2);
            Assert.IsTrue(setting.InternalValue.SomeOtherProperty == "Test2");

            DbAppSettingDto toDto = setting.ToDto();
            Assert.IsTrue(toDto.ApplicationKey == settingDto.ApplicationKey);
            Assert.IsTrue(toDto.Key == settingDto.Key);
            Assert.IsTrue(toDto.Type == settingDto.Type);
            Assert.IsTrue(toDto.Value.Equals(settingDto.Value, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
