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

        public class decimalSetting : DbAppSetting<decimalSetting, decimal> { public override decimal InitialValue => 1.25M; }
        [Test]
        public void DbAppSetting_decimal()
        {
            decimalSetting setting = new decimalSetting();

            Assert.IsTrue(setting.InitialValue == 1.25M);
            Assert.IsTrue(setting.InternalValue == 1.25M);

            DbAppSettingDto settingDto = new DbAppSettingDto() { Key = setting.FullSettingName, Value = 5.10M.ToString(), Type = setting.TypeString };
            setting.From(settingDto);

            Assert.IsTrue(setting.InitialValue == 1.25M);
            Assert.IsTrue(setting.InternalValue == 5.10M);

            DbAppSettingDto toDto = setting.ToDto();
            Assert.IsTrue(toDto.ApplicationKey == settingDto.ApplicationKey);
            Assert.IsTrue(toDto.Key == settingDto.Key);
            Assert.IsTrue(toDto.Type == settingDto.Type);
            Assert.IsTrue(toDto.Value.Equals(settingDto.Value, StringComparison.InvariantCultureIgnoreCase));
        }

        public class doubleSetting : DbAppSetting<doubleSetting, double> { public override double InitialValue => 1.25; }
        [Test]
        public void DbAppSetting_double()
        {
            doubleSetting setting = new doubleSetting();

            Assert.IsTrue(setting.InitialValue == 1.25);
            Assert.IsTrue(setting.InternalValue == 1.25);

            DbAppSettingDto settingDto = new DbAppSettingDto() { Key = setting.FullSettingName, Value = 5.10.ToString(), Type = setting.TypeString };
            setting.From(settingDto);

            Assert.IsTrue(setting.InitialValue == 1.25);
            Assert.IsTrue(setting.InternalValue == 5.10);

            DbAppSettingDto toDto = setting.ToDto();
            Assert.IsTrue(toDto.ApplicationKey == settingDto.ApplicationKey);
            Assert.IsTrue(toDto.Key == settingDto.Key);
            Assert.IsTrue(toDto.Type == settingDto.Type);
            Assert.IsTrue(toDto.Value.Equals(settingDto.Value, StringComparison.InvariantCultureIgnoreCase));
        }

        public class floatSetting : DbAppSetting<floatSetting, float> { public override float InitialValue => 1.25F; }
        [Test]
        public void DbAppSetting_float()
        {
            floatSetting setting = new floatSetting();

            Assert.IsTrue(setting.InitialValue == 1.25F);
            Assert.IsTrue(setting.InternalValue == 1.25F);

            DbAppSettingDto settingDto = new DbAppSettingDto() { Key = setting.FullSettingName, Value = 5.10F.ToString(), Type = setting.TypeString };
            setting.From(settingDto);

            Assert.IsTrue(setting.InitialValue == 1.25F);
            Assert.IsTrue(setting.InternalValue == 5.10F);

            DbAppSettingDto toDto = setting.ToDto();
            Assert.IsTrue(toDto.ApplicationKey == settingDto.ApplicationKey);
            Assert.IsTrue(toDto.Key == settingDto.Key);
            Assert.IsTrue(toDto.Type == settingDto.Type);
            Assert.IsTrue(toDto.Value.Equals(settingDto.Value, StringComparison.InvariantCultureIgnoreCase));
        }

        public class intSetting : DbAppSetting<intSetting, int> { public override int InitialValue => 1; }
        [Test]
        public void DbAppSetting_int()
        {
            intSetting setting = new intSetting();

            Assert.IsTrue(setting.InitialValue == 1);
            Assert.IsTrue(setting.InternalValue == 1);

            DbAppSettingDto settingDto = new DbAppSettingDto() { Key = setting.FullSettingName, Value = 5.ToString(), Type = setting.TypeString };
            setting.From(settingDto);

            Assert.IsTrue(setting.InitialValue == 1);
            Assert.IsTrue(setting.InternalValue == 5);

            DbAppSettingDto toDto = setting.ToDto();
            Assert.IsTrue(toDto.ApplicationKey == settingDto.ApplicationKey);
            Assert.IsTrue(toDto.Key == settingDto.Key);
            Assert.IsTrue(toDto.Type == settingDto.Type);
            Assert.IsTrue(toDto.Value.Equals(settingDto.Value, StringComparison.InvariantCultureIgnoreCase));
        }

        public class longSetting : DbAppSetting<longSetting, long> { public override long InitialValue => 2147483648; }
        [Test]
        public void DbAppSetting_long()
        {
            longSetting setting = new longSetting();

            Assert.IsTrue(setting.InitialValue == 2147483648);
            Assert.IsTrue(setting.InternalValue == 2147483648);

            DbAppSettingDto settingDto = new DbAppSettingDto() { Key = setting.FullSettingName, Value = 2147483649.ToString(), Type = setting.TypeString };
            setting.From(settingDto);

            Assert.IsTrue(setting.InitialValue == 2147483648);
            Assert.IsTrue(setting.InternalValue == 2147483649);

            DbAppSettingDto toDto = setting.ToDto();
            Assert.IsTrue(toDto.ApplicationKey == settingDto.ApplicationKey);
            Assert.IsTrue(toDto.Key == settingDto.Key);
            Assert.IsTrue(toDto.Type == settingDto.Type);
            Assert.IsTrue(toDto.Value.Equals(settingDto.Value, StringComparison.InvariantCultureIgnoreCase));
        }

        public class sbyteSetting : DbAppSetting<sbyteSetting, sbyte> { public override sbyte InitialValue => -127; }
        [Test]
        public void DbAppSetting_sbyte()
        {
            sbyteSetting setting = new sbyteSetting();

            Assert.IsTrue(setting.InitialValue == -127);
            Assert.IsTrue(setting.InternalValue == -127);

            DbAppSettingDto settingDto = new DbAppSettingDto() { Key = setting.FullSettingName, Value = 127.ToString(), Type = setting.TypeString };
            setting.From(settingDto);

            Assert.IsTrue(setting.InitialValue == -127);
            Assert.IsTrue(setting.InternalValue == 127);

            DbAppSettingDto toDto = setting.ToDto();
            Assert.IsTrue(toDto.ApplicationKey == settingDto.ApplicationKey);
            Assert.IsTrue(toDto.Key == settingDto.Key);
            Assert.IsTrue(toDto.Type == settingDto.Type);
            Assert.IsTrue(toDto.Value.Equals(settingDto.Value, StringComparison.InvariantCultureIgnoreCase));
        }

        public class shortSetting : DbAppSetting<shortSetting, short> { public override short InitialValue => 1; }
        [Test]
        public void DbAppSetting_short()
        {
            shortSetting setting = new shortSetting();

            Assert.IsTrue(setting.InitialValue == 1);
            Assert.IsTrue(setting.InternalValue == 1);

            DbAppSettingDto settingDto = new DbAppSettingDto() { Key = setting.FullSettingName, Value = 2.ToString(), Type = setting.TypeString };
            setting.From(settingDto);

            Assert.IsTrue(setting.InitialValue == 1);
            Assert.IsTrue(setting.InternalValue == 2);

            DbAppSettingDto toDto = setting.ToDto();
            Assert.IsTrue(toDto.ApplicationKey == settingDto.ApplicationKey);
            Assert.IsTrue(toDto.Key == settingDto.Key);
            Assert.IsTrue(toDto.Type == settingDto.Type);
            Assert.IsTrue(toDto.Value.Equals(settingDto.Value, StringComparison.InvariantCultureIgnoreCase));
        }

        public class stringSetting : DbAppSetting<stringSetting, string> { public override string InitialValue => "test"; }
        [Test]
        public void DbAppSetting_string()
        {
            stringSetting setting = new stringSetting();

            Assert.IsTrue(setting.InitialValue == "test");
            Assert.IsTrue(setting.InternalValue == "test");

            DbAppSettingDto settingDto = new DbAppSettingDto() { Key = setting.FullSettingName, Value = "new value".ToString(), Type = setting.TypeString };
            setting.From(settingDto);

            Assert.IsTrue(setting.InitialValue == "test");
            Assert.IsTrue(setting.InternalValue == "new value");

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

        public class DateTimeSetting : DbAppSetting<DateTimeSetting, DateTime> { public override DateTime InitialValue => DateTime.Today; }
        [Test]
        public void DbAppSetting_DateTime()
        {
            DateTimeSetting setting = new DateTimeSetting();

            Assert.IsTrue(setting.InitialValue == DateTime.Today);
            Assert.IsTrue(setting.InternalValue == DateTime.Today);

            DbAppSettingDto settingDto = new DbAppSettingDto() { Key = setting.FullSettingName, Value = DateTime.Today.AddDays(1).ToString(), Type = setting.TypeString };
            setting.From(settingDto);

            Assert.IsTrue(setting.InitialValue == DateTime.Today);
            Assert.IsTrue(setting.InternalValue == DateTime.Today.AddDays(1));

            DbAppSettingDto toDto = setting.ToDto();
            Assert.IsTrue(toDto.ApplicationKey == settingDto.ApplicationKey);
            Assert.IsTrue(toDto.Key == settingDto.Key);
            Assert.IsTrue(toDto.Type == settingDto.Type);
            Assert.IsTrue(toDto.Value.Equals(settingDto.Value, StringComparison.InvariantCultureIgnoreCase));
        }

        public class GuidSetting : DbAppSetting<GuidSetting, Guid> { public override Guid InitialValue => new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"); }
        [Test]
        public void DbAppSetting_Guid()
        {
            GuidSetting setting = new GuidSetting();

            Assert.IsTrue(setting.InitialValue == new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"));
            Assert.IsTrue(setting.InternalValue == new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"));

            DbAppSettingDto settingDto = new DbAppSettingDto() { Key = setting.FullSettingName, Value = new Guid("1245fe4a-d402-451c-b9ed-9c1a04247482").ToString(), Type = setting.TypeString };
            setting.From(settingDto);

            Assert.IsTrue(setting.InitialValue == new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"));
            Assert.IsTrue(setting.InternalValue == new Guid("1245fe4a-d402-451c-b9ed-9c1a04247482"));

            DbAppSettingDto toDto = setting.ToDto();
            Assert.IsTrue(toDto.ApplicationKey == settingDto.ApplicationKey);
            Assert.IsTrue(toDto.Key == settingDto.Key);
            Assert.IsTrue(toDto.Type == settingDto.Type);
            Assert.IsTrue(toDto.Value.Equals(settingDto.Value, StringComparison.InvariantCultureIgnoreCase));
        }

        public class TimeSpanSetting : DbAppSetting<TimeSpanSetting, TimeSpan> { public override TimeSpan InitialValue => new TimeSpan(1, 1, 1, 1); }
        [Test]
        public void DbAppSetting_TimeSpan()
        {
            TimeSpanSetting setting = new TimeSpanSetting();

            Assert.IsTrue(setting.InitialValue == new TimeSpan(1, 1, 1, 1));
            Assert.IsTrue(setting.InternalValue == new TimeSpan(1, 1, 1, 1));

            DbAppSettingDto settingDto = new DbAppSettingDto() { Key = setting.FullSettingName, Value = new TimeSpan(2, 1, 1, 1).ToString(), Type = setting.TypeString };
            setting.From(settingDto);

            Assert.IsTrue(setting.InitialValue == new TimeSpan(1, 1, 1, 1));
            Assert.IsTrue(setting.InternalValue == new TimeSpan(2, 1, 1, 1));

            DbAppSettingDto toDto = setting.ToDto();
            Assert.IsTrue(toDto.ApplicationKey == settingDto.ApplicationKey);
            Assert.IsTrue(toDto.Key == settingDto.Key);
            Assert.IsTrue(toDto.Type == settingDto.Type);
            Assert.IsTrue(toDto.Value.Equals(settingDto.Value, StringComparison.InvariantCultureIgnoreCase));
        }

        public class uintSetting : DbAppSetting<uintSetting, uint> { public override uint InitialValue => 4294967295; }
        [Test]
        public void DbAppSetting_uint()
        {
            uintSetting setting = new uintSetting();

            Assert.IsTrue(setting.InitialValue == 4294967295);
            Assert.IsTrue(setting.InternalValue == 4294967295);

            DbAppSettingDto settingDto = new DbAppSettingDto() { Key = setting.FullSettingName, Value = 4294967294.ToString(), Type = setting.TypeString };
            setting.From(settingDto);

            Assert.IsTrue(setting.InitialValue == 4294967295);
            Assert.IsTrue(setting.InternalValue == 4294967294);

            DbAppSettingDto toDto = setting.ToDto();
            Assert.IsTrue(toDto.ApplicationKey == settingDto.ApplicationKey);
            Assert.IsTrue(toDto.Key == settingDto.Key);
            Assert.IsTrue(toDto.Type == settingDto.Type);
            Assert.IsTrue(toDto.Value.Equals(settingDto.Value, StringComparison.InvariantCultureIgnoreCase));
        }

        public class ulongSetting : DbAppSetting<ulongSetting, ulong> { public override ulong InitialValue => 18446744073709551615; }
        [Test]
        public void DbAppSetting_ulong()
        {
            ulongSetting setting = new ulongSetting();

            Assert.IsTrue(setting.InitialValue == 18446744073709551615);
            Assert.IsTrue(setting.InternalValue == 18446744073709551615);

            DbAppSettingDto settingDto = new DbAppSettingDto() { Key = setting.FullSettingName, Value = 18446744073709551614.ToString(), Type = setting.TypeString };
            setting.From(settingDto);

            Assert.IsTrue(setting.InitialValue == 18446744073709551615);
            Assert.IsTrue(setting.InternalValue == 18446744073709551614);

            DbAppSettingDto toDto = setting.ToDto();
            Assert.IsTrue(toDto.ApplicationKey == settingDto.ApplicationKey);
            Assert.IsTrue(toDto.Key == settingDto.Key);
            Assert.IsTrue(toDto.Type == settingDto.Type);
            Assert.IsTrue(toDto.Value.Equals(settingDto.Value, StringComparison.InvariantCultureIgnoreCase));
        }

        public class ushortSetting : DbAppSetting<ushortSetting, ushort> { public override ushort InitialValue => 65535; }
        [Test]
        public void DbAppSetting_ushort()
        {
            ushortSetting setting = new ushortSetting();

            Assert.IsTrue(setting.InitialValue == 65535);
            Assert.IsTrue(setting.InternalValue == 65535);

            DbAppSettingDto settingDto = new DbAppSettingDto() { Key = setting.FullSettingName, Value = 65534.ToString(), Type = setting.TypeString };
            setting.From(settingDto);

            Assert.IsTrue(setting.InitialValue == 65535);
            Assert.IsTrue(setting.InternalValue == 65534);

            DbAppSettingDto toDto = setting.ToDto();
            Assert.IsTrue(toDto.ApplicationKey == settingDto.ApplicationKey);
            Assert.IsTrue(toDto.Key == settingDto.Key);
            Assert.IsTrue(toDto.Type == settingDto.Type);
            Assert.IsTrue(toDto.Value.Equals(settingDto.Value, StringComparison.InvariantCultureIgnoreCase));
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
