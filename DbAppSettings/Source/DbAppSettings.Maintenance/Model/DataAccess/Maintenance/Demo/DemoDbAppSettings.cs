using DbAppSettings.Model.Domain;

namespace DbAppSettings.Maintenance.Model.DataAccess.Maintenance.Demo
{
    internal class DemoDbAppSettings
    {
        public class DemoDbAppSettingBool : DbAppSetting<DemoDbAppSettingBool, bool> { public override bool InitialValue => true; }
        public class DemoDbAppSettingInt : DbAppSetting<DemoDbAppSettingInt, int> { public override int InitialValue => 100; }
        public class DemoDbAppSettingString : DbAppSetting<DemoDbAppSettingString, string> { public override string InitialValue => "Demo Test String"; }
    }
}
