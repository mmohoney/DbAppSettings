using System.Collections.Generic;
using DbAppSettings.Model.DataTransfer;
using DbAppSettings.Model.Domain;

namespace DbAppSettings.Test.Mock
{
    public class DummyDbAppSettings
    {
        public class DummyDbAppSettingDaoTestSetting1 : DbAppSetting<DummyDbAppSettingDaoTestSetting1, int> { public override int DefaultValue => 1; }
        public class DummyDbAppSettingDaoTestSetting2 : DbAppSetting<DummyDbAppSettingDaoTestSetting2, string> { public override string DefaultValue => "TEST"; }
        public class DummyDbAppSettingDaoTestSetting3 : DbAppSetting<DummyDbAppSettingDaoTestSetting3, bool> { public override bool DefaultValue => false; }
        public class DummyDbAppSettingDaoTestSetting4 : DbAppSetting<DummyDbAppSettingDaoTestSetting4, bool> { public override bool DefaultValue => false; }
        public class DummyDbAppSettingDaoTestSetting5 : DbAppSetting<DummyDbAppSettingDaoTestSetting5, bool> { public override bool DefaultValue => false; }
        public class DummyDbAppSettingDaoTestSetting6 : DbAppSetting<DummyDbAppSettingDaoTestSetting6, bool> { public override bool DefaultValue => false; }
    }

    public class DummyDbAppSettingsDtos
    {
        public IEnumerable<DbAppSettingDto> GetAllDbAppSettings()
        {
            return new List<DbAppSettingDto>
            {
                new DbAppSettingDto() { Key = new DummyDbAppSettings.DummyDbAppSettingDaoTestSetting1().FullSettingName, Value = "2", Type = typeof(int).FullName, ApplicationKey = "DbAppSettingApp" },
                new DbAppSettingDto() { Key = new DummyDbAppSettings.DummyDbAppSettingDaoTestSetting2().FullSettingName, Value = "NEW TEST", Type = typeof(string).FullName, ApplicationKey = "DbAppSettingApp" },
                new DbAppSettingDto() { Key = new DummyDbAppSettings.DummyDbAppSettingDaoTestSetting3().FullSettingName, Value = "true", Type = typeof(bool).FullName, ApplicationKey = "DbAppSettingApp" },

                new DbAppSettingDto() { Key = new DummyDbAppSettings.DummyDbAppSettingDaoTestSetting4().FullSettingName, Value = "true", Type = typeof(bool).FullName, ApplicationKey = "DbAppSettingApp_1" },
                new DbAppSettingDto() { Key = new DummyDbAppSettings.DummyDbAppSettingDaoTestSetting5().FullSettingName, Value = "false", Type = typeof(bool).FullName, ApplicationKey = "DbAppSettingApp_1" },
                new DbAppSettingDto() { Key = new DummyDbAppSettings.DummyDbAppSettingDaoTestSetting6().FullSettingName, Value = "true", Type = typeof(bool).FullName, ApplicationKey = "DbAppSettingApp_1" },
            };
        }
    }
}
