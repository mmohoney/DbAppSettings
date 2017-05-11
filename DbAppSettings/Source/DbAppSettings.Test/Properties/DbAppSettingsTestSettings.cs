using DbAppSettings.Model.Domain;

namespace DbAppSettings.Test.Properties
{
    public class MySettingsClass
    {
        public int PropertyOne => 1;
        public string PropertyTwo => "Settings are fun.";
    }

    public class DbAppSettingsTestSettings
    {
        public class EnableLogging : DbAppSetting<EnableLogging, bool> { public override bool InitialValue => false; }
        public class MyCustomObjectSetting : DbAppSetting<MyCustomObjectSetting, MySettingsClass> { public override MySettingsClass InitialValue => new MySettingsClass(); }
    }
}
