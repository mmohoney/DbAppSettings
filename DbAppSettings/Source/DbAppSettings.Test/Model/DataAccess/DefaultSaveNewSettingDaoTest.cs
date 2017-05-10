using DbAppSettings.Model.DataAccess.Implementations;
using DbAppSettings.Model.DataTransfer;
using NUnit.Framework;

namespace DbAppSettings.Test.Model.DataAccess
{
    [TestFixture]
    public class DefaultSaveNewSettingDaoTest
    {
        [Test]
        public void SaveNewSettingIfNotExists()
        {
            DefaultSaveNewSettingDao dao = new DefaultSaveNewSettingDao();
            Assert.IsNotNull(dao);

            dao.SaveNewSettingIfNotExists(new DbAppSettingDto());
        }
    }
}
