using DbAppSettings.Model.Domain;

namespace DbAppSettings.Model.DataAccess.Maintenance.Demo
{
    internal class DemoDbAppSettings
    {
        public class DemoDbAppSettingBool : DbAppSetting<DemoDbAppSettingBool, bool> { public override bool DefaultValue => true; }
        public class DemoDbAppSettingInt : DbAppSetting<DemoDbAppSettingInt, int> { public override int DefaultValue => 100; }
        public class DemoDbAppSettingString : DbAppSetting<DemoDbAppSettingString, string> { public override string DefaultValue => "Demo Test String"; }
    }
}
