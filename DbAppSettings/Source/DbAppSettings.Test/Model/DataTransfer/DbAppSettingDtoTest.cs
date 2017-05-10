using DbAppSettings.Model.DataTransfer;
using NUnit.Framework;

namespace DbAppSettings.Test.Model.DataTransfer
{
    [TestFixture]
    public class DbAppSettingDtoTest
    {
        [Test]
        public void Test()
        {
            DbAppSettingDto dto = new DbAppSettingDto() { Key = "MyAssembly.Key"};
            Assert.IsTrue(dto.Key == "MyAssembly.Key");
            Assert.IsTrue(dto.Assembly == "MyAssembly");
        }
    }
}
