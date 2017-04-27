using DbAppSettings.Model.Domain;
using NUnit.Framework;

namespace DbAppSettings.Test.Model.Domain
{
    [TestFixture]
    public class DbAppSettingDictionaryTest
    {
        public class DbAppSettingDictionaryTestSetting : DbAppSetting<DbAppSettingDictionaryTestSetting, int> { public override int InitialValue => 1; }

        [Test]
        public void Create()
        {
            DbAppSettingDictionary dictionary = DbAppSettingDictionary.Create();
            Assert.IsNotNull(dictionary);
        }

        [Test]
        public void ContainsKey_String()
        {
            DbAppSettingDictionary dictionary = DbAppSettingDictionary.Create();
            Assert.IsNotNull(dictionary);

            Assert.IsFalse(dictionary.ContainsKey(new DbAppSettingDictionaryTestSetting().FullSettingName));
        }

        [Test]
        public void ContainsKey_DbAppSetting()
        {
            DbAppSettingDictionary dictionary = DbAppSettingDictionary.Create();
            Assert.IsNotNull(dictionary);

            Assert.IsFalse(dictionary.ContainsKey<DbAppSettingDictionaryTestSetting, int>());
        }

        [Test]
        public void Add_ContainsKey_String()
        {
            DbAppSettingDictionary dictionary = DbAppSettingDictionary.Create();
            Assert.IsNotNull(dictionary);

            DbAppSettingDictionaryTestSetting setting = new DbAppSettingDictionaryTestSetting();
            dictionary.Add(setting.FullSettingName, setting);

            Assert.IsTrue(dictionary.ContainsKey(new DbAppSettingDictionaryTestSetting().FullSettingName));
        }

        [Test]
        public void Add_ContainsKey_DbAppSetting()
        {
            DbAppSettingDictionary dictionary = DbAppSettingDictionary.Create();
            Assert.IsNotNull(dictionary);

            DbAppSettingDictionaryTestSetting setting = new DbAppSettingDictionaryTestSetting();
            dictionary.Add(setting.FullSettingName, setting);

            Assert.IsTrue(dictionary.ContainsKey<DbAppSettingDictionaryTestSetting, int>());
        }

        [Test]
        public void Add_KeyObject()
        {
            DbAppSettingDictionary dictionary = DbAppSettingDictionary.Create();
            Assert.IsNotNull(dictionary);

            DbAppSettingDictionaryTestSetting setting = new DbAppSettingDictionaryTestSetting();
            dictionary.Add(setting.FullSettingName, setting);

            Assert.IsTrue(dictionary.ContainsKey<DbAppSettingDictionaryTestSetting, int>());
        }

        [Test]
        public void Add_KeyDbAppSetting()
        {
            DbAppSettingDictionary dictionary = DbAppSettingDictionary.Create();
            Assert.IsNotNull(dictionary);

            DbAppSettingDictionaryTestSetting setting = new DbAppSettingDictionaryTestSetting();
            dictionary.Add(setting);

            Assert.IsTrue(dictionary.ContainsKey<DbAppSettingDictionaryTestSetting, int>());
        }

        [Test]
        public void Add_ContainsKey_Get_String()
        {
            DbAppSettingDictionary dictionary = DbAppSettingDictionary.Create();
            Assert.IsNotNull(dictionary);

            DbAppSettingDictionaryTestSetting setting = new DbAppSettingDictionaryTestSetting();
            dictionary.Add(setting.FullSettingName, setting);

            Assert.IsTrue(dictionary.ContainsKey<DbAppSettingDictionaryTestSetting, int>());

            object result = dictionary.Get(new DbAppSettingDictionaryTestSetting().FullSettingName);
            Assert.IsNotNull(result);
        }

        [Test]
        public void Add_ContainsKey_GetValue()
        {
            DbAppSettingDictionary dictionary = DbAppSettingDictionary.Create();
            Assert.IsNotNull(dictionary);

            DbAppSettingDictionaryTestSetting setting = new DbAppSettingDictionaryTestSetting();
            dictionary.Add(setting.FullSettingName, setting);

            Assert.IsTrue(dictionary.ContainsKey<DbAppSettingDictionaryTestSetting, int>());

            DbAppSetting<DbAppSettingDictionaryTestSetting, int> result = dictionary.Get<DbAppSettingDictionaryTestSetting, int>();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.InitialValue == setting.InitialValue);
        }
    }
}
