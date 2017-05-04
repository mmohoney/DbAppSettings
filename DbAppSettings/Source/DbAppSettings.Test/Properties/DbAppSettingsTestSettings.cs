using DbAppSettings.Model.Domain;

namespace DbAppSettings.Test.Mock
{
    public class DbAppSettingsTestSettings
    {
        public class EnableLogging : DbAppSetting<EnableLogging, bool> { public override bool InitialValue => false; }
        public class MySecondSetting : DbAppSetting<MySecondSetting, int> { public override int InitialValue => 1; }
    }
}
