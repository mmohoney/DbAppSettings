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
    }
}
